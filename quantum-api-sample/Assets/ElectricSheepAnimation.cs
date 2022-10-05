using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using Quantum;
using UnityEngine.UI;
using Spine.Unity;
using Spine.Unity.AttachmentTools;

public unsafe class ElectricSheepAnimation : MonoBehaviour
{
    private EntityRef _entityRef = default;

    private Quaternion qInitRot;
    private Transform tCameraTransform;
    private NavMeshSteeringAgent* _navMeshSteeringAgent = default;

    [SerializeField] private Animator _animator = null;
    private int TriggerWalk = Animator.StringToHash("WalkTrigger");
    private int TriggerDeath = Animator.StringToHash("DeathTrigger");
    private int TriggerHit = Animator.StringToHash("HitTrigger");
    private int oldAnim;

    // This method is registered to the EntityView's OnEntityInstantiated event located on the parent GameObject
    public void Initialize(EntityRef entityRef)
    {

        _entityRef = entityRef;

        // Set up Quantum events
        QuantumEvent.Subscribe<EventOnDamageDealt>(this, TakeDamage);

        oldAnim = TriggerWalk;
        //qInitRot = transform.parent.rotation;
    }
    private void TakeDamage(EventOnDamageDealt obj)
    {
        if (obj.Target != _entityRef) return;
        _animator.SetTrigger(TriggerHit);
    }

    private void Start()
    {
        qInitRot = transform.rotation;
        tCameraTransform = Camera.main.transform;
    }

    void LateUpdate()
    {
        //if (_game.Frames.Verified.IsPredicted) return;
        if (transform.localEulerAngles.y > 0 && transform.localEulerAngles.y < 180)
        {
            //Debug.Log("look right");
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else if (transform.localEulerAngles.y > 180 && transform.localEulerAngles.y < 360)
        {
            //Debug.Log("look left");
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        //transform.parent.rotation = qInitRot; // THB dont rotate this child
        transform.rotation = tCameraTransform.rotation * qInitRot;
    }

    public void Death()
    {
        _animator.SetTrigger(TriggerDeath);
        StartCoroutine(WaitB4Death());
    }
    public IEnumerator WaitB4Death()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
