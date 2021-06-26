using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorMovementHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 0f;
    [SerializeField] private Vector3 startPoint = Vector3.zero;
    [SerializeField] private Vector3 endPoint = Vector3.zero;

    private ElevatorMovementDirection elevatorMovementDirection = ElevatorMovementDirection.Up;
    #endregion

    #region Delegate Functions
    public delegate void ElevatorMechanism();

    public ElevatorMechanism elevatorMechanism;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        elevatorMechanism = null;
    }

    private void Update()
    {
        if (elevatorMechanism != null)
        {
            elevatorMechanism();
        }   
    }
    #endregion

    #region Getter And Setter
    public PlayerMovementHandler ActivePlayerMovementHandler { get; set; }

    public EnemyMovementHandler ActiveEnemyMovementHandler { get; set; }
    #endregion

    #region Private Core Functions
    private void UpMovement()
    {
        if (elevatorMovementDirection == ElevatorMovementDirection.Up)
        {
            if (transform.position.y < endPoint.y)
            {
                transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
            }
            else
            {
                ActivePlayerMovementHandler.ForceStop = false;
            }
        }
    }
    #endregion

    #region Public Core Functions
    public void EnableElevatorMovementUp()
    {
        elevatorMechanism = null;
        elevatorMechanism += UpMovement;
    }
    #endregion
}
