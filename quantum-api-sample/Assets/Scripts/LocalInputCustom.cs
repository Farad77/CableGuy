﻿using Photon.Deterministic;
using Quantum;
using UnityEngine;
using UInput = UnityEngine.Input;
using QInput = Quantum.Input;

public class LocalInputCustom : MonoBehaviour
{
    private const string AXIS_MOVEMENT_HORIZONTAL = "Horizontal";
    private const string AXIS_MOVEMENT_VERTICAL = "Vertical";

    private const string BUTTON_TURN = "Submit";
    private const string BUTTON_ATTACK = "Fire1";
    private const string BUTTON_DEFEND = "Fire2";
    private const string BUTTON_ACTION = "Fire3";
    private const string BUTTON_JUMP = "Jump";
 
    #region New Unity Input System Variables

    // [SerializeField] private UInput.InputAction movementAxes = null;
    //
    // [SerializeField] private UInput.InputAction buttonAttack = null; 
    // [SerializeField] private UInput.InputAction buttonDefend = null; 
    // [SerializeField] private UInput.InputAction buttonJump = null; 
    // [SerializeField] private UInput.InputAction buttonAction = null;

    #endregion
    
    private void OnEnable()
    {
        QuantumCallback.Subscribe<CallbackPollInput>(this, PollInput);
       
        // EnableNewUnityInput();
    }

    private void OnDisable()
    {
        QuantumCallback.UnsubscribeListener<CallbackPollInput>(this);
        
        // DisableNewUnityInput();
    }
    
    private void PollInput(CallbackPollInput pollInput)
    {
        PollUnityInput(pollInput);
    }
    public GameObject cube;
    private void PollUnityInput(CallbackPollInput pollInput)
    {
        // QuantumRunner.ShutdownAll();
       
        var i = new QInput();
        
        i.MovementHorizontal = FP.FromFloat_UNSAFE(UInput.GetAxis(AXIS_MOVEMENT_HORIZONTAL));
        i.MovementVertical = FP.FromFloat_UNSAFE(UInput.GetAxis(AXIS_MOVEMENT_VERTICAL));
        i.MoveBack = UInput.GetAxis(AXIS_MOVEMENT_VERTICAL) < 0;
        
        i.Attack = UInput.GetButton(BUTTON_ATTACK);
        i.Defend = UInput.GetButton(BUTTON_DEFEND);
        i.Action = UInput.GetButton(BUTTON_ACTION);
        i.Jump = UInput.GetButton(BUTTON_JUMP);
       
            // Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            /*  i.AimDirection = AimDirection.gameObject.GetComponentInChildren<Transform>().transform.position.ToFPVector3();
              i.AimForward = AimDirection.gameObject.GetComponentInChildren<Transform>().transform.forward.ToFPVector3();*/
            /* i.AimDirection = AimDirection.transform.position.ToFPVector3();
             i.AimForward = AimDirection.transform.forward.ToFPVector3();*/
             Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

            //Get the Screen position of the mouse
             Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(UnityEngine.Input.mousePosition);
            // i.AimDirection = mouseOnScreen.ToFPVector2();
            //Get the angle between the points
             i.Angle = Mathf.RoundToInt(AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen))+90;
        //  i.Angle = (AimDirection.angle +90).ToFP();
        //  i.AimDirection = new FPVector3();
        if (UnityEngine.Input.GetMouseButtonDown(0))
        {
            Ray ray;
            RaycastHit hit;
            ray = Camera.main.ScreenPointToRay(UnityEngine.Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {

                i.AimDirection = hit.point.ToFPVector3();
                Debug.Log(" " + hit.point);
                //cube.transform.position = hit.point;

            }
            else
            {

            }
        }


        pollInput.SetInput(i, DeterministicInputFlags.Repeatable);
    }
    float AngleBetweenTwoPoints(Vector2 a, Vector2 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    public void LateUpdate()
    {
       
    }

    #region New Unity Input System Functions

    // private void PollNewUnityInput(CallbackPollInput pollInput)
    // {
    //     var i = new QInput();
    //
    //     var gamepad = UInput.Gamepad.current;
    //     var keyboard = UInput.Keyboard.current;
    //
    //     if (gamepad == null)
    //     {
    //         Log.Warn("This game is meant to be played with a Gamepad");
    //     }
    //     
    //     if (keyboard == null && gamepad == null)
    //     {
    //         Log.Error("No suitable input method found. Please connect a Gamepad or Keyboard to your device.");
    //         return;
    //     }
    //
    //     i.MovementHorizontal = FP.FromFloat_UNSAFE(movementAxes.ReadValue<Vector2>().x);
    //     i.MovementVertical = FP.FromFloat_UNSAFE(movementAxes.ReadValue<Vector2>().y);
    //     i.MoveBack = movementAxes.ReadValue<Vector2>().y < 0;
    //
    //     i.Attack = buttonAttack.ReadValue<float>() > 0;
    //     i.Defend = buttonDefend.ReadValue<float>() > 0;
    //     i.Action = buttonAction.ReadValue<float>() > 0;
    //     i.Jump = buttonJump.ReadValue<float>() > 0;
    //     
    //     pollInput.SetInput(i, DeterministicInputFlags.Repeatable);
    // }
    //
    // private void EnableNewUnityInput()
    // {
    //     movementAxes.Enable();
    //     
    //     buttonAttack.Enable();
    //     buttonDefend.Enable();
    //     buttonJump.Enable();
    //     buttonAction.Enable();
    // }
    //
    // private void DisableNewUnityInput()
    // {
    //     movementAxes.Disable();
    //     
    //     buttonAttack.Disable();
    //     buttonDefend.Disable();
    //     buttonJump.Disable();
    //     buttonAction.Disable();
    // }

    #endregion
}