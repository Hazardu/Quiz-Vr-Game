using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hints : MonoBehaviour
{

    public Transform rHand;
    public Transform head;
    public GameObject hintObj;
    public Text hintText;
    public float angle = 30;
    public float fadeSpeed = 1;

    bool showingHint;
    private void Start()
    {
        HideHint();
    }
    // Update is called once per frame
    void Update()
    {
        if (CheckAngle())
        {
            if (!showingHint)
                ShowHint();

        }
        else if (showingHint) HideHint();
    }
    
    public bool CheckAngle()
    {
        float a = Vector3.Angle(head.forward, rHand.forward);
        float b = Vector3.Angle(-head.right, rHand.forward);
        return (a < 90+angle && a > 90-angle)&&(b<angle && b > -angle);
            
    }
    void ShowHint()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }
    void HideHint()
    {

        StopAllCoroutines();
        StartCoroutine(FadeOut());

    }
    IEnumerator FadeIn()
    {
        showingHint = true;
        hintObj.SetActive(true);
        while (hintText.color.a<1)
        {
            var color = hintText.color;
            color.a+= Time.deltaTime * fadeSpeed;
            hintText.color = color;
            yield return null;
        }
    }
    IEnumerator FadeOut()
    {
        showingHint = false;
        while (hintText.color.a >0)
        {
            var color = hintText.color;
            color.a -= Time.deltaTime * fadeSpeed;
            hintText.color = color;
            yield return null;
        }
        hintObj.SetActive(false);
    }
}
