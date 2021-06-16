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

    [Header("Component References")]
    [SerializeField] private PlayerMovementHandler player;
    [SerializeField] private List<EnemyMovementHandler> enemies;


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
        LevelUIManager.Instance.UpdateLevelText(currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Win()
    {
        //Stop characters
        //Confetti
        //Victory animation
        isGameOn = false;
        IncreaseCurrentLevel();
        endTimer = Time.time;
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.Win);
        LevelUIManager.Instance.UpdateTimerText(endTimer - startTimer);       
    }

    public void Lose()
    {
        //Stop characters       
        isGameOn = false;
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.Lose);
        foreach (EnemyMovementHandler e in enemies)
        {
            e.enabled = false;
            
        }
        player.enabled = false;
    }

    public void StartLevel()
    {
        isGameOn = true;
        startTimer = Time.time;
        LevelUIManager.Instance.UpdateState(LevelUIManager.State.InGame);
        foreach(EnemyMovementHandler e in enemies)
        {
            e.enabled = true;
        }
        player.enabled = true;
        //Launch character
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

    public void RestartLevel()
    {
        SceneManager.LoadScene("Level " + currentLevel);
    }

    public void StartNextLevel()
    {
        SceneManager.LoadScene("Level " + currentLevel);
    }

    private int GetSceneNumber(int number)
    {
        return number / 3;
    }

    #endregion
}
