using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelUIManager : MonoBehaviour
{
    #region Properties
    public static LevelUIManager Instance = null;
    public enum State { Main, InGame, Win, Lose};
    public State currentState;
    [Header("UI Panel")]
    [SerializeField] private GameObject mainMenuUIObj = null;
    [SerializeField] private GameObject gameplayUIObj = null;
    [SerializeField] private GameObject gameWinUIObj = null;
    [SerializeField] private GameObject gameLoseUIObj = null;
    [SerializeField] private GameObject storeUIObj = null;

    [Header("Store UI Panel Setup")]
    [SerializeField] private Button LeftBtn = null;
    [SerializeField] private Button RightBtn = null;
    [SerializeField] private Button nextBtn = null;
    [SerializeField] private Button buyBtn = null;
    [SerializeField] private List<Sprite> characterRenders = new List<Sprite>();
    [SerializeField] private Image charactersImg = null;


    [Header("Text Fields")]
    [SerializeField] private List<Text> levelTexts;
    [SerializeField] private Text timerText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text coinText;
    [SerializeField] private Text doubleCoinText;
    [SerializeField] private Text normalCoinText;


    [Header("Settings")]
    [SerializeField] private Image SoundOn;
    [SerializeField] private Image SoundOff;
    [SerializeField] private Image VibrateOn;
    [SerializeField] private Image VibrateOff;

    [Header("Unlockable")]
    [SerializeField] private GameObject unlockClaimButton;
    [SerializeField] private Image unlockSprite;
    [SerializeField] private Text unlockPercent;



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
        LeftBtn.interactable = false;
        if (StoreManager.Instance.IsPlayable(storeCharacterIndex))
        {
            buyBtn.interactable = false;
            nextBtn.interactable = true;
        }
        else
        {
            buyBtn.interactable = true;
            nextBtn.interactable = false;
        }
      
        PlayerCharacterManager.Instance.EnablePlayerCharacter(storeCharacterIndex);
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

    public void UpdateCoinCount(int val)
    {
        coinText.text = "" + val;
    }

    public void UpdateTimerText(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        timerText.text = niceTime;
    }

    public void UpdateScoreText(int v)
    {
        scoreText.text = ""+v;
        normalCoinText.text = "$" + v + " IS ENOUGH";
    }

    public void UpdateMultiplierScoreText(int value)
    {
        doubleCoinText.text = "$" + (value);
    }

    public int GetMultiplierValue()
    {
        return GetComponent<MultiplierWheel>().GetCurrentMultiplier();
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
                GetComponent<MultiplierWheel>().Spin();
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

    public void UpdateUnlockableSprite(Image img)
    {
        
    }

    public void UpdateUnlockPercent(float value)
    {
        print(value);
        if (value >= 100)
        {
            unlockPercent.text = "UNLOCKED!";
            unlockClaimButton.GetComponent<Button>().interactable = true;

            unlockSprite.fillAmount = 1;


        }
        else
        {
            unlockPercent.text = "" + value+"%";
            unlockSprite.fillAmount =  value / 100;
            unlockClaimButton.GetComponent<Button>().interactable =false;

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
           // charactersImg.sprite = characterRenders[storeCharacterIndex];
            
            if (StoreManager.Instance.IsPlayable(storeCharacterIndex))
            {
                StoreManager.Instance.UpdatePrice(00, false);
                buyBtn.interactable = false;
                nextBtn.interactable = true;
            }
            else
            {
                StoreManager.Instance.UpdatePrice(storeCharacterIndex, true);
                buyBtn.interactable = true;
                nextBtn.interactable = false;
            }

            PlayerCharacterManager.Instance.EnablePlayerCharacter(storeCharacterIndex);

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

    public void UpdateSettings(bool sound, bool vibrate)
    {
        if (sound)
        {
            SoundOn.enabled = true;
            SoundOff.enabled = false;
        }
       else if (!sound)
        {
            SoundOn.enabled = false;
            SoundOff.enabled = true;
        }

        if (vibrate)
        {
            VibrateOn.enabled = true;
            VibrateOff.enabled = false;
        }

       else if (!vibrate)
        {
            VibrateOn.enabled = false;
            VibrateOff.enabled = true;
        }
    }

    public void OnClick_StoreLeftBtn()
    {
        storeCharacterIndex--;
        if (storeCharacterIndex > -1)
        {
           // charactersImg.sprite = characterRenders[storeCharacterIndex];
            
            if (StoreManager.Instance.IsPlayable(storeCharacterIndex))
            {
                StoreManager.Instance.UpdatePrice(00, false);
                buyBtn.interactable = false;
                nextBtn.interactable = true;
            }
            else
            {
                StoreManager.Instance.UpdatePrice(storeCharacterIndex, true);
                buyBtn.interactable = true;
                nextBtn.interactable = false;
            }

            PlayerCharacterManager.Instance.EnablePlayerCharacter(storeCharacterIndex);

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
