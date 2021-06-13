using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private Transform bridgeTopTransform = null;
    #endregion

    #region MonoBehaviour Functions    
    #endregion

    #region Getter And Setter
    public Transform GetBridgeTopTransform { get => bridgeTopTransform; }
    #endregion
}
