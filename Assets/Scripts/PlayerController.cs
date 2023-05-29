using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool canMove = true;
    private bool shouldJump => Input.GetKeyDown(jumpKey) && characterController.isGrounded;
    private bool shouldCrouch => Input.GetKey(crouchKey);
    private bool shouldSptint => Input.GetKey(sprintKey) && characterController.isGrounded && !isCrouching;
    private bool shouldReload => Input.GetKeyDown(reloadKey) && !duringReloadAnimation;
    private bool shouldFire => Input.GetKey(fireKey) && !duringReloadAnimation;
    private bool shouldADS => Input.GetKeyDown(adsKey);
    private bool shouldSwitch => Input.GetKeyDown(weaponOneKey) || Input.GetKeyDown(weaponTwoKey) || Input.GetKeyDown(weaponThreeKey);

    private WeaponManager weaponManager;
    public Armature armature;

    public static bool noAmmo;
    public static bool fullAmmo;
    public static bool fireFinihed;
    public static bool duringSwitchAnimation;
    public bool onLadder;

    [SerializeField] private Animator animator;

    [Header("Function")]
    [SerializeField] private bool canJump = true;
    [SerializeField] private bool canCrouch = true;
    [SerializeField] private bool canFire = true;
    [SerializeField] private bool canSprint = true;

    [Header("Control")]
    [SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [SerializeField] private KeyCode reloadKey = KeyCode.R;
    [SerializeField] private KeyCode fireKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode adsKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;

    [SerializeField] private KeyCode weaponOneKey = KeyCode.Alpha1;
    [SerializeField] private KeyCode weaponTwoKey = KeyCode.Alpha2;
    [SerializeField] private KeyCode weaponThreeKey = KeyCode.Alpha3;

    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3.0f;
    [SerializeField] private float crouchSpeed = 1.5f;
    [SerializeField] private float gravity = 10.0f;
    [SerializeField] private float sprintSpeed = 5.0f;
    private bool isSprinting;

    [Header("Look")]
    [SerializeField, Range(1, 10)] private float lookSpeedX = 2.0f;
    [SerializeField, Range(1, 10)] private float lookSpeedY = 2.0f;
    [SerializeField, Range(1, 180)] private float lookUpLimit = 90.0f;
    [SerializeField, Range(1, 180)] private float lookDownLimit = 90.0f;

    [Header("Jump")]
    [SerializeField] private float jumpForce = 5.0f;


    [Header("Crouch")]
    [SerializeField] private float crouchHeight = 1.2f;
    [SerializeField] private float standHeight = 1.8f;
    [SerializeField] private float timeToCrouch = 0.1f;
    [SerializeField] private Vector3 crouchCenter = new Vector3(0f, 0.3f, 0f);
    [SerializeField] private Vector3 standCenter = new Vector3(0f, 0f, 0f);
    private bool isCrouching = false;
    private bool duringCrouchAnimation = false;
    private float targetHeight;
    private Vector3 targetCenter;

    [Header("Reload")]
    public static Action reloadInput;
    public bool duringReloadAnimation;

    [Header("Fire")]
    public static Action shootInput;
    [SerializeField] private Camera playerCamera;
    private CharacterController characterController;

    [Header("ADS")]
    public static Action adsInput;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip walkClip;
    public AudioClip waterClip;
    public AudioClip jumpClip;
    private float stepTimer = 0f;
    public float walkStep = 0.8f;
    public float sprintStep = 0.4f;


    private Vector3 moveDirection;
    private Vector2 currentInput;
    private float rotationX = 0f;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        weaponManager = GetComponent<WeaponManager>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        animator.SetBool("usingPistol", true);
    }


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayerController.noAmmo = false;
    }


    void Update()
    {
        if (canMove)
        {
            HandleLadder();
            HandleMovementInput();
            HandleMouseLook();
            HandleReload();
            HandleFire();
            HandleADS();
            HandleSwitch();
            HandleStep();
            if (canJump)
            {
                HandleJump();
            }
            if (canCrouch)
            {
                HandleCrouch();
            }

            ApplyFinalMovements();
        }
    }

    private void HandleLadder()
    {
        if (!onLadder)
        {
            return;
        }
        else
        {
            
            if (Input.GetKey(KeyCode.W))
            {
                characterController.transform.position += Vector3.up / 3.2f;
            }
            if (Input.GetKey(KeyCode.S))
            {
                characterController.transform.position += Vector3.down / 3.2f;
            }
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other!=null && other.gameObject.CompareTag("Ladder"))
        {
            onLadder = true;
            gravity = 0f;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other != null && other.gameObject.CompareTag("Ladder"))
        {
            onLadder = false;
            gravity = 10f;
        }
    }

    private void HandleStep()
    {
        if (!characterController.isGrounded)
        {
            return;
        }
        if (currentInput == Vector2.zero)
        {
            return;
        }
        if (isCrouching)
        {
            return;
        }

        stepTimer += Time.deltaTime;
        if (!isSprinting && !isCrouching && characterController.isGrounded)
        {
            if (stepTimer >= walkStep)
            {
                Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3);
                if (hit.collider.tag == "Water")
                {
                    audioSource.PlayOneShot(waterClip);
                }
                else
                {
                    audioSource.PlayOneShot(walkClip);
                }
                stepTimer = 0f;
            }
        }
        else
        {
            if (stepTimer >= sprintStep)
            {
                Physics.Raycast(playerCamera.transform.position, Vector3.down, out RaycastHit hit, 3);
                if (hit.collider.tag == "Water")
                {
                    audioSource.PlayOneShot(waterClip);
                }
                else
                {
                    audioSource.PlayOneShot(walkClip);
                }
                stepTimer = 0f;
            }
        }


    }

    private void HandleMovementInput()
    {
        if (onLadder)
        {
            moveDirection = Vector3.zero;
            return;
        }
        if (!isCrouching)
        {
            if (shouldSptint)
            {
                isSprinting = true;
                currentInput = new Vector2(sprintSpeed * Input.GetAxis("Vertical"), sprintSpeed * Input.GetAxis("Horizontal"));
            }
            else
            {
                isSprinting = false;
                currentInput = new Vector2(walkSpeed * Input.GetAxis("Vertical"), walkSpeed * Input.GetAxis("Horizontal"));
            }
        }
        else
        {
            currentInput = new Vector2(crouchSpeed * Input.GetAxis("Vertical"), crouchSpeed * Input.GetAxis("Horizontal"));
        }
        

        float moveDirectionY = moveDirection.y;
        moveDirection = (transform.TransformDirection(Vector3.forward) * currentInput.x) + (transform.TransformDirection(Vector3.right) * currentInput.y);
        moveDirection.y = moveDirectionY;

    }

    private void HandleMouseLook()
    {
        rotationX -= Input.GetAxis("Mouse Y") * lookSpeedY;
        rotationX = Mathf.Clamp(rotationX, -lookUpLimit, lookDownLimit);
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeedX, 0);
    }

    private void HandleJump()
    {
        if (shouldJump)
        {
            moveDirection.y = jumpForce;
            audioSource.PlayOneShot(jumpClip);
        }
    }
    private void HandleCrouch()
    {
        if (shouldCrouch)
        {
            targetHeight = crouchHeight;
            targetCenter = crouchCenter;
            if (duringCrouchAnimation)
            {
                return;
            }
            if (!isCrouching)
            {
                isCrouching = true;
                StartCoroutine(StandCrouch());
            }
        }
        
        if (isCrouching)
        {
            if (!shouldCrouch)
            {
                if (isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1f))
                {
                    return;
                }
                targetHeight = standHeight;
                targetCenter = standCenter;
                if (duringCrouchAnimation)
                {
                    return;
                }
                isCrouching = false;
                StartCoroutine(StandCrouch());
                return;
            }
        }
    }

    private void HandleReload()
    {
        if (duringSwitchAnimation)
        {
            return;
        }
        if (fullAmmo)
        {
            return;
        }
        if (shouldReload)
        {
            Reload();
        }
    }

    private void Reload()
    {
        if (!duringReloadAnimation)
        {
            animator.SetTrigger("reloadGun");
        }
        reloadInput?.Invoke();
        armature.SwitchToHand();
        duringReloadAnimation = true;
        animator.SetBool("isReloading", true);
    }
    private void HandleFire()
    {
        if (duringSwitchAnimation)
        {
            return;
        }
        if (noAmmo)
        {
            animator.SetBool("isFiring", false);
            Reload();
            return;
        }
        if (shouldFire)
        {
            fireFinihed = false;
            shootInput?.Invoke();
            if (WeaponManager.primaryInt != 3)
            {
                animator.SetBool("isFiring", true);
            }

        }
        if (!shouldFire)
        {
            if (fireFinihed)
            {
                animator.SetBool("isFiring", false);
            }
        }
    }

    private void HandleADS()
    {
        if (shouldADS)
        {
            adsInput?.Invoke();
        }
    }

    private void HandleSwitch()
    {
        if (shouldSwitch)
        {
            if (Input.GetKeyDown(weaponOneKey))
            {
                if (WeaponManager.primaryInt == 1)
                {
                    return;
                }
                duringSwitchAnimation = true;
                WeaponManager.primaryInt = 1;
                animator.SetBool("usingPistol", false);
                animator.SetBool("isSwitching", true);
                animator.SetTrigger("switchGun");
                weaponManager.SwitchGun();
                if (duringReloadAnimation)
                {
                    armature.FinishReload();
                    animator.SetBool("isFiring", false);
                }
            }
            if (Input.GetKeyDown(weaponTwoKey))
            {
                if (WeaponManager.primaryInt == 2)
                {
                    return;
                }
                duringSwitchAnimation = true;
                WeaponManager.primaryInt = 2;
                animator.SetBool("usingPistol", false);
                animator.SetBool("isSwitching", true);
                animator.SetTrigger("switchGun");
                weaponManager.SwitchGun();
                if (duringReloadAnimation)
                {
                    armature.FinishReload();
                    animator.SetBool("isFiring", false);
                }
            }
            if (Input.GetKeyDown(weaponThreeKey))
            {
                if (WeaponManager.primaryInt == 3)
                {
                    return;
                }
                duringSwitchAnimation = true;
                WeaponManager.primaryInt = 3;
                animator.SetBool("usingPistol", true);
                animator.SetBool("isSwitching", true);
                weaponManager.SwitchGun();
                if (duringReloadAnimation)
                {
                    armature.FinishReload();
                    animator.SetBool("isFiring", false);
                }
            }
        }
    }
    private void ApplyFinalMovements()
    {
        if (onLadder)
        {
            return;
        }
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }
        characterController.Move(moveDirection * Time.deltaTime);
    }

    private IEnumerator StandCrouch()
    {
        duringCrouchAnimation = true;
        float timeElapsed = 0f;
        float currentHeight = characterController.height;
        Vector3 currentCenter = characterController.center;
        while (timeElapsed < timeToCrouch)
        {
            characterController.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            characterController.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        characterController.height = targetHeight;
        characterController.center = targetCenter;
        duringCrouchAnimation = false;
    }
}
