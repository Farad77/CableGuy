using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using Quantum;
using UnityEngine.UI;

public unsafe class ElectricSheepAnimation : MonoBehaviour
{
    private EntityRef _entityRef = default;

    private Quaternion qInitRot;
    private NavMeshSteeringAgent* _navMeshSteeringAgent = default;

    // This method is registered to the EntityView's OnEntityInstantiated event located on the parent GameObject
    public void Initialize(EntityRef entityRef)
    {
        qInitRot = transform.rotation;

        _entityRef = entityRef;

        // Set up Quantum events
        QuantumEvent.Subscribe<EventOnDamageDealt>(this, TakeDamage);

    }
    private void TakeDamage(EventOnDamageDealt obj)
    {
        if (obj.Target != _entityRef) return;
        //_animator.SetTrigger(TRIGGER_DAMAGE_HIT);
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
        transform.rotation = qInitRot; // THB dont rotate this child
    }

    public void Death()
    {
        /*_animator.SetFloat(FLOAT_MOVEMENT_SPEED, 0.0f);
        _animator.SetFloat(FLOAT_MOVEMENT_HORIZONTAL, 0.0f);
        _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, 0.0f);

        _animator.SetTrigger(TRIGGER_DEATH);*/

        //Destroy(gameObject, _deathAnimationClip.length + 0.5f);
        Destroy(gameObject);
    }
}
