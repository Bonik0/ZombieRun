using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HudRoot : MonoBehaviour
{
    public static HudRoot Instance { get; private set; }
    [SerializeField] private bool fadeInOverlay = true;
    [SerializeField] private TMP_Text hudReadout;
    [SerializeField] private TMP_Text waveReadout;
    [SerializeField] private Image reticle;
    [SerializeField] internal Slider enduranceGauge;

    [Header("References")]
    public GameObject hitVignette;
    public GameObject pickupBanner;

    internal ThrowableLauncher activeLauncher;
    private InputAction pauseAction;
    private GameObject optionsPanel;
    private GameObject hintPanel;
    private Protagonist protagonist;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (GetComponent<CanvasScaler>().uiScaleMode == CanvasScaler.ScaleMode.ConstantPixelSize)
            Debug.LogWarning($"{gameObject.name} is currently set to 'Constant Pixel Size', this is usually undesired!");
        if (FindAnyObjectByType<EventSystem>() == null)
            Debug.LogWarning("No Event System in Scene!");

        protagonist = GameObject.FindGameObjectWithTag("Player").GetComponent<Protagonist>();
        pauseAction = InputSystem.actions.FindAction("Pause");

        if (fadeInOverlay) SpawnMenu("FadeOutEffect");
    }

    private void Update()
    {
        hudReadout.text = Mathf.Ceil(protagonist.Mendth) + " HP\n";
        if (activeLauncher != null)
        {
            hudReadout.text += $"{activeLauncher.remainingThrows} ROUNDS";
            if (activeLauncher.remainingThrows >= activeLauncher.capacity) hudReadout.text += " (FULL)";
        }

        if (pauseAction.WasPressedThisFrame() && optionsPanel == null)
            optionsPanel = SpawnMenu("Options Menu");
    }

    public void SetWaveLabel(int waveIndex)
    {
        waveReadout.gameObject.GetComponent<Animator>().Play("Highlight");
        waveReadout.text = $"WAVE {waveIndex + 1}";
    }

    public void SetEscapeLabel()
    {
        waveReadout.gameObject.GetComponent<Animator>().Play("Highlight");
        waveReadout.text = "ESCAPE";
    }

    public void SetReticle(string reticleName)
    {
        reticle.sprite = Resources.Load<Sprite>("UI/Crosshairs/" + reticleName);
    }

    public GameObject SpawnMenu(string resourceName)
    {
        return Instantiate((GameObject)Resources.Load($"UI/{resourceName}"), transform);
    }

    public void OpenOptions()
    {
        optionsPanel = SpawnMenu("Options Menu");
    }

    public GameObject SpawnHint(string resourceName)
    {
        if (hintPanel != null) Destroy(hintPanel);
        hintPanel = Instantiate((GameObject)Resources.Load($"UI/Tutorials/{resourceName}"), transform);

        return hintPanel;
    }

    public GameObject SpawnHint(string resourceName, bool canOverride)
    {
        if (hintPanel != null && !canOverride) return null;

        return SpawnHint(resourceName);
    }

    public void SpawnMenuFromEvent(string resourceName)
    {
        Instantiate((GameObject)Resources.Load($"UI/{resourceName}"), transform);
    }

    public void SpawnHintFromEvent(string resourceName)
    {
        if (hintPanel != null) Destroy(hintPanel);
        hintPanel = Instantiate((GameObject)Resources.Load($"UI/Tutorials/{resourceName}"), transform);
    }

    private void Reset()
    {
        gameObject.name = "--- Canvas ---";
    }
}
