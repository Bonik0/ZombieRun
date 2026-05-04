using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshRoulette : MonoBehaviour
{
    [SerializeField] private Mesh[] meshOptions;
    [SerializeField] private bool randomizeRotation;

    private void Start()
    {
        GetComponent<MeshFilter>().mesh = meshOptions[Random.Range(0, meshOptions.Length)];
        if (randomizeRotation) transform.rotation = Random.rotation;
    }
}
