using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TechDrawBehaviour : MonoBehaviour
{
    public static Vector3 scale;
    public static TechDrawingCreator.Edge[] edges;
    public RenderProperties renderProperties;
    private float width;
    public bool Push, Pop;
    
    private void OnRenderObject()
    {
        width = renderProperties.width;
        //   if (!renderProperties.material)
        //{
        //    // Unity has a built-in shader that is useful for drawing
        //    // simple colored things. In this case, we just want to use
        //    // a blend mode that inverts destination colors.
        //    Shader shader = Shader.Find("Hidden/Internal-Colored");
        //    mat = new Material(shader);
        //    mat.hideFlags = HideFlags.HideAndDontSave;
        //    // Set blend mode to invert destination colors.
        //    mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        //    mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.DstAlpha);
        //    // Turn off backface culling, depth writes, depth test.
        //    mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
        //    mat.SetInt("_ZWrite", 0);
        //    mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        //}
        renderProperties.material.SetPass(0);


        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        //GL.MultMatrix(Matrix4x4.Rotate(transform.rotation));
        //GL.MultMatrix(transform.localToWorldMatrix);
        // Draw lines
        int edgecount = edges.Length;
        float r = renderProperties.color.r;
        float g = renderProperties.color.g;
        float b = renderProperties.color.b;
        var c = new Color(r, g, b, 1);
        if (width == 0)
        {
        GL.wireframe = true;
            GL.Begin(GL.LINES);
            for (int i = 0; i < edgecount; ++i)
            {
                // Vertex colors change from red to green
                // One vertex at transform position
                GL.Vertex(ScaledVector(edges[i].vert1));
                GL.Color(c);
                GL.Vertex(ScaledVector(edges[i].vert2));
                GL.Color(c);
                // Another vertex at edge of circle
            }
            GL.End();
        }
        else if (width > 0)
        {
            GL.Begin(GL.QUADS);
            for (int i = 0; i < edgecount; ++i)
            {
                var point1 = edges[i].vert1;
                var point2 = edges[i].vert2;

                Vector3 startPoint = point1;
                Vector3 endPoint = point2;

                //var diffx = Mathf.Abs(point1.x - point2.x);
                //var diffy = Mathf.Abs(point1.y - point2.y);

                //if (diffx > diffy)
                //{
                //    if (point1.x <= point2.x)
                //    {
                //        startPoint = point1;
                //        endPoint = point2;
                //    }
                //    else
                //    {
                //        startPoint = point2;
                //        endPoint = point1;
                //    }
                //}
                //else
                //{
                //    if (point1.y <= point2.y)
                //    {
                //        startPoint = point1;
                //        endPoint = point2;
                //    }
                //    else
                //    {
                //        startPoint = point2;
                //        endPoint = point1;
                //    }
                //}

                var angle = Mathf.Atan2(endPoint.y - startPoint.y, endPoint.x - startPoint.x);
                var perp = angle + Mathf.PI * 0.5f;

                var p1 = Vector3.zero;
                var p2 = Vector3.zero;
                var p3 = Vector3.zero;
                var p4 = Vector3.zero;

                var cosAngle = Mathf.Cos(angle);
                var cosPerp = Mathf.Cos(perp);
                var sinAngle = Mathf.Sin(angle);
                var sinPerp = Mathf.Sin(perp);

                var distance = Vector2.Distance(startPoint, endPoint);
                p1 =startPoint -transform.up*width;

                p2 = startPoint + transform.up * width;


                p3 = endPoint - transform.up * width;

                p4= endPoint + transform.up * width;


                GL.Color(c);
                GL.Vertex3(p1.x,p1.y,p1.z);
                GL.Vertex3(p2.x, p2.y, p2.z);
                GL.Vertex3(p3.x, p3.y, p3.z);
                GL.Vertex3(p4.x, p4.y, p4.z);

            }
            GL.End();
        }
    
        GL.wireframe = false;
        GL.PopMatrix();
    }
    public static Vector3 ScaledVector(Vector3 v)
    {
        return new Vector3(v.x * scale.x, v.y * scale.y, v.z * scale.z);
    }
}
