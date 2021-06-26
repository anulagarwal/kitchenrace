using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeTopHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] internal int stageNumber = 0;

    [Header("Components Reference")]
    [SerializeField] private Transform nextStageTransform = null;
    [SerializeField] private SweetsPacketManager sweetsPacketManager = null;
    #endregion

    #region Getter And Setter
    public Transform GetNextStageTransform { get => nextStageTransform; }

    public SweetsPacketManager GetSweetsPacketManager { get => sweetsPacketManager; }
    #endregion
}
