using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsManager : MonoBehaviour
{
    #region Properties
    public static SweetsManager Instance = null;

    [Header("Components Reference")]
    [SerializeField] internal List<SweetsHandler> sweetsHandlers = new List<SweetsHandler>();
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
