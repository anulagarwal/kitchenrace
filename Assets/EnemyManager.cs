using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    #region Properties
    public static EnemyManager Instance = null;

    [Header("Components Reference")]
    [SerializeField] internal List<EnemyMovementHandler> enemies = new List<EnemyMovementHandler>();
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

    #region Public Core Functions
    public void StartEnemy()
    {

    }

    public void EnemyLose()
    {
        foreach(EnemyMovementHandler e in enemies)
        {
            e.Lose();
        }
    }

    public void StopEnemies()
    {
        foreach(EnemyMovementHandler e in enemies)
        {
            e.Stop();
        }
    }
    #endregion
}
