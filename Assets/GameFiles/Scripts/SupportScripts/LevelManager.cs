using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Properties
    public static LevelManager Instance = null;

    [Header("Components Reference")]
    [SerializeField] internal List<StageHandler> stageHandlers = new List<StageHandler>(); 
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
    public Transform GetTargetBridge(int index)
    {
        //return stageHandlers[index].bridgeTransforms[Random.Range(0, stageHandlers[index].bridgeTransforms.Count)];
        return stageHandlers[index].bridgeTransforms[Random.Range(0, stageHandlers[index].bridgeTransforms.Count)];
    }
    #endregion
}
