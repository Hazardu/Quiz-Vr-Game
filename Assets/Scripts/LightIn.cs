using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIn : MonoBehaviour
{
    public GameObject[] objs;
    public int size;
    public float frequency = 1.4f;
    public float waitTime = 3;
    // Start is called before the first frame update
    public void Init()
    {
        StartCoroutine(StartAsync());
    }
    IEnumerator StartAsync()
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < objs.Length; i++)
        {
            if (i > 0 && i % size == 0) yield return new WaitForSeconds(frequency);
            objs[i].SetActive(true);
            objs[i].GetComponent<AudioSource>().Play();
        }
        objs = null;
    }
}
