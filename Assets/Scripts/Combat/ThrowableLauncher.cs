using UnityEngine;
using UnityEngine.InputSystem;

public class ThrowableLauncher : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;
    public Transform releasePoint;
    public GameObject throwablePrefab;
    [SerializeField] private Sprite reticle;
    [SerializeField] private string animationState;
    [SerializeField] private string[] cueNames;

    [Header("SessionConfig")]
    public bool unlimitedThrows;
    public int remainingThrows;
    public int capacity = 100;
    public float cooldown;

    [Header("Throwing")]
    public float launchForce;
    public float upwardForce;
    private bool readyToLaunch;
    private InputAction attackAction;
    private Animator animator;

    private void Start()
    {
        attackAction = InputSystem.actions.FindAction("Attack");
        animator = Camera.main.transform.GetComponent<Animator>();
        readyToLaunch = true;
        RefillThrows();
    }

    private void Update()
    {
        if (attackAction.WasPressedThisFrame() && readyToLaunch) Launch();
    }

    private void Launch()
    {
        if (remainingThrows <= 0 && !unlimitedThrows) return;
        if (animationState != string.Empty) animator.Play(animationState, 1);
        readyToLaunch = false;
        SessionDirector.Instance.PlayCue(cueNames[Random.Range(0, cueNames.Length)]);
        GameObject projectile = Instantiate(throwablePrefab, releasePoint.position, cameraTransform.rotation);
        Rigidbody projectileBody = projectile.GetComponent<Rigidbody>();
        Vector3 forceDirection = cameraTransform.forward;

        if (Physics.Raycast(releasePoint.position, cameraTransform.forward, out RaycastHit hit, 500f))
        {
            forceDirection = (hit.point - releasePoint.position).normalized;
        }

        Vector3 force = forceDirection * launchForce + transform.up * upwardForce;
        projectileBody.AddForce(force, ForceMode.Impulse);
        if (!unlimitedThrows) remainingThrows--;
        Invoke(nameof(ResetLaunch), cooldown);
    }

    private void ResetLaunch()
    {
        readyToLaunch = true;
    }

    public void RefillThrows()
    {
        remainingThrows = capacity;
    }

    private void OnEnable()
    {
        if (reticle != null) HudRoot.Instance.SetReticle(reticle.name);
        HudRoot.Instance.activeLauncher = this;
    }
}
