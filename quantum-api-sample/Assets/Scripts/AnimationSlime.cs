using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

public unsafe class AnimationSlime : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private AnimationClip _deathAnimationClip = null;
    
    private EntityRef _entityRef = default;
    private NavMeshSteeringAgent* _navMeshSteeringAgent = default;

    private const string FLOAT_MOVEMENT_SPEED = "floatMovementSpeed";
    private const string FLOAT_MOVEMENT_HORIZONTAL = "floatMovementHorizontal";
    private const string FLOAT_MOVEMENT_VERTICAL = "floatMovementVertical";

    private const string BOOL_IS_MOVING = "boolIsMoving";

    private const string TRIGGER_DEATH = "triggerDeath";
    private const string TRIGGER_DAMAGE_HIT = "triggerDamageHit";
    
    public void Initialize()
    {
        _entityRef = GetComponentInParent<EntityView>().EntityRef;
        _navMeshSteeringAgent = QuantumRunner.Default.Game.Frames.Verified.Unsafe.GetPointer<NavMeshSteeringAgent>(_entityRef);

        QuantumEvent.Subscribe<EventOnDamageDealt>(this, TakeDamage);
    }

    private void TakeDamage(EventOnDamageDealt obj)
    {
        if(obj.Target != _entityRef) return;
        _animator.SetTrigger(TRIGGER_DAMAGE_HIT);
    }


    // Update is called once per frame
    void Update()
    {
        MovementAnimation();
    }

    private void MovementAnimation()
    {
        if (_navMeshSteeringAgent == null) return;
        bool isMoving = _navMeshSteeringAgent->Velocity.Magnitude.AsFloat > 0.2f;
        _animator.SetBool(BOOL_IS_MOVING, isMoving);

        if (isMoving)
        {
            _animator.SetFloat(FLOAT_MOVEMENT_SPEED, _navMeshSteeringAgent->Velocity.Magnitude.AsFloat / _navMeshSteeringAgent->MaxSpeed.AsFloat);
            _animator.SetFloat(FLOAT_MOVEMENT_HORIZONTAL, _navMeshSteeringAgent->Velocity.X.AsFloat);
            _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, _navMeshSteeringAgent->Velocity.Y.AsFloat);
        }
        else
        {
            _animator.SetFloat(FLOAT_MOVEMENT_SPEED, 0.0f);
            _animator.SetFloat(FLOAT_MOVEMENT_HORIZONTAL, 0.0f);
            _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, 0.0f);
        }
    }

    public void Death()
    {
        _animator.SetFloat(FLOAT_MOVEMENT_SPEED, 0.0f);
        _animator.SetFloat(FLOAT_MOVEMENT_HORIZONTAL, 0.0f);
        _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, 0.0f);
        
        _animator.SetTrigger(TRIGGER_DEATH);
        
        Destroy(gameObject, _deathAnimationClip.length + 0.5f);
    }
}
