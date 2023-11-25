using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    //Dependencies
    private PlayerInput     _playerInput => GetComponent<PlayerInput>();
    private InputActionMap  _currentMap;

    //Public Data
    public Vector2 _moveInput  { get; private set; }
    public bool _primaryShoot {  get; private set; }
    public bool _secondaryShoot { get; private set; }

    //Inspector Hided Public Data
    [HideInInspector] public InputAction moveAction;
    [HideInInspector] public InputAction primaryShoot;
    [HideInInspector] public InputAction secondaryShoot;

    private void Awake()
    {
        _currentMap     = _playerInput.currentActionMap;

        //Get the actions references
        moveAction      = _currentMap.FindAction("Move");
        primaryShoot    = _currentMap.FindAction("PrimaryShoot");
        secondaryShoot  = _currentMap.FindAction("SecondaryShoot");

        //Subscribe all actions callbacks.
        moveAction.performed        += onMove;
        primaryShoot.performed      += onPrimaryShoot;
        secondaryShoot.performed    += onSecondaryShoot;

        moveAction.canceled         += onMove;
        primaryShoot.canceled       += onPrimaryShoot;
        secondaryShoot.canceled     += onSecondaryShoot;

    }

    private void onMove(InputAction.CallbackContext context)            => _moveInput = context.ReadValue<Vector2>();
    private void onPrimaryShoot(InputAction.CallbackContext context)    => _primaryShoot = context.ReadValueAsButton();
    private void onSecondaryShoot(InputAction.CallbackContext context)  => _secondaryShoot = context.ReadValueAsButton();


    private void OnEnable()
    {
        _currentMap.Enable();
    }
    private void OnDisable()
    {
        _currentMap.Disable();
    }
}