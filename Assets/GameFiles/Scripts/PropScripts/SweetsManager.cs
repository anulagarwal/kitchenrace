using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsManager : MonoBehaviour
{
   
    #region Properties
    public static SweetsManager Instance = null;

    public List<SweetType> sweetTypes;
    public int currentSweetType;
    [Header("Components Reference")]
    [SerializeField] internal List<SweetsPacketManager> sweetsPacketManagers = new List<SweetsPacketManager>();
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

    #region Public functions

    public void SpawnSweets()
    {
        foreach(SweetsPacketManager spm in sweetsPacketManagers)
        {
            spm.SpawnSweets();
        }
    }
    public SweetType GetSweetType()
    {
        return sweetTypes[currentSweetType];
    }

    public void SetSweetType(int index)
    {
        currentSweetType = index;
    }
    #endregion
}
