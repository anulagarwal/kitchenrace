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
    [SerializeField] internal CharacterSweetStackHandler characterSweetStackHandler = null;

    [Header("Gravity Setup")]
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float gravityInfluence = -9.81f;
    [SerializeField] private Transform groundCheckTrans = null;
    [SerializeField] private LayerMask groundCheckLayerMask = 0;
    [SerializeField] private float jumpHeight = 0f;

    private Vector3 movementDirection = Vector3.zero;
    internal Vector3 velocity = Vector3.zero;
    private VariableJoystick movementJS = null;
    internal CharacterAnimationHandler characterAnimationHandler = null;
    private bool isGrounded = false;
    internal int stage = 0;

    private RaycastHit hit;
    internal PlayerMovementType playerMovementType = PlayerMovementType.Running; 
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        movementJS = LevelUIManager.Instance.GetMovementJS;
        playerMovementType = PlayerMovementType.Running;
        characterAnimationHandler = PlayerSingleton.Instance.GetCharacterAnimationHandler;

        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[stage].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterData.GetCharacterCode)
            {
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                characterSweetStackHandler.C_SweetsPacketHandler.EnableSweetsMeshRenderer();
                return;
            }
        }
    }

    private void Update()
    {
        GravityMechanism();

        print(playerMovementType);

        movementDirection = new Vector3(movementJS.Horizontal, 0, movementJS.Vertical).normalized;
        if (movementDirection != Vector3.zero && playerMovementType == PlayerMovementType.Running)
        {
            transform.rotation = Quaternion.LookRotation(movementDirection);
            characterController.Move(movementDirection * Time.deltaTime * moveSpeed);

            if (!playerAnimator.GetBool("b_Run"))
            {
                characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Run);
            }
        }
        else if (playerMovementType == PlayerMovementType.Jumping)
        {
            characterController.Move(Vector3.forward * Time.deltaTime * moveSpeed);
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
        isGrounded = Physics.Raycast(groundCheckTrans.position, groundCheckTrans.up, groundDistance, groundCheckLayerMask);


        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        velocity.y += gravityInfluence * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime);
    }
    #endregion

    #region Public Core Functions

    public void UpdateStage()
    {
        stage++;
        //Modified
        if (stage >= LevelManager.Instance.stageHandlers.Count)
        {
            // aIMovementType = AIMovementType.GameOver;
            //Win();
            SoundManager.Instance.PlaySound(SoundType.Victory);
            characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Victory);
            EnemyManager.Instance.EnemyLose();
            GameManager.Instance.Win();
            this.enabled = false;
            return;
        }

        characterSweetStackHandler.C_SweetsPacketHandler.EnableSweetsMeshRenderer();
        SelectSweetsPacketHandler();       
    }

  
    public void SelectSweetsPacketHandler()
    {
     
        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[stage].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterData.GetCharacterCode)
            {
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                characterSweetStackHandler.C_SweetsPacketHandler.EnableSweetsMeshRenderer();
                return;
            }
        }
    }

    public void JumpMechanism(bool value, float jumpValue)
    {
        if (value)
        {
            velocity.y = Mathf.Sqrt(jumpValue * -2 * gravityInfluence);
            //groundCheckTrans.local
        }
    }
    #endregion
}
