using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using UnityEngine;

public class ModHandler : MonoBehaviour
{
    public ModObject prefab;
    public Transform pool;
    public AnswerManager answers;
    public string dir;
    public float objectScale = 1;
    private struct LoadMeshData
    {
        public string filename;
        public string textureFilename;
        public bool loadTexture;
    }

    // Start is called before the first frame update
    IEnumerator Start()
    {
        string CultureName = Thread.CurrentThread.CurrentCulture.Name;
        CultureInfo ci = new CultureInfo(CultureName);
        if (ci.NumberFormat.NumberDecimalSeparator != ".")
        {
            // Forcing use of decimal separator for numerical values
            ci.NumberFormat.NumberDecimalSeparator = ".";
            Thread.CurrentThread.CurrentCulture = ci;
        }
        Debug.Log("Starting loading of new objects");
        //DontDestroyOnLoad(gameObject);
        yield return null;
      var queue = LoadMeshes(dir);
        yield return null;

        if (queue != null)
        {
            while (queue.Count > 0)
            {
                var x = queue.Dequeue();
                Mesh mesh = MeshImporter.ImportFile(x.filename);

                if (x.loadTexture)
                {
                    Texture2D tex = null;
                    bool flag = false;
                    //t2 = new Thread(() =>
                    //    {
                    //        tex = new Texture2D(1, 1);
                            flag = tex.LoadImage(File.ReadAllBytes(x.textureFilename));
                        //}
                        //);

                    //start the threads
                    //t2.Start();

                    //wait for them to finish;
                    //while (t.IsAlive || t2.IsAlive)
                    //{
                    //    yield return null;
                    //}
                    ModObject obj = Instantiate(prefab, Vector3.down * 100, Quaternion.identity, pool);
                    obj.gameObject.SetActive(false);
                    obj.meshFilter.mesh = mesh;
                    if (flag)
                        obj.renderer.material.mainTexture = tex;
                    else
                    {
                        Debug.Log("Couldnt load image " + x.textureFilename);
                    }
                    obj.collider.sharedMesh = mesh;
                    var bounds = obj.renderer.bounds;
                    var s = bounds.size;
                    float max = Mathf.Max(s.x, s.y, s.z);
                    obj.transform.localScale /= max*objectScale;
                    yield return null;
                    answers.goPool.Add(obj.gameObject);
                    answers.meshPool.Add(obj.meshFilter);
                }
                else
                {
              
                    ////wait for the threads to finish;
                    //while (t.IsAlive)
                    //{
                    //    yield return null;
                    //}
                    ModObject obj = Instantiate(prefab, Vector3.down * 100, Quaternion.identity, pool);
                    obj.gameObject.SetActive(false);
                    obj.meshFilter.mesh = mesh;
                    obj.collider.sharedMesh = mesh;
                    mesh.RecalculateBounds();
                    var s = mesh.bounds.size;
                    float max = Mathf.Max(s.x, s.y, s.z);
                    obj.transform.localScale /= max * objectScale;
                    yield return null;
                    answers.goPool.Add(obj.gameObject);
                    answers.meshPool.Add(obj.meshFilter);
                }
            }
        }
    }

    

    static Queue<LoadMeshData> LoadMeshes(string dir)
    {
        var path = Application.dataPath+'/'+ dir;
        Debug.Log(path);
        if(!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            return null;
        }

        List<string> files = new List<string>(Directory.GetFiles(path));
        Queue<LoadMeshData> queue = new Queue<LoadMeshData>();
        for (int i = 0; i < files.Count; i++)
        {
            //accepted formats : OBJ
            if (files[i].EndsWith(".obj", System.StringComparison.CurrentCultureIgnoreCase))
            {
                //Queue meshes to load concurrently
                string textureFileName = files[i].TrimEnd('.', 'o', 'b', 'j') + ".png";
                bool flagTexture = File.Exists(textureFileName);
                queue.Enqueue(new LoadMeshData()
                {
                    filename = files[i],
                    loadTexture = flagTexture,
                    textureFilename = flagTexture ? textureFileName : "",
                });

            }
        }
        if (queue.Count > 0) return queue;
        return null;


    }
}
