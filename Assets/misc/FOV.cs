using UnityEngine;
using Game.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

public class FOV : MonoBehaviour
{
    [SerializeField] LayerMask layerMask;
    [SerializeField] float fov = 300f;
    [SerializeField] float viewDistance = 4f;
    [SerializeField] float startingAngle = 0f;
    [SerializeField] string targetTag;

    GameSession session;
    Transform target;
    readonly int rayCount = 40;
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

        vertices[0] = target.position - new Vector3(0f, 0f, 3f);

        Iterator<(int, int)>
            .Of((1, -3), ((int a, int b) t) => (t.a + 1, t.b + 3))
            .Take(rayCount + 1)
            .ForEach(((int vertexIdx, int triangleIdx) t) => {
                Vector3 direction = Vectors.ToVector3(angle);
                bool isHit = Physics.Raycast(
                    vertices[0], direction, out RaycastHit hit, viewDistance, layerMask);

                if (isHit) vertices[t.vertexIdx] = hit.point;
                else vertices[t.vertexIdx] = vertices[0] + direction * viewDistance;

                if (t.triangleIdx >= 0)
                {
                    triangles[t.triangleIdx] = 0;
                    triangles[t.triangleIdx + 1] = t.vertexIdx - 1;
                    triangles[t.triangleIdx + 2] = t.vertexIdx;
                }

                angle -= angleIncrease;
            });

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.bounds = new Bounds(vertices[0], Vector3.one * 1000f);
    }
}
