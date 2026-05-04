using UnityEngine;
using UnityEngine.InputSystem;

public class AmmoStation : MonoBehaviour
{
    [SerializeField] private int slotIndex;
    [SerializeField] private GameObject stockedVisual;
    [SerializeField] private ParticleSystem refillEffect;
    [SerializeField] private float restockDelay = 60;
    private bool playerNearby;
    private bool stocked = true;
    private LoadoutSwitcher loadout;
    private InputAction interactAction;
    private GameObject hint;

    private void Start()
    {
        stocked = true;
        interactAction = InputSystem.actions.FindAction("Interact");
        loadout = GameObject.FindGameObjectWithTag("Player").GetComponent<LoadoutSwitcher>();
    }

    private void Update()
    {
        if (playerNearby && interactAction.WasPressedThisFrame()) RefillStation();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (stocked && loadout.slotUnlocked[slotIndex])
        {
            playerNearby = true;
            if (hint == null) hint = HudRoot.Instance.SpawnHint("Refill", false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerNearby = false;
        if (hint != null) Destroy(hint);
    }

    public void RefillStation()
    {
        if (refillEffect != null) refillEffect.Play();
        stockedVisual.SetActive(false);
        stocked = false;
        playerNearby = false;
        loadout.launchers[slotIndex].RefillThrows();
        switch (slotIndex)
        {
            case 0:
                SessionDirector.Instance.PlayCue("refill-popcorn");
                break;
            case 1:
                SessionDirector.Instance.PlayCue("refill-soda");
                break;
            case 2:
                SessionDirector.Instance.PlayCue("refill-tickets");
                break;
            case 3:
                SessionDirector.Instance.PlayCue("refill-glasses");
                break;
        }
        Invoke(nameof(RestockStation), restockDelay);
    }

    public void RestockStation()
    {
        stocked = true;
        stockedVisual.SetActive(true);
    }
}
