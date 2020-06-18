using UnityEngine;

public class ScanningMeshVFX : MonoBehaviour
{
    public bool enable;
    public Mesh mesh;
    public Material material;
    public Vector3 position;
    public Quaternion rotation;
    public Color GlColor;

    public float Y = 0;
    public float r;
    public void Set()
    {
        Y = -Screen.height;
        r= 0;
    }

    private void Update()
    {
        if (Y < Screen.height) Y += Time.deltaTime* 0.3f*Screen.height;
        r += Time.deltaTime * 10;
       rotation= Quaternion.Euler(0, r, 0);
        material.SetFloat("_Height", Y);
    }
    private void OnRenderObject()
    {
        if (!enable) return;
       if(mesh != null && TechDrawBehaviour.edges!= null)
        {
            //Graphics.DrawMesh(mesh, position, rotation, material, 0);
            material.SetPass(0);
            GL.PushMatrix();
            // Set transformation matrix for drawing to
            // match our transform
            //GL.MultMatrix(Matrix4x4.Rotate(transform.rotation));
            //GL.MultMatrix(transform.localToWorldMatrix);
            GL.wireframe = true;
            // Draw lines
            GL.Begin(GL.LINES);
            int edgecount = TechDrawBehaviour.edges.Length;
            float r = GlColor.r;
            float g = GlColor.g;
            float b = GlColor.b;
            for (int i = 0; i < edgecount; ++i)
            {
                // Vertex colors change from red to green
                GL.Color(new Color(r, g, b, 1));
                // One vertex at transform position
                GL.Vertex(TechDrawBehaviour.edges[i].vert1);
                GL.Vertex(TechDrawBehaviour.edges[i].vert2);
                // Another vertex at edge of circle
            }
            GL.End();
            GL.wireframe = false;
            GL.PopMatrix();
        }
    }
}
