using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsPanel : MonoBehaviour
{
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private TMP_Text sensitivityReadout;
    private ViewController viewController;

    private void Start()
    {
        try { SessionDirector.Instance.PauseClock(); } catch { }
        Cursor.lockState = CursorLockMode.Confined;
        try { viewController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ViewController>(); } catch { }
        sensitivitySlider.value = PlayerPrefs.GetFloat("sensitivity", 1);
        sensitivityReadout.text = $"SENSITIVITY: {Mathf.Round(sensitivitySlider.value * 100) / 100}";
    }

    public void SyncSensitivity()
    {
        float newSensitivity = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivity", newSensitivity);
        try
        {
            if (viewController == null)
                viewController = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<ViewController>();

            viewController.mouseScale = newSensitivity;
        }
        catch { }

        sensitivityReadout.text = $"SENSITIVITY: {Mathf.Round(newSensitivity * 100) / 100}";
    }

    private void OnDestroy()
    {
        try { SessionDirector.Instance.ResumeClock(); } catch { }
        if (SceneManager.GetActiveScene().buildIndex != 0) Cursor.lockState = CursorLockMode.Locked;
    }
}
