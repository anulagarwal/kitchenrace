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
    [SerializeField] private GameObject storeUIObj = null;

    [Header("Store UI Panel Setup")]
    [SerializeField] private Button LeftBtn = null;
    [SerializeField] private Button RightBtn = null;
    [SerializeField] private List<Sprite> characterRenders = new List<Sprite>();
    [SerializeField] private Image charactersImg = null;


    [Header("Text Fields")]
    [SerializeField] private List<Text> levelTexts;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;


    [Header("Progress Bar")]
    [SerializeField] List<GameObject> progressBars;


    [Header("Gameplay UI Panel")]
    [SerializeField] private VariableJoystick movementJS = null;

    private int storeCharacterIndex = 0;
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

    private void Start()
    {
        charactersImg.sprite = characterRenders[storeCharacterIndex];
        LeftBtn.interactable = false;
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

    #region Btn Events Functions
    public void OnClick_StoreBtn()
    {
        mainMenuUIObj.SetActive(false);
        gameplayUIObj.SetActive(false);
        gameLoseUIObj.SetActive(false);
        gameWinUIObj.SetActive(false);
        storeUIObj.SetActive(true);
    }

    public void OnClick_StoreExitBtn()
    {
        mainMenuUIObj.SetActive(true);
        gameplayUIObj.SetActive(false);
        gameLoseUIObj.SetActive(false);
        gameWinUIObj.SetActive(false);
        storeUIObj.SetActive(false);
    }

    public void OnClick_StoreRightBtn()
    {
        storeCharacterIndex++;
        if (storeCharacterIndex < characterRenders.Count)
        {
            charactersImg.sprite = characterRenders[storeCharacterIndex];

            if (storeCharacterIndex >= characterRenders.Count - 1)
            {
                RightBtn.interactable = false;
                LeftBtn.interactable = true;
            }
            else
            {
                RightBtn.interactable = true;
                LeftBtn.interactable = true;
            }
        }
    }

    public void OnClick_StoreLeftBtn()
    {
        storeCharacterIndex--;
        if (storeCharacterIndex > -1)
        {
            charactersImg.sprite = characterRenders[storeCharacterIndex];

            if (storeCharacterIndex <= 0)
            {
                RightBtn.interactable = true;
                LeftBtn.interactable = false;
            }
            else
            {
                RightBtn.interactable = true;
                LeftBtn.interactable = true;
            }
        }
    }
    #endregion
}
