using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsManager : MonoBehaviour
{
    #region Properties
    public static SweetsManager Instance = null;

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
}
