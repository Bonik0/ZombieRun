using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class DebugOverlay : MonoBehaviour
{
    public bool developerMode;
    private InputAction debugAction;

    private void Start()
    {
        debugAction = InputSystem.actions.FindAction("Debug");
    }

    private void Update()
    {
        if (debugAction.WasPressedThisFrame()) developerMode = !developerMode;
    }

    private void OnGUI()
    {
        if (!developerMode) return;

        GUI.Label(new Rect(10, 10, 200, 20), $"ms per frame: {System.Decimal.Round((decimal)(Time.deltaTime * 1000), 2)}");
        GUI.Label(new Rect(10, 40, 200, 20), $"frame per second: {1f / Time.deltaTime}");
        GUI.Label(new Rect(10, 70, 100, 20), "FOV: " + (int)Camera.main.fieldOfView);
        Camera.main.fieldOfView = GUI.HorizontalSlider(new Rect(10, 100, 100, 20), Camera.main.fieldOfView, 10, 170);

        if (GUI.Button(new Rect(10, 130, 150, 40), "Toggle Mortality"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<Protagonist>().ToggleInvulnerability();
        if (GUI.Button(new Rect(10, 180, 150, 40), "Unlock All Weapons"))
            GameObject.FindGameObjectWithTag("Player").GetComponent<LoadoutSwitcher>().slotUnlocked = new bool[] { true, true, true, true };
        if (GUI.Button(new Rect(10, 230, 150, 40), "Open All Gateways"))
            foreach (Gateway gateway in FindObjectsByType<Gateway>(FindObjectsInactive.Include, FindObjectsSortMode.None)) gateway.OpenGate();
        if (GUI.Button(new Rect(10, 280, 150, 40), "Reload"))
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        if (GUI.Button(new Rect(10, 330, 150, 40), "Exit"))
            Application.Quit();
    }
}
