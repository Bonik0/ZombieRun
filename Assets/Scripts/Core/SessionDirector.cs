using System.Collections;
using UnityEngine.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(AudioBus))]
[RequireComponent(typeof(DebugOverlay))]
public class SessionDirector : MonoBehaviour
{
    public static SessionDirector Instance { get; private set; }

    public bool runIntro;
    private int activeStalkerCount;

    [Header("References")]
    public SessionConfig SessionConfig;
    public SessionConfig Config => SessionConfig;
    private AudioBus audioBus;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ApplyFrameSessionConfig();
        if (SessionConfig == null)
        {
            Debug.Break();
            Debug.LogError("NO CONFIG ASSIGNED!");
        }

        audioBus = GetComponent<AudioBus>();
        if (audioBus == null) audioBus = gameObject.AddComponent<AudioBus>();
        if (GetComponent<DebugOverlay>() == null) gameObject.AddComponent<DebugOverlay>();
        if (runIntro) StartCoroutine(RunIntroSequence());
    }

    public void PlayCue(string fileName)
    {
        audioBus.PlayCue(fileName);
    }

    public void PlayCue(AudioClip clip)
    {
        audioBus.PlayCue(clip);
    }

    public bool ReserveStalkerSlot()
    {
        if (activeStalkerCount >= SessionConfig.maxStalkerCount) return false;

        activeStalkerCount++;
        return true;
    }

    public void ReleaseStalkerSlot()
    {
        activeStalkerCount--;
    }

    public void RevealLoadoutSlot(int index)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<LoadoutSwitcher>().slotUnlocked[index] = true;
    }

    public void HideLoadoutSlot(int index)
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<LoadoutSwitcher>().slotUnlocked[index] = false;
    }

    private void Update()
    {
        if (Application.targetFrameRate != SessionConfig.targetFrameRate)
            Application.targetFrameRate = SessionConfig.targetFrameRate;
    }

    private void Reset()
    {
        transform.position = Vector3.zero;
        gameObject.tag = "GameController";
        gameObject.name = "--- SessionDirector ---";
    }

    public void ShowFailureScene()
    {
        OpenFailureScene();
    }

    private void OpenFailureScene() => SceneManager.LoadScene("GameOverScene");

    public void ShowVictoryScene()
    {
        SceneManager.LoadScene("WinScene");
    }

    public void PauseClock()
    {
        Time.timeScale = 0f;
    }

    public void ResumeClock()
    {
        Time.timeScale = 1f;
    }

    private IEnumerator RunIntroSequence()
    {
        GameObject latest = null;
        int index = 0;

        while (index < 4)
        {
            if (latest == null)
            {
                switch (index)
                {
                    case 0:
                        latest = HudRoot.Instance.SpawnHint("Look");
                        break;
                    case 1:
                        latest = HudRoot.Instance.SpawnHint("Walk");
                        break;
                    case 2:
                        latest = HudRoot.Instance.SpawnHint("Shoot");
                        break;
                    case 3:
                        latest = HudRoot.Instance.SpawnHint("Sprint");
                        break;
                }

                index++;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private void ApplyFrameSessionConfig()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
    }
}
