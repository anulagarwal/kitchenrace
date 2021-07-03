using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionAndTriggerEventsHandler : MonoBehaviour
{
    #region Properties 
    [Header("Components Reference")]
    [SerializeField] private CharacterData characterData = null;
    [SerializeField] private CharacterSweetStackHandler characterSweetStackHandler = null;
    [SerializeField] private PlayerMovementHandler playerMovementHandler = null;

    [Header("Attributes Reference")]
    [SerializeField] private Vector3 bumpForce;

    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sweet" && (other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterData.GetCharacterCode || other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterData.manipulationCode))
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            characterSweetStackHandler.StackSweet(other.transform);

            if (other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterData.manipulationCode)
            {
                other.gameObject.GetComponent<SweetsHandler>().GetCookieMeshRenderer.material.color = characterData.GetColorCode;
            }

            if (characterData.GetCharacterCode == CharacterCode.Player)
            {
                if (SettingsManager.Instance.isVibrateOn)
                {
                    //Vibration.Vibrate(30);
                    GameManager.Instance.Vibrate();
                }

                SoundManager.Instance.PlaySound(SoundType.Collect);

            }

            if (characterData.GetCharacterCode != CharacterCode.Player && characterData.GetCharacterCode != CharacterCode.None)
            {
                if (gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler_1))
                {
                    if (enemyMovementHandler_1.enabled)
                    {
                        enemyMovementHandler_1.sweetCollectionCount -= 1;

                        if (enemyMovementHandler_1.aIMovementType == AIMovementType.Stacking)
                        {
                            enemyMovementHandler_1.ChangeDestination();
                        }
                    }
                }
            }
        }
        else if (other.gameObject.tag == "Stair")
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                if (playerMovementHandler)
                {
                    playerMovementHandler.Building = true;
                }

                if (other.gameObject.TryGetComponent<StairHandler>(out StairHandler stairHandler))
                {
                    if (stairHandler.OwnerCode != characterData.GetCharacterCode)
                    {
                        stairHandler.ChangeStairColor(characterData.GetColorCode, characterData.GetCharacterCode);
                        characterSweetStackHandler.ReleaseSweet();
                        if (characterData.GetCharacterCode == CharacterCode.Player)
                        {
                            SoundManager.Instance.PlaySound(SoundType.Bridge);
                        }
                        if (characterSweetStackHandler.GetSweetStackSize <= 0)
                        {
                            stairHandler.EnableBlocker(true);
                        }
                    }
                }
            }

        }
        else if (other.gameObject.tag == "Blocker")
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                other.gameObject.transform.parent.GetComponent<StairHandler>().EnableBlocker(false);
            }
        }
        else if (other.gameObject.tag == "JumpPad")
        {
            if (playerMovementHandler)
            {
                playerMovementHandler.JumpMechanism(true, other.GetComponent<Jumper>().jumpForce);
                playerMovementHandler.playerMovementType = PlayerMovementType.Jumping;
            }
        }
        else if (other.gameObject.tag == "ElevatorPlot")
        {
            if (characterData.GetCharacterCode == CharacterCode.Player)
            {
                playerMovementHandler.ForceStop = true;
                playerMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Idle);
                other.gameObject.GetComponent<ElevatorTriggerEventsHandler>().elevatorMovementHandler.EnableElevatorMovementUp();
                other.gameObject.GetComponent<ElevatorTriggerEventsHandler>().elevatorMovementHandler.ActivePlayerMovementHandler = playerMovementHandler;
            }
        }

        if (other.gameObject.tag == "Ground")
        {
            if (playerMovementHandler)
            {
                playerMovementHandler.playerMovementType = PlayerMovementType.Running;
                playerMovementHandler.velocity.y = -2f;
            }
        }

        if (other.gameObject.tag == "BridgeTop")
        {
            if (gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler_2))
            {
                if (other.gameObject.GetComponent<BridgeTopHandler>().stageNumber > enemyMovementHandler_2.stage)
                {
                    enemyMovementHandler_2.enabled = false;
                    enemyMovementHandler_2.NullifyTargetDestinationTransform();
                    enemyMovementHandler_2.aIMovementType = AIMovementType.ChangingStage;
                    enemyMovementHandler_2.targetLocationTransform = other.gameObject.GetComponent<BridgeTopHandler>().GetNextStageTransform;
                    enemyMovementHandler_2.UpdateStage(other.gameObject.GetComponent<BridgeTopHandler>().GetSweetsPacketManager);
                   // enemyMovementHandler_2.SetSweetsPacketHandler(other.gameObject.GetComponent<BridgeTopHandler>().GetSweetsPacketManager);
                    
                    other.gameObject.GetComponent<BoxCollider>().enabled = false;
                    enemyMovementHandler_2.enabled = true;
                }
            }
            else if (gameObject.TryGetComponent<PlayerMovementHandler>(out PlayerMovementHandler playerMovementHandler))
            {
                if (other.gameObject.GetComponent<BridgeTopHandler>().stageNumber > playerMovementHandler.stage)
                {
                    playerMovementHandler.UpdateStage(other.gameObject.GetComponent<BridgeTopHandler>().GetSweetsPacketManager);
                }
            }
        }

        if (other.gameObject.tag == "NextStage" && characterData.GetCharacterCode != CharacterCode.Player)
        {
            if (other.gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler_3))
            {
                enemyMovementHandler_3.NewLocation();
            }
        }

        if (other.gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler) && characterData.GetCharacterCode == CharacterCode.Player )
        {
            if (playerMovementHandler && !playerMovementHandler.Building)
            {
                if (enemyMovementHandler.characterSweetStackHandler.sweetStack.Count < characterSweetStackHandler.sweetStack.Count)
                {      
                    enemyMovementHandler.characterSweetStackHandler.EnablePhysics();
                    enemyMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                    enemyMovementHandler.ragdollEnablerObj.SetActive(true);
                    SoundManager.Instance.PlaySound(SoundType.Bump);
                }
                else
                {

                    characterSweetStackHandler.EnablePhysics();
                    if (playerMovementHandler)
                    {
                        playerMovementHandler.ragdollEnablerObj.SetActive(true);
                        playerMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                    }
                }
            }
        }

        if (other.gameObject.tag == "BridgeStart" && playerMovementHandler)
        {
            playerMovementHandler.Building = false;
        }

        //if (other.gameObject.tag == "Enemy")
        //{
        //    if (other.gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler))
        //    {
        //        if (enemyMovementHandler.characterSweetStackHandler.sweetStack.Count < characterSweetStackHandler.sweetStack.Count)
        //        {
        //            print("Inside");
        //            //enemyMovementHandler.ApplyStumbleForce((transform.position - other.gameObject.transform.position).normalized);
        //            enemyMovementHandler.characterSweetStackHandler.EnablePhysics();
        //            enemyMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
        //            SoundManager.Instance.PlaySound(SoundType.Bump);

        //        }
        //        else
        //        {
        //            print("GameOver");

        //            characterSweetStackHandler.EnablePhysics();
        //            if (playerMovementHandler)
        //            {
        //                playerMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
        //            }
        //        }
        //    }
        //    //Handheld.Vibrate();
        //}

        //if (other.gameObject.tag == "StairEnd")
        //{
        //    if (gameObject.tag == "Enemy")
        //    {
        //        GetComponent<EnemyMovementHandler>().UpdateStage();
        //    }

        //    if(gameObject.tag =="Player")
        //    {

        //    }
        //}
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground" && playerMovementHandler)
        {
            playerMovementHandler.playerMovementType = PlayerMovementType.Running;
            playerMovementHandler.velocity.y = -2f;
        }
    }
    #endregion
}
