using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name References")]
    [SerializeField] private string actionMapNameBasic = "Basic";
    [SerializeField] private string actionMapNameMedium = "Medium";
    [SerializeField] private string actionMapNameGun = "Gun";

    [Header("Action Name References")]
    [SerializeField] private string move = "Move";
    [SerializeField] private string look = "Look";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string crouch = "Crouch";
    [SerializeField] private string reload = "Reload";
    [SerializeField] private string ADS = "ADS";
    [SerializeField] private string fire = "Fire";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction crouchAction;
    private InputAction reloadAction;
    private InputAction ADSAction;
    private InputAction fireAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool JumpTriggered { get; private set; }
    public float SprintValue { get; private set; }
    public bool CrouchValue { get; private set; }
    public bool ReloadValue { get; private set; }
    public bool ADSValue { get; private set; }
    public bool FireValue { get; private set; }
    public static PlayerInputHandler Instance { get; private set; }


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }


        moveAction = playerControls.FindActionMap(actionMapNameBasic).FindAction(move);
        lookAction = playerControls.FindActionMap(actionMapNameBasic).FindAction(look);
        jumpAction = playerControls.FindActionMap(actionMapNameMedium).FindAction(jump);
        sprintAction = playerControls.FindActionMap(actionMapNameMedium).FindAction(sprint);
        crouchAction = playerControls.FindActionMap(actionMapNameMedium).FindAction(crouch);
        reloadAction = playerControls.FindActionMap(actionMapNameGun).FindAction(reload);
        ADSAction = playerControls.FindActionMap(actionMapNameGun).FindAction(ADS);
        fireAction = playerControls.FindActionMap(actionMapNameGun).FindAction(fire);
        

        RegisterInputActions();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        moveAction.canceled += context => MoveInput = Vector2.zero;

        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lookAction.canceled += context => LookInput = Vector2.zero;

        jumpAction.performed += context => JumpTriggered = true;
        jumpAction.canceled += context => JumpTriggered = false;

        sprintAction.performed += context => SprintValue = context.ReadValue<float>();
        sprintAction.canceled += context => SprintValue = 0f;

        crouchAction.performed += context => CrouchValue = context.ReadValue<float>() > 0.5f;
        crouchAction.canceled += context => CrouchValue = false;

        reloadAction.performed += context => ReloadValue = context.ReadValue<float>() > 0.5f;
        reloadAction.canceled += context => ReloadValue = false;

        ADSAction.performed += context => ADSValue = context.ReadValue<float>() > 0.5f;
        ADSAction.canceled += context => ADSValue = false;

        fireAction.performed += context => FireValue = context.ReadValue<float>() > 0.5f;
        fireAction.canceled += context => FireValue = false;

    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();
        jumpAction.Enable();
        sprintAction.Enable();
        crouchAction.Enable();
        ADSAction.Enable();
        reloadAction.Enable();
        fireAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable();
        lookAction.Disable();
        jumpAction.Disable();
        sprintAction.Disable();
        crouchAction.Disable();
        ADSAction.Disable();
        reloadAction.Disable();
        fireAction.Disable();
    }
}
