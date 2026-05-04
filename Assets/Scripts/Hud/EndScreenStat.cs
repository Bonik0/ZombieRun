using TMPro;
using UnityEngine;

public class EndScreenStat : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI readout;
    public string[] lines;

    private void WriteLine()
    {
        readout.text = lines[Random.Range(0, lines.Length)];
    }

    private void Awake()
    {
        WriteLine();
    }
}
