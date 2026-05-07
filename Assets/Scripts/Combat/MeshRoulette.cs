using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(MeshFilter))]
public class MeshRoulette : MonoBehaviour
{
    [SerializeField] private Mesh[] meshes;
    [SerializeField] private bool randomizeRotation;

    private void Start()
    {
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        if (randomizeRotation) transform.rotation = Random.rotation;
    }
}
