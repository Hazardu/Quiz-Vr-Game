using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosingWalls : MonoBehaviour
{
    public Transform movingWall;
    public Vector3 lastLivePosition, startingPosition, endOfRoundPosition;

    public float speed;

    private float currentLerp;
    private float goalLerp;
    bool lost;
    public void OnWrongAns()
    {
        goalLerp =(float) GameManager.CurrentLives / GameManager.StartLives;
        goalLerp = 1 - goalLerp;
    }
    public void OnLost()
    {
        startingPosition = movingWall.position;
        lost = true;
        currentLerp = 0;
        goalLerp = 1;
    }
    private void Start()
    {
        startingPosition = movingWall.position;
    }


    private void Update()
    {
        if(!lost)
        {
        movingWall.position = Vector3.Lerp(startingPosition, lastLivePosition, currentLerp);
            if(currentLerp < goalLerp)
            {
                currentLerp += Time.deltaTime * speed;
            }

        }
        else
        {
        movingWall.position = Vector3.Lerp(startingPosition, endOfRoundPosition, currentLerp);
            if (currentLerp < goalLerp)
            {
                currentLerp += Time.deltaTime * speed;
            }
        }
    }

}
