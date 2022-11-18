using UnityEngine;
using Game.Utils;
using System.Collections.Generic;

public class FOV : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float fov = 300f;
    [SerializeField] float viewDistance = 4f;
    [SerializeField] float startingAngle = 0f;
    [SerializeField] GameObject player;
    readonly int rayCount = 50;
    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void LateUpdate()
    {
        float angle = startingAngle + player.transform.rotation.eulerAngles.z;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = player.transform.position;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;
            Vector3 direction = Vectors.ToVector3(angle);
            bool isHit = Physics.Raycast(
                vertices[0], direction, out RaycastHit hit, viewDistance, layerMask);
            if (isHit)
            {
                vertex = hit.point;
            }
            else vertex = vertices[0] + direction * viewDistance;
            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex++;
            angle -= angleIncrease;
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(vertices[0], Vector3.one * 1000f);
    }

    public void SetViewDistance(float viewDistance)
    {
        this.viewDistance = viewDistance;
    }
}
