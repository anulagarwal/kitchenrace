using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovementHandler : MonoBehaviour
{
    #region Properties
    public static PlayerSingleton Instance = null;

    [Header("Attributes")]
    [SerializeField] private CharacterCode characterCode = CharacterCode.None;
    [SerializeField] private float moveSpeed = 0f;

    [Header("Components Reference")]
    [SerializeField] private Animator characterAnimator = null;
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] private CharacterAnimationHandler characterAnimationHandler = null;
    [SerializeField] private CharacterSweetStackHandler characterSweetStackHandler = null;

    [Header("Gravity Setup")]
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float gravityInfluence = -9.81f;
    [SerializeField] private Transform groundCheckTrans = null;
    [SerializeField] private LayerMask groundCheckLayerMask = 0;

    private Vector3 movementDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = false;
    private SweetsPacketHandler sweetsPacketHandler = null;
    private Transform targetLocationTransform = null;
    private int stage = 0;
    private int sweetCollectionCount = 0;
    private AIMovementType aIMovementType = AIMovementType.Stacking;
    #endregion



    #region MonoBehaviour Functions
    private void Start()
    {
        sweetCollectionCount = Random.Range(2, 5);

        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[stage].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsPacketHandler = sh;
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }

    private void Update()
    {
        GravityMechanism();
        NewLocation();

        if (targetLocationTransform && aIMovementType == AIMovementType.Stacking)
        {
            if (Vector3.Distance(transform.position, targetLocationTransform.position) >= 0.2f)
            {
                movementDirection = (targetLocationTransform.position - transform.position).normalized;
                characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
                transform.rotation = Quaternion.LookRotation(movementDirection);

                if (!characterAnimator.GetBool("b_Run"))
                {
                    characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Run);
                }
            }

            if (characterSweetStackHandler.GetSweetStackSize >= sweetCollectionCount)
            {
                targetLocationTransform = LevelManager.Instance.GetTargetBridge(stage);
                aIMovementType = AIMovementType.Building;
            }
        }
        else if (targetLocationTransform && aIMovementType == AIMovementType.Building)
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                movementDirection = (targetLocationTransform.position - transform.position).normalized;
                characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
                transform.rotation = Quaternion.LookRotation(movementDirection);

                if (Vector3.Distance(transform.position, targetLocationTransform.position) <= 0.2f)
                {
                    targetLocationTransform = targetLocationTransform.GetComponent<BridgeHandler>().GetBridgeTopTransform;
                }
            }
            else
            {
                targetLocationTransform = null;
                NewLocation();
                aIMovementType = AIMovementType.Stacking;
            }
        }
    }
    #endregion

    #region Private Core Functions
    private void GravityMechanism()
    {
        isGrounded = Physics.Raycast(groundCheckTrans.position, -groundCheckTrans.up, groundDistance, groundCheckLayerMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravityInfluence * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }

    private void NewLocation()
    {
        if (sweetsPacketHandler.sweetObjs.Count > 0)
        {
            foreach (GameObject obj in sweetsPacketHandler.sweetObjs)
            {
                if (targetLocationTransform == null)
                {
                    targetLocationTransform = obj.transform;
                    targetLocationTransform.GetComponent<BoxCollider>().enabled = true;
                }
                else
                {
                    return;
                }
            }
        }
    }
    #endregion

    #region Public Core Functions
    public void ChangeDestination()
    {
        NewLocation();
        if (characterAnimator.GetBool("b_Run"))
        {
            characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
        }
        sweetsPacketHandler.sweetObjs.Remove(targetLocationTransform.gameObject);
        targetLocationTransform = null;
    }

    public void UpdateStage()
    {
        stage++;
        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[stage].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsPacketHandler = sh;
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }
    #endregion
}
