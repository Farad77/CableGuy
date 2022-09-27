using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using Quantum;

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
    private int oldAnim;

    private Quaternion qInitRot;

    // This method is registered to the EntityView's OnEntityInstantiated event located on the parent GameObject
    public void Initialize(PlayerRef playerRef, EntityRef entityRef)
    {
        qInitRot = transform.rotation;
        oldAnim = TriggerIdle;

        _playerRef = playerRef;
        _entityRef = entityRef;
        _game = QuantumRunner.Default.Game;

        // Set up Quantum events
        QuantumEvent.Subscribe<Quantum.EventPlayerAttack>(this, Attack);
        QuantumEvent.Subscribe<Quantum.EventPlayerHit>(this, Hit);
    }

    void LateUpdate()
    {
        if (_game.Frames.Verified.IsPredicted) return;

        transform.rotation = qInitRot; // THB dont rotate this child
    }
    void Update()
    {
        if (_game.Frames.Verified.IsPredicted) return;

        MovementAnimation();
    }

    private void MovementAnimation()
    {
        var kcc = _game.Frames.Verified.Unsafe.GetPointer<CharacterController3D>(_entityRef);
        bool isMoving = kcc->Velocity.Magnitude.AsFloat > 0.2f;

        if (isMoving)
        {
            if(oldAnim == TriggerIdle) ChangeAnim(TriggerWalk);
        }
        else
        {
            if (oldAnim == TriggerWalk) ChangeAnim(TriggerIdle);
        }
        //_animator.SetBool(BOOL_IS_MOVING, isMoving);
        //_animator.SetBool(BOOL_IS_GROUNDED, kcc->Grounded);

    }

    private void Attack(EventPlayerAttack e)
    {
        if (e.PlayerRef != _playerRef) return;
        if(oldAnim != TriggerAttack) ChangeAnim(TriggerAttack);
    }
    private void Hit(EventPlayerHit e)
    {
        if (e.PlayerRef != _playerRef) return; // THB e.Target.Index ça marche ça?
        _animator.SetTrigger(TriggerHit);
    }
    /*private void Jump(EventPlayerJump e)
    {
        if (e.PlayerRef != _playerRef) return;
        _animator.SetTrigger(TRIGGER_JUMP);
    }*/

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
            Debug.Log("attacking");
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
            Debug.Log("hit");
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
        yield return new WaitForSeconds(0.249925f);
        ChangeAnim(WhichTrigger);
    }
    private IEnumerator WaitEndOfHit(int WhichTrigger)
    {
        yield return new WaitForSeconds(0.249925f);
        ChangeAnim(WhichTrigger);
    }
}
