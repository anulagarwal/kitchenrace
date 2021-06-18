using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelUIManager : MonoBehaviour
{
    #region Properties
    public static LevelUIManager Instance = null;
    public enum State { Main, InGame, Win, Lose};
    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuUIObj = null;
    [SerializeField] private GameObject gameplayUIObj = null;
    [SerializeField] private GameObject gameWinUIObj = null;
    [SerializeField] private GameObject gameLoseUIObj = null;


    [Header("Text Fields")]
    [SerializeField] private List<Text> levelTexts;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;


    [Header("Progress Bar")]
    [SerializeField] List<GameObject> progressBars;


    [Header("Gameplay UI Panel")]
    [SerializeField] private VariableJoystick movementJS = null;
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    #endregion

    #region Public Functions

    public void UpdateLevelText(int level)
    {
        foreach(Text t in levelTexts)
        {
            t.text = "LEVEL " + level;
        }
    }

    public void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.text = niceTime;
    }

    public void UpdateScoreText(string s)
    {
        scoreText.text = s;
    }
    public void UpdateState(State state)
    {
        switch (state)
        {
            case State.Main:
                mainMenuUIObj.SetActive(true);
                gameplayUIObj.SetActive(false);
                gameWinUIObj.SetActive(false);
                gameLoseUIObj.SetActive(false);
                break;
            case State.InGame:
                mainMenuUIObj.SetActive(false);
                gameplayUIObj.SetActive(true);
                gameWinUIObj.SetActive(false);
                gameLoseUIObj.SetActive(false);
                break;

            case State.Win:
                mainMenuUIObj.SetActive(false);
                gameplayUIObj.SetActive(false);
                gameWinUIObj.SetActive(true);
                gameLoseUIObj.SetActive(false);
                break;

            case State.Lose:
                mainMenuUIObj.SetActive(false);
                gameplayUIObj.SetActive(false);
                gameWinUIObj.SetActive(false);
                gameLoseUIObj.SetActive(true);
                break;
        }
    }

    //For win screen progress bar
    public void UpdateProgressBar(int number)
    {
        for(int i = 0; i< number; i++)
        {
            progressBars[i].SetActive(true);
        }
    }

    #endregion
    #region Getter And Setter
    public VariableJoystick GetMovementJS { get => movementJS; }
    #endregion
}
