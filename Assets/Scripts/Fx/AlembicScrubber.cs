using System.IO;
using UnityEngine;
using UnityEngine.Formats.Alembic.Importer;

[RequireComponent(typeof(AlembicStreamPlayer))]
public class AlembicScrubber : MonoBehaviour
{
    [SerializeField] private bool loadExternalAsset = true;
    [Header("Importing .abc settings")]
    [Tooltip("file path if it is deeper nested in 'StreamingAssets'")]
    [SerializeField] private string pathExtension;
    [SerializeField] private string clipName;
    [SerializeField] private Material material;
    [SerializeField] private TimeValue startTime;
    [SerializeField] private TimeValue endTime;
    [SerializeField] private Vector3 scale = Vector3.one;
    [SerializeField] private Vector3 offset;

    [System.Serializable] public struct TimeValue
    {
        public bool @override;
        public float newTime;
    }

    [Header("Animation SessionConfig")]
    [Tooltip("If animation should start from beginning when looping")]
    public bool loopFromStart = true;
    [Tooltip("If animation should reverse back to 0 when looping")]
    public bool pingPongLooping;
    public float speedMultiplier = 1;
    private AlembicStreamPlayer player;

    private void LoadAsset()
    {
        if (string.IsNullOrEmpty(clipName)) return;
        string path = Path.Combine(Application.streamingAssetsPath, pathExtension, clipName + ".abc");
        player.LoadFromFile(path);
        if (startTime.@override) player.StartTime = startTime.newTime;
        if (endTime.@override) player.EndTime = endTime.newTime;
        transform.position += offset;
        transform.localScale = scale;
        foreach (MeshRenderer meshRenderer in gameObject.GetComponentsInChildren<MeshRenderer>()) meshRenderer.material = material;
    }

    private void Awake()
    {
        player = GetComponent<AlembicStreamPlayer>();
        if (loadExternalAsset) LoadAsset();
    }

    private void Start()
    {
        if (speedMultiplier == 0) Debug.LogWarning($"Speed Modifier is 0 on {gameObject.name}! No Animation will play.");
        if (loopFromStart && pingPongLooping) Debug.Log($"{gameObject.name} can not have both looping modifiers on at once! straightLooping will override.");
    }

    private void Update()
    {
        if (player.CurrentTime >= player.Duration && !loopFromStart && !pingPongLooping) return;
        player.CurrentTime += Time.deltaTime * speedMultiplier;
        if (player.CurrentTime >= player.Duration || player.CurrentTime <= 0)
        {
            if (loopFromStart) player.CurrentTime = 0;
            else if (pingPongLooping) speedMultiplier *= -1;
        }
    }
}
