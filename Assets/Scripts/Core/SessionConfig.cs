using UnityEngine;

[CreateAssetMenu(fileName = "SessionConfig", menuName = "Scriptable Objects/Session Config")]
public class SessionConfig : ScriptableObject
{
    [Header("Session")]
    public int targetFrameRate = 60;
    public int clearedWave = 0;

    [Header("Control")]
    [Tooltip("If the player can press 'Q' to quick switch to their previous weapon")]
    public bool canQuickSwap = true;
    [Tooltip("How much the player heals per second")]
    public float recoveryRate = 1.5f;
    [Tooltip("Time in seconds before the player start to heal")]
    public float recoveryDelay = 5.5f;

    [Header("Opposition")]
    [Tooltip("Maximum number of Enemies allowed to exists at once")]
    public int maxHostileCount = 30;
}
