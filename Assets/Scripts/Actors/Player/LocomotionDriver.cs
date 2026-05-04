using UnityEngine;
using UnityEngine.InputSystem;

public class LocomotionDriver : MonoBehaviour
{
    private const string walkingFlag = "isWalking";
    private const string runningFlag = "isRunning";
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float strollSpeed;
    [SerializeField] private float moveSpeed;
    private StaminaMeter staminaMeter;
    private CharacterController controller;
    private Vector3 inputVector;
    private Vector3 movementVector;
    private float gravity = -10f;
    public float momentumDampening = 5f;
    [SerializeField] private Animator cameraAnimator;
    private bool walking;
    public bool sprinting;
    [SerializeField] private float staminaCost = 0.1f;
    private InputAction moveAction;
    private InputAction sprintAction;

    private void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        sprintAction = InputSystem.actions.FindAction("Sprint");
        moveSpeed = strollSpeed;
        controller = GetComponent<CharacterController>();
        staminaMeter = GetComponent<StaminaMeter>();
    }

    private void Update()
    {
        CheckEndurance();
        ReadInput();
        MoveActor();
        CheckSprintRelease();
        cameraAnimator.SetBool(walkingFlag, walking);
        cameraAnimator.SetBool(runningFlag, sprinting);
    }

    private void ReadInput()
    {
        if (moveAction.IsPressed())
        {
            if (sprintAction.IsPressed() && staminaMeter.canSprint)
            {
                moveSpeed = sprintSpeed;
                sprinting = true;
                staminaMeter.Endurance -= staminaCost * Time.deltaTime;
            }
            else if (sprintAction.WasPressedThisFrame() && !staminaMeter.canSprint)
            {
                HudRoot.Instance.SpawnHint("Stamina", false);
            }

            Vector2 rawInput = moveAction.ReadValue<Vector2>();
            inputVector = new Vector3(rawInput.x, 0f, rawInput.y);
            inputVector.Normalize();
            inputVector = transform.TransformDirection(inputVector);
            walking = true;
        }
        else
        {
            inputVector = Vector3.Lerp(inputVector, Vector3.zero, momentumDampening * Time.deltaTime);
            walking = false;
        }

        movementVector = (inputVector * moveSpeed) + (Vector3.up * gravity);
    }

    private void MoveActor()
    {
        controller.Move(movementVector * Time.deltaTime);
    }

    private void CheckSprintRelease()
    {
        if (sprintAction.WasReleasedThisFrame())
        {
            moveSpeed = strollSpeed;
            sprinting = false;
        }
    }

    private void CheckEndurance()
    {
        if (!staminaMeter.canSprint) sprinting = false;
        if (!sprinting) moveSpeed = strollSpeed;
    }
}
