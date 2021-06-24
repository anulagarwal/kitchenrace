using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{

    public static GameManager Instance = null;


    [Header ("Game Attributes")]
    private int currentLevel;
    private bool isGameOn;
    private float startTimer;
    private float endTimer;
    private int currentScore = 0;
    private int globalCoins = 0;


    [Header("Component References")]
    [SerializeField] private PlayerMovementHandler player;
    [SerializeField] private PlayerCharacterManager pcm;
    [SerializeField] private GameObject confetti;
    [SerializeField] private GameObject mainCam;
    [SerializeField] private GameObject endCam;
    [SerializeField] private Transform endStackPos;
    [SerializeField] private Animator chef;
    [SerializeField] private GameObject awesomeText;


    [Header("End Level Attributes")]
    [SerializeField] private float eatSpeed;
    [SerializeField] private int pointsPerStack;
    [SerializeField] private float shiftSpeed;
    private int finalMultiplier;


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

    // Start is called before the first frame update
    void Start()
    {
        currentLevel = PlayerPrefs.GetInt("level", 1);
        globalCoins = PlayerPrefs.GetInt("coins", 0);
        LevelUIManager.Instance.UpdateCoinCount(globalCoins);
        LevelUIManager.Instance.UpdateLevelText(currentLevel);        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win()
    {                
        isGameOn = false;       
        IncreaseCurrentLevel();
        endTimer = Time.time;
        confetti.SetActive(true);
        LevelUIManager.Instance.UpdateTimerText(endTimer - startTimer);
        PlayerSingleton.Instance.ShiftStack(endStackPos, shiftSpeed);
       
        // Invoke("EatRemainingStack",1.5f);
    }

    public void EatRemainingStack()
    {       
        PlayerSingleton.Instance.characterSweetStackHandler.EatStack(pointsPerStack, eatSpeed);
        chef.Play("Eat");
    }
    public void FinishEating()
    {
        Invoke("ShowWinUi", 3f);
    }
   
    public void ShowWinUI()
    {
        LevelUIManager.Instance.UpdateScoreText(currentScore);
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.Win);
        confetti.SetActive(false);
        confetti.SetActive(true);
        chef.Play("Victory");
    }
    public void SwitchCam()
    {
        mainCam.SetActive(false);
        //shiftCam.SetActive(false);
        endCam.SetActive(true);
    }
    public void AddCoins()
    {
        PlayerPrefs.SetInt("coins", globalCoins + currentScore);
    }

    public void AddDoubleCoins()
    {
        PlayerPrefs.SetInt("coins", globalCoins + (currentScore * finalMultiplier));
    }

    public void UpdateMultiplier(int value)
    {
        finalMultiplier = value;
        LevelUIManager.Instance.UpdateMultiplierScoreText(currentScore * finalMultiplier);
    }
    public void Lose()
    {
        //Stop characters       
        isGameOn = false;
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.Lose);
        EnemyManager.Instance.DisableEnemies();
        PlayerCharacterManager.Instance.DisablePlayer();
    }

    public void StartLevel()
    {
        isGameOn = true;
        startTimer = Time.time;
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.InGame);
        EnemyManager.Instance.EnableEnemies();
        PlayerCharacterManager.Instance.EnablePlayer();
        //Launch character
    }

    public void AddScore(int value, Vector3 pos)
    {
        currentScore += value;
        Instantiate(awesomeText, pos, Quaternion.identity);
        SoundManager.Instance.PlaySound(SoundType.Collect);
    }

    public void EndLevelScoreMultiplier()
    {

    }
    #region Scene Handlers

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public void IncreaseCurrentLevel()
    {
        currentLevel++;
        PlayerPrefs.SetInt("level", currentLevel);
    }

    public void LoadScene(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level " + GetSceneNumber(currentLevel));
    }

    public void StartNextLevel()
    {
        SceneManager.LoadScene("Level " + GetSceneNumber(currentLevel));
    }

    private int GetSceneNumber(int number)
    {
        return (int)Mathf.Ceil(number / 3f);        
    }

    #endregion
}
