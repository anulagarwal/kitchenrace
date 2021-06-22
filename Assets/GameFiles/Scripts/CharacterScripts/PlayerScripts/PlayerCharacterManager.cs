using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerCharacterManager : MonoBehaviour
{//
    #region Properties
    public static PlayerCharacterManager Instance = null;

    [Header("Components Reference")]
    [SerializeField] private List<GameObject> playerCharacters = new List<GameObject>();
    [SerializeField] private CinemachineVirtualCameraBase cmvcb = null;

    private int currentEnabledCharacter;
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
    public void EnablePlayerCharacter(int index)
    {
        foreach(GameObject obj in playerCharacters)
        {
            obj.SetActive(false);
        }
        cmvcb.Follow = playerCharacters[index].transform;
        playerCharacters[index].SetActive(true);
        currentEnabledCharacter = index;
    }

    public void EnablePlayer()
    {
        playerCharacters[currentEnabledCharacter].GetComponent<PlayerMovementHandler>().enabled = true;
    }

    public void DisablePlayer()
    {
        playerCharacters[currentEnabledCharacter].GetComponent<PlayerMovementHandler>().enabled = false;
    }
    #endregion
}
