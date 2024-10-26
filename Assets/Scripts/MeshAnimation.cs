using UnityEngine;

public class MeshAnimation : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    MeshCollider meshCollider;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
    }

    void Update()
    {
        UpdateCollider();
    }

    void UpdateCollider()
    {
        Mesh mesh = new Mesh();
        skinnedMeshRenderer.BakeMesh(mesh);

        if (IsMeshValid(mesh))
        {
            meshCollider.sharedMesh = null;
            meshCollider.sharedMesh = mesh;
        }
        else
        {
            Debug.LogError("Mesh contains non-finite values.");
        }
    }

    bool IsMeshValid(Mesh mesh)
    {
        Vector3[] vertices = mesh.vertices;
        foreach (Vector3 vertex in vertices)
        {
            if (float.IsNaN(vertex.x) || float.IsNaN(vertex.y) || float.IsNaN(vertex.z) ||
                float.IsInfinity(vertex.x) || float.IsInfinity(vertex.y) || float.IsInfinity(vertex.z))
            {
                return false;
            }
        }
        return true;
    }
}
