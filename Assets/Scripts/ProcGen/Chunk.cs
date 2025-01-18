using UnityEngine;
using UnityEngine.Assertions;


public class Chunk : MonoBehaviour
{
    public MeshFilter MeshFilter;
    public MeshRenderer MeshRenderer;

    public void Awake()
    {
        MeshFilter = GetComponent<MeshFilter>();
        MeshRenderer = GetComponent<MeshRenderer>();

        Assert.IsNotNull(MeshFilter);
        Assert.IsNotNull(MeshRenderer);
    }

    public void CreateMesh(MeshData data)
    {
        Mesh mesh = data.CreateMesh();
        MeshFilter.sharedMesh = mesh;
    }

    public void CreateMeshAndOverrideMaterial(MeshData data, Material material)
    {
        Mesh mesh = data.CreateMesh();
        MeshFilter.sharedMesh = mesh;
        MeshRenderer.material = material;
    }
}