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
    public Transform GetTargetBridge(int index, CharacterCode cc)
    {
        int c = 0;
        //return stageHandlers[index].bridgeTransforms[Random.Range(0, stageHandlers[index].bridgeTransforms.Count)];
        switch (cc)
        {
            case CharacterCode.Enemy_1:
                c = 0;
                break;
            case CharacterCode.Enemy_2:
                c = 1;
                break;
            case CharacterCode.Enemy_3:
                c = 2;
                break;
            case CharacterCode.Enemy_4:
                c = 3;
                break;

        }
        if(c >= stageHandlers[index].bridgeTransforms.Count)
        {
            c = Random.Range(0, stageHandlers[index].bridgeTransforms.Count);
        }
         return stageHandlers[index].bridgeTransforms[c];

        //for testing
       // return stageHandlers[index].bridgeTransforms[0];

    }
    #endregion
}
