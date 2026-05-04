using TMPro;
using UnityEngine;

public class EndScreenLoader : MonoBehaviour
{
    public SessionConfig config;
    [SerializeField] private TMP_Text waveReadout;

    private void Start()
    {
        waveReadout.text = $"WAVE {config.clearedWave}";
    }
}
