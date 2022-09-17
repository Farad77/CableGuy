using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;
using Quantum;

public unsafe class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator = null;
    [SerializeField] private GameObject _weapon = null;
    [SerializeField] private GameObject _shield = null;
    
    private PlayerRef _playerRef = default;
    private EntityRef _entityRef = default;
    private QuantumGame _game = null;
    
    // Animator Parameters
    private const string FLOAT_MOVEMENT_SPEED = "floatMovementSpeed";
    private const string FLOAT_MOVEMENT_HORIZONTAL = "floatHorizontalMovement";
    private const string FLOAT_MOVEMENT_VERTICAL = "floatVerticalMovement";
    private const string FLOAT_AIR_DIRECTION_Y = "floatAirDirY";

    private const string BOOL_IS_GROUNDED = "boolIsGrounded";
    private const string BOOL_IS_MOVING = "boolIsMoving";
    private const string BOOL_IS_WEAPON_EQUIPPED = "boolIsWeaponEquipped";
    private const string BOOL_IS_BLOCKING = "boolIsBlocking";

    private const string TRIGGER_JUMP = "triggerJump";
    private const string TRIGGER_ATTACK = "triggerAttack";
    
    private const string TRIGGER_EQUIP_WEAPON = "triggerEquipWeapon";
    private const string TRIGGER_UNEQUIP_WEAPON = "triggerUnequipWeapon";

    // This method is registered to the EntityView's OnEntityInstantiated event located on the parent GameObject
    public void Initialize(PlayerRef playerRef, EntityRef entityRef)
    {
        _playerRef = playerRef;
        _entityRef = entityRef;
        _game = QuantumRunner.Default.Game;

        // Set up Quantum events
        QuantumEvent.Subscribe<EventPlayerWeaponEquip>(this, WeaponEquip);
        QuantumEvent.Subscribe<EventPlayerAttack>(this, Attack);
        QuantumEvent.Subscribe<EventPlayerJump>(this, Jump);
    }

    // Update is called once per frame
    void Update()
    {
        if (_game.Frames.Verified.IsPredicted) return;
        
        MovementAnimation();
        BlockingAnimation();
    }

    private void MovementAnimation()
    {
        var kcc = _game.Frames.Verified.Unsafe.GetPointer<CharacterController3D>(_entityRef);
        bool isMoving = kcc->Velocity.Magnitude.AsFloat > 0.2f;
        
        _animator.SetBool(BOOL_IS_MOVING, isMoving);
        _animator.SetBool(BOOL_IS_GROUNDED, kcc->Grounded);
        
        if (isMoving)
        {
            _animator.SetFloat(FLOAT_MOVEMENT_SPEED, kcc->Velocity.Magnitude.AsFloat);
            _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, kcc->Velocity.Z.AsFloat);
            _animator.SetFloat(FLOAT_AIR_DIRECTION_Y, kcc->Velocity.Y.AsFloat);
        }
        else
        {
            _animator.SetFloat(FLOAT_MOVEMENT_SPEED, 0.0f);
            _animator.SetFloat(FLOAT_MOVEMENT_VERTICAL, 0.0f);
            _animator.SetFloat(FLOAT_AIR_DIRECTION_Y, 0.0f);
        }
    }

    private void BlockingAnimation()
    {
        var shield = _game.Frames.Predicted.Unsafe.GetPointer<Shield>(_entityRef);
        _animator.SetBool(BOOL_IS_BLOCKING, shield->IsBlocking);
    }

  private void Jump(EventPlayerJump e)
    {
        if (e.PlayerRef != _playerRef) return;
        _animator.SetTrigger(TRIGGER_JUMP);
    }
    
    private void WeaponEquip(EventPlayerWeaponEquip e)
    {
        if (e.PlayerRef != _playerRef) return;
        ToggleEquipment(e.Equip);
    }

    private void ToggleEquipment(bool value)
    {
      _shield.SetActive(value);
      _weapon.SetActive(value);
      _animator.SetBool(BOOL_IS_WEAPON_EQUIPPED, value);
      _animator.SetTrigger(value ? TRIGGER_EQUIP_WEAPON : TRIGGER_UNEQUIP_WEAPON);
    }

    private void Attack(EventPlayerAttack e)
    {
        if (e.PlayerRef != _playerRef) return;
        _animator.SetTrigger(TRIGGER_ATTACK);
    }
}
