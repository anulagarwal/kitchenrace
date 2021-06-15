using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementHandler : MonoBehaviour
{
    #region Properties
    public static PlayerSingleton Instance = null;

    [Header("Attributes")]
    [SerializeField] private float moveSpeed = 0f;

    [Header("Components Reference")]
    [SerializeField] private CharacterData characterData = null;
    [SerializeField] private Animator playerAnimator = null;
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] private CharacterSweetStackHandler characterSweetStackHandler = null;

    [Header("Gravity Setup")]
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float gravityInfluence = -9.81f;
    [SerializeField] private Transform groundCheckTrans = null;
    [SerializeField] private LayerMask groundCheckLayerMask = 0;

    private Vector3 movementDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private VariableJoystick movementJS = null;
    private CharacterAnimationHandler characterAnimationHandler = null;
    private bool isGrounded = false;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        movementJS = LevelUIManager.Instance.GetMovementJS;
        characterAnimationHandler = PlayerSingleton.Instance.GetCharacterAnimationHandler;

        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[0].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterData.GetCharacterCode)
            {
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }

    private void Update()
    {
        GravityMechanism();

        movementDirection = new Vector3(movementJS.Horizontal, 0, movementJS.Vertical).normalized;
        if (movementDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);

            if (!playerAnimator.GetBool("b_Run"))
            {
                characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Run);
            }
        }
        else
        {
            if (playerAnimator.GetBool("b_Run"))
            {
                characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
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
    #endregion
}