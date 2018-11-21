using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneLineRenderer : MonoBehaviour
{
    public Transform hook;
    public Camera camera;

    Mesh mesh;
    List<int> idx;
    List<Vector3> offsets;

    private void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        idx = new List<int>();
        offsets = new List<Vector3>();

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            if (mesh.vertices[i].y < -10)
            {
                idx.Add(i);
                offsets.Add(mesh.vertices[i] - hook.position);
            }
        }
    }

    private void Update()
    {
        Vector3[] vertices = mesh.vertices;

        for (int i = 0; i < idx.Count; i++)
        {
            vertices[idx[i]] = hook.position + offsets[i];
        }

        mesh.vertices = vertices;
    }
}
