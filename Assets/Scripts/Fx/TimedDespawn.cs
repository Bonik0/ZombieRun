using UnityEngine;

public class TimedDespawn : MonoBehaviour
{
    [Header("Destroy After")]
    public bool destroyAfter;
    public float afterSeconds = 2;

    private void Start()
    {
        if (destroyAfter) Destroy(gameObject, afterSeconds);
    }
}
