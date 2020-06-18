using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public static Transform right, left;
    [SerializeField] private Transform _r, _l;
    
    // Start is called before the first frame update
    void Awake()
    {
        right = _r;
        left = _l;
    }
}
