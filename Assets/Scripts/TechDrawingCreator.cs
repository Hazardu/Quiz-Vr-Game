using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class TechDrawingCreator : MonoBehaviour
{
    public static TechDrawingCreator instance;
    public static float _AngleTolerance = 0.1f;
    public static float SinTolerance;

    public static bool done;

    private void Start()
    {
        instance = this;
        SinTolerance = Mathf.Sin(Mathf.PI * _AngleTolerance / 180f);
    }


    public void Create(Mesh mesh, Transform tr)
    {
        done = false;
        StartCoroutine(StartCreating(mesh, tr));
    }

    public class Line
    {
        public int A, B;
        public Vector3 normal;

        public Line(int a, int b, Vector3 normal)
        {
            A = a;
            B = b;
            this.normal = normal;
        }
    }
    public struct LineKey
    {
        public int A, B;

        public LineKey(int a, int b)
        {
            A = a;
            B = b;
        }
    }

    public struct Edge
    {
        public Vector3 vert1, vert2;
        public float opacity;

        public Edge(Vector3 vert1, Vector3 vert2, float opacity)
        {
            this.vert1 = vert1;
            this.vert2 = vert2;
            this.opacity = opacity;
        }
    }
    private IEnumerator StartCreating(Mesh mesh, Transform tr)
    {
        List<Vector3> verts = new List<Vector3>();
        List<int> triangles = new List<int>();

        mesh.GetVertices(verts);
        for (int i = 0; i < mesh.subMeshCount; i++)
        {
            triangles.AddRange(mesh.GetTriangles(i));
        }

        Edge[] result = null;

        Thread thread = new Thread((object o) =>
        {
            result = RunThread(o);

        }
        );
        thread.Start(new object[] { verts.ToArray(), triangles.ToArray() });

        while (thread.IsAlive)
        {
            yield return null;
        }
       // TechDrawBehaviour.drawnTransform = tr;
        //TechDrawBehaviour.drawnMesh = mesh;
        TechDrawBehaviour.edges = result;
        TechDrawBehaviour.scale = tr.localScale;
        MultiView.instance.SetTarget(tr);
        done = true;
    }
    private static Edge[] RunThread(object parameter)
    {
        object[] _parameters = (object[])parameter;

        Vector3[] verts = (Vector3[])_parameters[0];
        int[] tris = (int[])_parameters[1];
        int l = tris.Length / 3;

        Dictionary<Vector3, int> dupes = new Dictionary<Vector3, int>();
        int[] vertsNoDupes = new int[verts.Length];
        for (int i = 0; i < verts.Length; i++)
        {
            Vector3 v = verts[i];
            if (dupes.ContainsKey(v))
            {
                vertsNoDupes[i] = dupes[v];
            }
            else
            {
                dupes.Add(v, i);
                vertsNoDupes[i] = i;

            }
        }

        List<Edge> sharpEdges = new List<Edge>();
        Dictionary<LineKey, Line> lines = new Dictionary<LineKey, Line>();


        for (int i = 0; i < l; i++)
        {
            int A = tris[i * 3];
            int B = tris[i * 3 + 1];
            int C = tris[i * 3 + 2];
            Vector3 normal = Vector3.Cross((verts[B] - verts[A]), (verts[C] - verts[A]));

            var keyA = GetLineKey(vertsNoDupes[A], vertsNoDupes[B], verts[A], verts[B]);
            var keyB = GetLineKey(vertsNoDupes[A], vertsNoDupes[C], verts[A], verts[C]);
            var keyC = GetLineKey(vertsNoDupes[C], vertsNoDupes[B], verts[C], verts[B]);

            CheckLine(lines, keyA, normal, sharpEdges, verts[A], verts[B], vertsNoDupes[A], vertsNoDupes[B]);
            CheckLine(lines, keyB, normal, sharpEdges, verts[A], verts[C], vertsNoDupes[A], vertsNoDupes[C]);
            CheckLine(lines, keyC, normal, sharpEdges, verts[C], verts[B], vertsNoDupes[C], vertsNoDupes[B]);
        }
        foreach (var line in lines)
        {
            sharpEdges.Add(new Edge(verts[line.Value.A], verts[line.Value.B], 1));

        }

        return sharpEdges.ToArray();
    }
    public static void CheckLine(Dictionary<LineKey, Line> lines, LineKey keyA,Vector3 normal, List<Edge> sharpEdges,Vector3 vA,Vector3 vB,int A,int B)
    {

        if (lines.ContainsKey(keyA))
        {
            Vector3 n = lines[keyA].normal;
            float angle = Vector3.Angle(normal, n);
            float sin = Mathf.Sin(Mathf.PI * angle / 180f);
            if (sin > SinTolerance)
            {
                sharpEdges.Add(new Edge(vA,vB, sin));
            }
            lines.Remove(keyA);
        }
        else
        {
            lines.Add(keyA, new Line(A,B, normal));
        }
    }
    public static LineKey GetLineKey(int a, int b, Vector3 va, Vector3 vb)
    {
        if (va.x < vb.x)
        {
            return new LineKey(a, b);
        }
        else if (va.x > vb.x)
        {
            return new LineKey(b, a);
        }
        else
        {
            if (va.y < vb.y)
            {
                return new LineKey(a, b);
            }
            else if (va.y > vb.y)
            {
                return new LineKey(b, a);
            }else
            {
                if (va.z < vb.z)
                {
                    return new LineKey(a, b);
                }
                else if (va.z > vb.z)
                {
                    return new LineKey(b, a);
                }
                return new LineKey(a, b);
            }
        }
    }
}
