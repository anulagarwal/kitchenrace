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
    [SerializeField] private float stumbleforce = 0f;
    [SerializeField] private float rotSpeed = 0f;
    [SerializeField] private float jumpHeight = 0f;

    [Header("Components Reference")]
    [SerializeField] private Rigidbody rb = null;
    [SerializeField] private CapsuleCollider capsuleCollider = null;
    [SerializeField] private Animator characterAnimator = null;
    [SerializeField] private CharacterController characterController = null;
    [SerializeField] internal CharacterAnimationHandler characterAnimationHandler = null;
    [SerializeField] internal CharacterSweetStackHandler characterSweetStackHandler = null;
    [SerializeField] internal GameObject ragdollEnablerObj = null;

    [Header("Gravity Setup")]
    [SerializeField] private float groundDistance = 0f;
    [SerializeField] private float gravityInfluence = -9.81f;
    [SerializeField] private Transform groundCheckTrans = null;
    [SerializeField] private LayerMask groundCheckLayerMask = 0;

    private Vector3 movementDirection = Vector3.zero;
    private Vector3 velocity = Vector3.zero;
    private bool isGrounded = false;
    private SweetsPacketHandler sweetsPacketHandler = null;
    internal Transform targetLocationTransform = null;
    internal int stage = 0;
    internal int sweetCollectionCount = 0;
    internal AIMovementType aIMovementType = AIMovementType.Stacking;
    internal bool isStumbling = false;
    private float stumbleTimer = 1f;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        sweetCollectionCount = Random.Range(5, 7);
        isStumbling = false;

        foreach (SweetsPacketHandler sh in SweetsManager.Instance.sweetsPacketManagers[stage].sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsPacketHandler = sh;
                sweetsPacketHandler.EnableSweetsMeshRenderer();
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }

    private void Update()
    {
        if (aIMovementType != AIMovementType.GameOver)
        {
            GravityMechanism();

            if (aIMovementType != AIMovementType.ChangingStage)
            {
                NewLocation();
            }

            if (isStumbling)
            {
                if (isGrounded && stumbleTimer <= 0)
                {
                    isStumbling = false;
                    characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                }
                else
                {
                    stumbleTimer -= Time.deltaTime;
                }
            }
            else
            {
                stumbleTimer = 1;
            }

            if (targetLocationTransform && aIMovementType == AIMovementType.Stacking)
            {
                if (Vector3.Distance(transform.position, targetLocationTransform.position) >= 0.2f && !isStumbling)
                {
                    movementDirection = (targetLocationTransform.position - transform.position).normalized;
                    characterController.Move(movementDirection * Time.deltaTime * moveSpeed);

                    Quaternion newRot = Quaternion.LookRotation(movementDirection);
                    transform.rotation = Quaternion.Lerp(transform.rotation, newRot, rotSpeed * Time.deltaTime);
                    //transform.rotation = Quaternion.LookRotation(movementDirection);

                    if (!characterAnimator.GetBool("b_Run"))
                    {
                        characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Run);
                    }
                }

                if (sweetCollectionCount <= 0 || characterSweetStackHandler.C_SweetsPacketHandler.sweetObjs.Count <= 0)
                {
                    targetLocationTransform = LevelManager.Instance.GetTargetBridge(stage, characterCode);
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
                        if (targetLocationTransform.TryGetComponent<BridgeHandler>(out BridgeHandler bridgeHandler))
                        {
                            targetLocationTransform = bridgeHandler.GetBridgeTopTransform;
                        }
                    }
                }
                else
                {
                    targetLocationTransform = null;
                    NewLocation();
                    aIMovementType = AIMovementType.Stacking;

                    if (sweetCollectionCount <= 0)
                    {
                        sweetCollectionCount = Random.Range(5, 8);
                    }
                }
            }

            if (targetLocationTransform && aIMovementType == AIMovementType.ChangingStage)
            {
                if (Vector3.Distance(transform.position, targetLocationTransform.position) >= 0.2f)
                {
                    movementDirection = (targetLocationTransform.position - transform.position).normalized;
                    characterController.Move(movementDirection * Time.deltaTime * moveSpeed);
                    transform.rotation = Quaternion.LookRotation(movementDirection);
                }
                else
                {
                    NewLocation();
                    aIMovementType = AIMovementType.Stacking;
                    if (sweetCollectionCount <= 0)
                    {
                       // sweetCollectionCount = Random.Range(5, 8);
                    }
                    //characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
                }
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

        //if (!isGrounded && aIMovementType == AIMovementType.Stacking)
        //{
        //    Stop();
        //}
    }

    internal void NewLocation()
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
        //if (characterAnimator.GetBool("b_Run"))
        //{
        //    characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
        //}
        sweetsPacketHandler.sweetObjs.Remove(targetLocationTransform.gameObject);
        targetLocationTransform = null;
    }

    public void UpdateStage(SweetsPacketManager sweetsPacketManager)
    {
        stage++;
        //Modified
        if (stage >= LevelManager.Instance.stageHandlers.Count)
        {
            aIMovementType = AIMovementType.GameOver;
            Win();
            return;
        }

        foreach (SweetsPacketHandler sh in sweetsPacketManager.sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsPacketHandler = sh;
                sweetsPacketHandler.EnableSweetsMeshRenderer();
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }

    //Testing - not used
    public void SetSweetsPacketHandler(SweetsPacketManager sweetsPacketManager)
    {
        foreach (SweetsPacketHandler sh in sweetsPacketManager.sweetsPacketHandlers)
        {
            if (sh.GetCharacterCode == characterCode)
            {
                sweetsPacketHandler = sh;
                sweetsPacketHandler.EnableSweetsMeshRenderer();
                characterSweetStackHandler.C_SweetsPacketHandler = sh;
                return;
            }
        }
    }

    //public void ApplyStumbleForce(Vector3 direction)
    //{
    //    rb.isKinematic = false;
    //    capsuleCollider.isTrigger = false;
    //    rb.useGravity = true;
    //    rb.AddForce(direction * stumbleforce, ForceMode.Impulse);

    //    Invoke("DisablePhysics", 2f);
    //}

    public void ApplyStumbleForce(Vector3 direction)
    {
        isStumbling = true;
        velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravityInfluence);
        characterController.Move(direction * Time.deltaTime * moveSpeed);
    }

    //Added
    public void Lose()
    {        
        characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Defeat);
        Stop();
    }
    //Added
    public void Win()
    {
        characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Victory);
        GameManager.Instance.Lose();
        Stop();
    }

    public void Stop()
    {
        this.enabled = false;
    }
    public void NullifyTargetDestinationTransform()
    {
        targetLocationTransform = null;
    }
    #endregion

    #region Invoke Functions
    private void DisablePhysics()
    {
        capsuleCollider.isTrigger = true;
        rb.isKinematic = true;
        rb.useGravity = false;
        CancelInvoke();
    }
    #endregion
}
