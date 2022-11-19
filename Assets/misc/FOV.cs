using UnityEngine;
using Game.Utils;

public class FOV : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float fov = 300f;
    [SerializeField] float viewDistance = 4f;
    [SerializeField] float startingAngle = 0f;
    [SerializeField] string targetTag;

    GameSession session;
    Transform target;
    readonly int rayCount = 50;
    Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        session = FindObjectOfType<GameSession>();
        target = session.GetTaggedObject(targetTag).transform;
    }

    void LateUpdate()
    {
        DrawMaskShape();
    }

    void DrawMaskShape()
    {
        float angle = startingAngle + target.rotation.eulerAngles.z;
        float angleIncrease = fov / rayCount;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = target.position;

        int vertexIndex = 1;
        int triangleIndex = 0;
        foreach (int i in Colls.Range(rayCount + 1))
        {
            Vector3 direction = Vectors.ToVector3(angle);
            bool isHit = Physics.Raycast(
                vertices[0], direction, out RaycastHit hit, viewDistance, layerMask);

            if (isHit) vertices[vertexIndex] = hit.point;
            else vertices[vertexIndex] = vertices[0] + direction * viewDistance;

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
}
