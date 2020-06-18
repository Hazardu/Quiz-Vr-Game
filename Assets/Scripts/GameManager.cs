using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public int pointsToWin = 21,startingLives = 3;
    private int currentPoints = 0,currentLives;
    public static int GoalPoints => instance.pointsToWin;
    public static int Points => instance.currentPoints;

    public static int StartLives => instance.startingLives;
    public static int CurrentLives => instance.currentLives;

    private void Awake()
    {
        instance = this;
        currentLives = startingLives;
    }
    public bool enableGrabbing,enablePicking,enableRotateGrabbing;
    public static bool EnableGrabbing => instance.enableGrabbing&& instance.enablePicking;
    public static bool EnablePicking => instance.enablePicking;
    public static bool EnableRotating => instance.enableRotateGrabbing;

    public static void SubmitCorrectAnswer()
    {
        instance.currentPoints++;
        if(instance.currentPoints == instance.pointsToWin)
        {
            instance.win.Invoke();
            return;
        }
        instance.correctAnswerSubmitted.Invoke();
    }
    public static void SubmitIncorrectAnswer()
    {
        instance.currentLives--;
        if(instance.currentLives <= 0)
        {
            instance.loose.Invoke();
            return;
        }
        instance.incorrectAnswerSubmitted.Invoke();
    }
    private bool endlessMode;
    public static bool EndlessMode => instance.endlessMode;
    public void EnableEndlessMode()
    {
        endlessMode = true;
        onEndlessModeEnabled?.Invoke();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ReloadLevel()
    {
        SceneManager.LoadScene("MainGame");
    }

    public UnityEvent incorrectAnswerSubmitted,correctAnswerSubmitted,win,loose,onEndlessModeEnabled;


}

