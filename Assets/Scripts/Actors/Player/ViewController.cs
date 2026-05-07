using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.InputSystem;

public class ViewController : MonoBehaviour
{
    public float mouseScale = 1;
    [SerializeField] private Transform player;
    private float pitch;
    private InputAction lookAction;

    private void Start()
    {
        lookAction = InputSystem.actions.FindAction("Look");
        mouseScale = PlayerPrefs.GetFloat("sensitivity", 1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ApplyLookInput();
    }

    private void ApplyLookInput()
    {
        Vector2 input = lookAction.ReadValue<Vector2>() * mouseScale / 10;
        pitch -= input.y;
        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        player.Rotate(Vector3.up * input.x);
    }

    private void OnDestroy()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}

public class LookOscillator
{
    private float horizontalInput;
    private float smoothedInput;
    private readonly float sensitivity;
    private readonly float smoothing;
    private float yaw;

    public LookOscillator(float sensitivity, float smoothing)
    {
        this.sensitivity = sensitivity;
        this.smoothing = smoothing;
    }

    public Quaternion Evaluate(float input, Vector3 axis)
    {
        horizontalInput = input * sensitivity * smoothing;
        smoothedInput = Mathf.Lerp(smoothedInput, horizontalInput, 1f / smoothing);
        yaw += smoothedInput;
        return Quaternion.AngleAxis(yaw, axis);
    }
}
