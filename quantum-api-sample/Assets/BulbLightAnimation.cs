using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using Quantum;
using UnityEngine.UI;
using Spine.Unity;
using Spine.Unity.AttachmentTools;
public unsafe class BulbLightAnimation : MonoBehaviour // THB
{
    [SerializeField] private Animator _animator = null;

    private PlayerRef _playerRef = default;
    private EntityRef _entityRef = default;
    private QuantumGame _game = null;

    private int TriggerIdle = Animator.StringToHash("IdleTrigger");
    private int TriggerWalk = Animator.StringToHash("WalkTrigger");
    private int TriggerAttack = Animator.StringToHash("AttackTrigger");
    private int TriggerHit = Animator.StringToHash("HitTrigger");
    private int FloatBlendOnOff = Animator.StringToHash("BlendEnergyFloat");
    private int oldAnim;

    private Quaternion qInitRot;
    private Transform tCameraTransform;

    public Slider sliderEnergy;
    private bool bDontMoveWhileAnim;
    public SkeletonRenderer skeletonRenderer;
    public Spine.PathConstraint constraintX;
    public float posX;
    public Spine.Slot slotArmR;
    public Spine.Attachment saveAttach;
    // This method is registered to the EntityView's OnEntityInstantiated event located on the parent GameObject
    public void Initialize(PlayerRef playerRef, EntityRef entityRef)
    {
        oldAnim = TriggerIdle;

        _playerRef = playerRef;
        _entityRef = entityRef;
        _game = QuantumRunner.Default.Game;

        // Set up Quantum events
        QuantumEvent.Subscribe<Quantum.EventPlayerAttack>(this, Attack);
        QuantumEvent.Subscribe<Quantum.EventPlayerHit>(this, Hit);
        QuantumEvent.Subscribe<EventOnRegenTick>(this, OnRegenTick);

        sliderEnergy.minValue = 0f;
        sliderEnergy.maxValue = 1f;
        sliderEnergy.value = 0.5f;
        _animator.SetFloat(FloatBlendOnOff, 0.5f);

        bDontMoveWhileAnim = false;

        constraintX = skeletonRenderer.skeleton.PathConstraints.Items[0];
        posX = constraintX.Position;


        slotArmR = skeletonRenderer.skeleton.FindSlot("Arm_R");
        saveAttach = slotArmR.Attachment;

    }
    private void Start()
    {
        qInitRot = transform.rotation;
        tCameraTransform = Camera.main.transform;
    }


    void LateUpdate()
    {
        if (_game.Frames.Verified.IsPredicted) return;
        transform.rotation = tCameraTransform.rotation * qInitRot; // THB dont rotate this child
        sliderEnergy.value = _animator.GetFloat(FloatBlendOnOff); // get the amount lerped in real time by the animator


        float whereTo = (transform.localEulerAngles.y - 180.0f) / 360.0f;
        //if (Mathf.Abs(constraintX.Position - whereTo) > 0.5f) Debug.Log("lerp de ouf");
        constraintX.Position = Mathf.Lerp(constraintX.Position, whereTo, Time.deltaTime * 8f);
    }
    void Update()
    {
        if (_game.Frames.Verified.IsPredicted) return;

        MovementAnimation();
    }

    private void MovementAnimation()
    {
        var kcc = _game.Frames.Verified.Unsafe.GetPointer<CharacterController3D>(_entityRef);
        if (bDontMoveWhileAnim) kcc->Velocity = FPVector3.Zero;
        else
        {
            bool isMoving = kcc->Velocity.Magnitude.AsFloat > 0.2f;

            if (isMoving)
            {
                if (oldAnim == TriggerIdle) ChangeAnim(TriggerWalk);
            }
            else
            {
                if (oldAnim == TriggerWalk) ChangeAnim(TriggerIdle);
            }
        }

    }

    private void Attack(EventPlayerAttack e)
    {
        if (e.PlayerRef != _playerRef) return;
        if (oldAnim != TriggerAttack) ChangeAnim(TriggerAttack);
        slotArmR.Attachment = null;
    }
    private void Hit(EventPlayerHit e)
    {
        if (e.PlayerRef != _playerRef) return;
        if (oldAnim != TriggerHit) ChangeAnim(TriggerHit);
        slotArmR.Attachment = saveAttach;
    }
    private void OnRegenTick(EventOnRegenTick e)
    {
        if (e.Target != _entityRef) return;
        //sliderEnergy.value = (float)e.Amount;
        _animator.SetFloat(FloatBlendOnOff, Mathf.Min(1f, (float)e.Amount / 100f), 0.05f, Time.deltaTime);


    }

    private void ChangeAnim(int idAnim)
    {
        if (idAnim == TriggerWalk)
        {
            oldAnim = TriggerWalk;
            //Debug.Log("walking");
            _animator.ResetTrigger(TriggerIdle);
            _animator.SetTrigger(TriggerWalk);
            _animator.ResetTrigger(TriggerAttack);
            StopCoroutine(WaitEndOfAttack(oldAnim));
            _animator.ResetTrigger(TriggerHit);
            StopCoroutine(WaitEndOfHit(oldAnim));
        }
        else if (idAnim == TriggerIdle)
        {
            oldAnim = TriggerIdle;
            //Debug.Log("idleing");
            _animator.SetTrigger(TriggerIdle);
            _animator.ResetTrigger(TriggerWalk);
            _animator.ResetTrigger(TriggerAttack);
            StopCoroutine(WaitEndOfAttack(oldAnim));
            _animator.ResetTrigger(TriggerHit);
            StopCoroutine(WaitEndOfHit(oldAnim));
        }
        else if (idAnim == TriggerAttack)
        {
            //oldAnim = TriggerAttack;
            //Debug.Log("attacking");
            _animator.ResetTrigger(TriggerIdle);
            _animator.ResetTrigger(TriggerWalk);
            _animator.SetTrigger(TriggerAttack);
            StopCoroutine(WaitEndOfAttack(oldAnim));
            StartCoroutine(WaitEndOfAttack(oldAnim));
            _animator.ResetTrigger(TriggerHit);
            StopCoroutine(WaitEndOfHit(oldAnim));
        }
        else if (idAnim == TriggerHit)
        {
            //oldAnim = TriggerAttack;
            //Debug.Log("hit");
            _animator.ResetTrigger(TriggerIdle);
            _animator.ResetTrigger(TriggerWalk);
            _animator.ResetTrigger(TriggerAttack);
            StopCoroutine(WaitEndOfAttack(oldAnim));
            _animator.SetTrigger(TriggerHit);
            StopCoroutine(WaitEndOfHit(oldAnim));
            StartCoroutine(WaitEndOfHit(oldAnim));
        }
        else
        {
            oldAnim = -1;
            _animator.ResetTrigger(TriggerIdle);
            _animator.ResetTrigger(TriggerWalk);
            _animator.ResetTrigger(TriggerAttack);
            StopCoroutine(WaitEndOfAttack(oldAnim));
            _animator.ResetTrigger(TriggerHit);
            StopCoroutine(WaitEndOfHit(oldAnim));
        }
    }

    private IEnumerator WaitEndOfAttack(int WhichTrigger)
    {
        bDontMoveWhileAnim = true;
        yield return new WaitForSeconds(0.333f);
        bDontMoveWhileAnim = false;
        ChangeAnim(WhichTrigger);
    }
    private IEnumerator WaitEndOfHit(int WhichTrigger)
    {
        bDontMoveWhileAnim = true;
        yield return new WaitForSeconds(0.167f);
        bDontMoveWhileAnim = false;
        ChangeAnim(WhichTrigger);
    }
}
