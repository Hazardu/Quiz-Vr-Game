using System.Collections;
using UnityEngine;

public class MultiView : MonoBehaviour
{
    public static MultiView instance;
    //X,Y,Z
    public Camera[] Cameras;
    public Transform target;
    public Rigidbody targetRB;
    public float scale = 1f;
    public float dragSpeed = 5;
    public float rotationSpeed = 60;

    [Header("Replacing Material")]
    public Material material;
    public Shader shader;
    public string ReplacementTag;


    public Transform rhand, lhand;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private IEnumerator SetCameras()
    {
        MeshRenderer mr = target.GetComponent<MeshRenderer>();
        Vector3 size = mr.bounds.size;
        Vector3 center = mr.bounds.center;
        float maxSize = Mathf.Max(size.x, size.y, size.z);
        Material backupMat = mr.material;
        foreach (Camera cam in Cameras)
        {
            cam.orthographicSize = scale * maxSize;
        }
        Cameras[0].transform.position = -Vector3.right * 30;
        Cameras[0].transform.rotation = Quaternion.LookRotation(Vector3.right);

        Cameras[1].transform.position = Vector3.up * 30;
        Cameras[1].transform.rotation = Quaternion.LookRotation(-Vector3.up);

        Cameras[2].transform.position = -Vector3.forward * 30;
        Cameras[2].transform.rotation = Quaternion.LookRotation(Vector3.forward);
        yield return null;
        foreach (Camera cam in Cameras)
        {
            cam.gameObject.SetActive(true);
            cam.Render();
            cam.gameObject.SetActive(false);
        }

    }

    public void SetTarget(Transform tr)
    {
        if (target != null)
        {
            target.gameObject.layer = 10;
        }
        target = tr;
        targetRB = target.GetComponent<Rigidbody>();
        if (targetRB == null)
        {
            targetRB = target.gameObject.AddComponent<Rigidbody>();
        }

        tr.gameObject.layer = 9;
        targetRB.drag = 0.02f;
        targetRB.angularDrag = 0.02f;
        targetRB.useGravity = false;

        StartCoroutine(SetCameras());
    }

    public void SetTargetNoRB(Transform tr)
    {
        if (target != null)
        {
            target.gameObject.layer = 10;
        }
        target = tr;
        tr.gameObject.layer = 9;
        StartCoroutine(SetCameras());
    }


}
