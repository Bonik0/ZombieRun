using TMPro;
using UnityEngine.Serialization;
using UnityEngine;

public class EndScreenLoader : MonoBehaviour
{
    public SessionConfig settings;
    [SerializeField] private TMP_Text waveReadout;

    private void Start()
    {
        waveReadout.text = $"WAVE {settings.clearedWave}";
    }
}
