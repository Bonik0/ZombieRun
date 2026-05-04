using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuPanel : MonoBehaviour
{
    [SerializeField] private GameObject firstSelectedOverride;
    private EventSystem eventSystem;

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) Cursor.lockState = CursorLockMode.Confined;
        try { eventSystem = FindFirstObjectByType<EventSystem>().GetComponent<EventSystem>(); }
        catch
        {
            eventSystem = gameObject.AddComponent<EventSystem>().GetComponent<EventSystem>();
            gameObject.AddComponent<UnityEngine.InputSystem.UI.InputSystemUIInputModule>();
        }

        eventSystem.firstSelectedGameObject = firstSelectedOverride == null ? gameObject : firstSelectedOverride;
    }

    public void OpenSceneByName(string sceneName)
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(sceneName);
    }

    public void OpenOptionsPanel()
    {
        Instantiate((GameObject)Resources.Load($"UI/{"Options Menu"}"), transform);
    }

    public void OpenSceneByIndex(int index)
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(index);
    }

    public void ReloadCurrentScene()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayClip(AudioClip clip)
    {
        try { gameObject.GetComponent<AudioSource>().PlayOneShot(clip); }
        catch { gameObject.AddComponent<AudioSource>().PlayOneShot(clip); }
    }

    public void ToggleVisible()
    {
        SetVisible(!gameObject.activeInHierarchy);
    }

    public void SetVisible(bool isVisible)
    {
        gameObject.SetActive(isVisible);
    }

    public void SetClockScale(float newTimeScale)
    {
        Time.timeScale = newTimeScale;
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public void DisposeSelf()
    {
        Destroy(gameObject);
    }
}
