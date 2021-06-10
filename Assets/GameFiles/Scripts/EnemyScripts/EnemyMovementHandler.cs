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

    [Header("Gravity Setup")]
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float gravityInfluence = -9.81f;
    [SerializeField] private Transform groundCheckTrans = null;
    [SerializeField] private LayerMask groundCheckLayerMask = 0;

    private Vector3 movementDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = false;
    private SweetsHandler sweetsHandler = null;
    private Transform targetLocationTransform = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        foreach (SweetsHandler sh in SweetsManager.Instance.sweetsHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsHandler = sh;
                return;
            }
        }
    }

    private void Update()
    {
        GravityMechanism();
        NewLocation();

        if (Vector3.Distance(transform.position,targetLocationTransform.position) >= 0.1f)
        {
            movementDirection = (targetLocationTransform.position - transform.position).normalized;
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.LookRotation(movementDirection);

            if (!characterAnimator.GetBool("b_Run"))
            {
                characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Run);
            }
        }
        else
        {
            if (characterAnimator.GetBool("b_Run"))
            {
                characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
            }
            targetLocationTransform = null;
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
        foreach (GameObject obj in sweetsHandler.sweetObjs)
        {
            if (targetLocationTransform == null)
            {
                targetLocationTransform = obj.transform;
            }
            else
            {
                return;
            }
        }
    }
    #endregion
}
