using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class FpsController : MonoBehaviour
{
    [Header("Movement Speeds")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float sprintMultiplier = 2.0f;

    [Header("Jump Parameters")]
    [SerializeField] private float jumpForce = 5.0f;
    [SerializeField] private float gravity = 9.8f;

    [Header("Look Sensitivity")]
    [SerializeField] private float mouseSensitivity = 1.0f;
    [SerializeField] private float upDownRange = 80.0f;

    [Header("Player Controls and Objects")]
    [SerializeField] private InputActionAsset playerControls;
    [SerializeField] private GameObject gun;
    [SerializeField] private AudioSource gunFire;
    [SerializeField] private int ammo = 60;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TextMeshProUGUI shotsFiredText;
    [SerializeField] private TextMeshProUGUI targetsHitText;

    [Header("Enemies")]
    [SerializeField] private GameObject BananaMan;
    [SerializeField] private GameObject BananaMan1;
    [SerializeField] private GameObject BananaMan2;

    private CharacterController characterController;
    private Camera mainCamera;
    private PlayerInputHandler inputHandler;
    private Vector3 currentMovement;
    private float verticalRotation;
    private Animation ADS_M4;
    private bool isReloading;
    private EnemyMovement enemyMovement;
    private Vector3 randomSpawnPos;
    private int shotsFired = 0;
    private int targetsHit = 0;

    // Awake is called when the script instance is being loaded
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        mainCamera = Camera.main;
        inputHandler = PlayerInputHandler.Instance;
        ADS_M4 = GetComponent<Animation>();
        isReloading = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        enemyMovement = GameObject.Find("BananaMan").GetComponent<EnemyMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleCrouching();
        HandleAds();
        HandleFire();
        HandleReload();

        randomSpawnPos = new Vector3(Random.Range(-16, 10), 0, Random.Range(4, 26));

        // Update UI
        targetsHitText.text = $"Targets Hit: {targetsHit}";
        shotsFiredText.text = $"Shots Fired: {shotsFired}";
    }

    // Handles player movement
    void HandleMovement()
    {
        float speed = walkSpeed * (inputHandler.SprintValue > 0 ? sprintMultiplier : 1f);

        Vector3 inputDirection = new Vector3(inputHandler.MoveInput.x, 0f, inputHandler.MoveInput.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection).normalized;

        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        HandleJumping();
        characterController.Move(currentMovement * Time.deltaTime);
    }

    // Handles player rotation (looking around)
    void HandleRotation()
    {
        float mouseXRotation = inputHandler.LookInput.x * mouseSensitivity;
        transform.Rotate(0, mouseXRotation, 0);

        verticalRotation -= inputHandler.LookInput.y * mouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -upDownRange, upDownRange);
        mainCamera.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
    }

    // Handles jumping and falling
    void HandleJumping()
    {
        if (characterController.isGrounded)
        {
            currentMovement.y = -0.5f;

            if (inputHandler.JumpTriggered)
            {
                currentMovement.y = jumpForce;
            }
        }
        else
        {
            currentMovement.y -= gravity * Time.deltaTime;
        }
    }

    // Handles crouching
    void HandleCrouching()
    {
        transform.localScale = inputHandler.CrouchValue ? new Vector3(1, 0.5f, 1) : new Vector3(1, 1, 1);
    }

    // Handles aiming down sights (ADS)
    void HandleAds()
    {
        if (inputHandler.ADSValue)
        {
            gun.transform.localPosition = new Vector3(0.303f, 1.494f, -0.004f);
        }
        else
        {
            gun.transform.localPosition = new Vector3(-0.21f, 1.73f, 0.67f);
        }
    }

    // Handles reloading the gun
    void HandleReload()
    {
        if (inputHandler.ReloadValue && !isReloading)
        {
            isReloading = true;
            Invoke(nameof(ReloadFunction), 1);
        }
    }

    // Handles firing the gun
    void HandleFire()
    {
        if (inputHandler.FireValue && !isReloading)
        {
            if (ammo <= 0)
            {
                ammoText.text = "0/∞";
            }
            else
            {
                gunFire.Play();
                ammo--;
                shotsFired++;
                ammoText.text = $"{ammo}/∞";

                Ray bullet = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                Debug.DrawRay(bullet.origin, bullet.direction * 9999);
                if (Physics.Raycast(bullet, out RaycastHit hitInfo, 9999, layerMask))
                {
                    GameObject parent = hitInfo.collider.gameObject.transform.parent.gameObject;
                    string name = parent.name;
                    targetsHit++;

                    HandleEnemyHit(name);

                    Destroy(parent);
                }
            }
        }
    }

    // Handle enemy being hit
    void HandleEnemyHit(string name)
    {
        GameObject newEnemy;
        switch (name)
        {
            case "BananaMan":
                newEnemy = Instantiate(BananaMan, randomSpawnPos, Quaternion.Euler(0, 90, 0));
                break;
            case "BananaMan1":
                newEnemy = Instantiate(BananaMan1, randomSpawnPos, Quaternion.Euler(0, 90, 0));
                break;
            case "BananaMan2":
                newEnemy = Instantiate(BananaMan2, randomSpawnPos, Quaternion.Euler(0, 90, 0));
                break;
            default:
                return;
        }

        if (newEnemy.GetComponent<EnemyMovement>() == null)
        {
            newEnemy.AddComponent<EnemyMovement>();
        }
        newEnemy.name = name;
    }

    // Reload the gun and reset ammo
    void ReloadFunction()
    {
        ammo = 60;
        ammoText.text = $"{ammo}/∞";
        isReloading = false;
    }
}
