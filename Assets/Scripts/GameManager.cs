using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public enum State { MainMenu, InGame, Win, Lose };
    #region Attributes

    [Header("Attributes")]
    public bool isPaused;

    [Header("Component Reference")]
    public GameObject confetti;

    [Header("GameplayStates")]
    public State gameState;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    // Start is called before the first frame update

    #region Monobehaviour functions
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #endregion


    public void StartLevel()
    {
        ChangeState(State.InGame);
    //    PlayerController.Instance.Fall();
    }

    public void WinLevel()
    {

       // PlayerController.Instance.Victory();
        ChangeState(State.Win);
        confetti.SetActive(true);
    }

    public void LoseLevel()
    {

    }

    public void LoadLevel(string s)
    {
        SceneManager.LoadScene(s);
    }

    public void ChangeState(State state)
    {
        gameState = state;
        UIManager.Instance.UpdateState(state);
    }
}
