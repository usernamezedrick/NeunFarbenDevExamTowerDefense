using System;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using static InputSystem_Actions;

[CreateAssetMenu(fileName = "PlayerInputReader", menuName = "ScriptableObject/Player/InputReader")]
public class PlayerInputReader : SerializedScriptableObject, IPlayerActions, IUIActions
{
    public Subject<Vector2> Movement;
    public Subject<Unit> Attack;
    
    
    private InputSystem_Actions _inputActions;
    
    private void OnEnable()
    {
        if (_inputActions != null)
            return;
        
        Initialize();
    }
    
    private void Initialize()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Player.SetCallbacks(this);
        _inputActions.UI.SetCallbacks(this);

        Movement = new Subject<Vector2>();
        Attack = new Subject<Unit>();
    }
    
    public void EnablePlayerActions() 
    {
        _inputActions.Enable();
        _inputActions.Player.Enable();
        _inputActions.UI.Enable();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Movement.OnNext(context.ReadValue<Vector2>());
    }

    public void OnLook(InputAction.CallbackContext context)
    {
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) 
        {
            Attack.OnNext(UniRx.Unit.Default);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
    }

    public void OnJump(InputAction.CallbackContext context)
    {
    }

    public void OnPrevious(InputAction.CallbackContext context)
    {
    }

    public void OnNext(InputAction.CallbackContext context)
    {
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
    }

    public void OnCancel(InputAction.CallbackContext context)
    {
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
    }

    public void OnClick(InputAction.CallbackContext context)
    {
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDevicePosition(InputAction.CallbackContext context)
    {
    }

    public void OnTrackedDeviceOrientation(InputAction.CallbackContext context)
    {
    }
}
