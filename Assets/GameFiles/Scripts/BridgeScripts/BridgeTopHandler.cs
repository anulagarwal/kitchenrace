using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTopHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private Transform nextStageTransform = null;
    #endregion

    #region Getter And Setter
    public Transform GetNextStageTransform { get => nextStageTransform; }
    #endregion
}