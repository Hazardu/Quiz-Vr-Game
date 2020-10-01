using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnswerManager : MonoBehaviour
{
    public Piedestal[] piedestals;
    public bool PickPredefined;
    public List<int[]> predefinedList;



    public List<GameObject> goPool;
    public List<MeshFilter> meshPool;

    private List<int> options;
    private GameObject[] currentActive;
    public bool answered;

    public Transform poolParentObject;

    private int correctAns;
    public float pauseDuration = 0.4f;
    public bool Submit(int n)
    {
        if(n==correctAns && !answered )
        {
            //win
            answered = true;
            Invoke("ResetAnswers", pauseDuration);
            GameManager.SubmitCorrectAnswer();
            return true;
        }else
        {
            GameManager.SubmitIncorrectAnswer();
            return false;
        }
    }
    public void ResetAnswers()
    {
        ClearAnswers();
        if (GameManager.Points == GameManager.GoalPoints && !GameManager.EndlessMode)
            return;
		foreach (var item in piedestals)
		{
            item.OnReset();
		}
        SetAnswers();
    }

    public static AnswerManager instance;
    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        if(poolParentObject != null)
        {

            goPool =new List<GameObject>();
            int a = 0;
            foreach (Transform item in poolParentObject)
            {
                goPool.Add( item.gameObject);
                a++;
            }
        }


        options = new List<int>();
        meshPool = new List<MeshFilter>();
        currentActive = new GameObject[piedestals.Length];
        int size = goPool.Count;
        for (int i = 0; i < size; i++)
        {
            goPool[i].SetActive(true);
            var meshFilter = goPool[i].GetComponent<MeshFilter>();
            if (meshFilter == null) { meshFilter = goPool[i].GetComponentInChildren<MeshFilter>(); }
            if (meshFilter == null) { Debug.Log("Cant find mesh filter for " + goPool[i].name); }
            else
                meshPool.Add(meshFilter);

            goPool[i].SetActive(false);
        }
        //Invoke( "SetAnswers",5);
    }
    void ClearAnswers()
    {
        foreach ( var item in currentActive)
        {
            item.SetActive(false);
        }
    }
 void SetAnswers()
    {
        options.Clear();
        if (!PickPredefined|| predefinedList.Count==0)
        {
            Debug.Log("Picking random set of answers");
            if (options.Count < piedestals.Length)
                for (int i = 0; i < goPool.Count; i++)
                {
                    options.Add(i);

                }
        }
        else
        {
            Debug.Log("Picking predefined set of answers");
            options.AddRange(predefinedList[0]);
            predefinedList.RemoveAt(0);
        }
        int len = options.Count;
        int n = Random.Range(0, Mathf.Min( piedestals.Length,len));
        correctAns = n;
        for (int i = 0; i < piedestals.Length; i++)
        {
            if(i >= len)
            {
                piedestals[i].ansNr = -1;
                piedestals[i].child = null;
                continue;
            }
            int r = Random.Range(0, options.Count);
            int index = options[r];
            var go = goPool[index];
            options.RemoveAt(r);
            piedestals[i].ansNr = i;
            piedestals[i].child = go.transform;
            piedestals[i].childRB = null;
            currentActive[i] = go;
            //go.SetActive(true);
            if (i == n)
            {
                go.transform.parent = null;
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                StartCoroutine(GenerateDrawing(go.transform, meshPool[index].sharedMesh, i));
                //MultiView.instance.SetTargetNoRB(go.transform);
                //TechDrawingCreator.instance.Create(, go.transform);
            }
            else
            {
                go.transform.SetParent(piedestals[i].target);
                go.transform.localPosition = Vector3.zero;
                go.transform.localRotation = Quaternion.identity;
                // go.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            }
        }
        
        answered = false;
    }

    IEnumerator GenerateDrawing(Transform tr, Mesh m,int i)
    {
        tr.position = Vector3.down * -100;
        tr.gameObject.SetActive(true);
        TechDrawingCreator.instance.Create(m,tr);
        while (!TechDrawingCreator.done)
        {
        yield return null;
        yield return null;
        }
        yield return null;
        MultiView.instance.SetTargetNoRB(tr);
        yield return null;
        currentActive[i] = tr.gameObject;
        tr.SetParent(piedestals[i].target);
        tr.localPosition = Vector3.zero;
        tr.localRotation = Quaternion.identity;
        yield return null;
        yield return null;
        for (int x = 0; x < currentActive.Length; x++)
        {

            currentActive[x].SetActive(true);
            currentActive[x].transform.SetParent(piedestals[x].target);
            currentActive[x].transform.localPosition = Vector3.zero;
            currentActive[x].transform.localRotation = Quaternion.identity;
        }
        
    }
}
