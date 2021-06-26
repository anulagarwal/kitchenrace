using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorTriggerEventsHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] internal ElevatorMovementHandler elevatorMovementHandler = null;
    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            //elevatorMovementHandler.EnableElevatorMovementUp();
        }
    }
    #endregion
}
