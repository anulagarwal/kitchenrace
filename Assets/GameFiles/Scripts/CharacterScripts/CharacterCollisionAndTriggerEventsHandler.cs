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
                    Vibration.Vibrate(30);
                }
               
                    SoundManager.Instance.PlaySound(SoundType.Collect);
                
            }

            if (characterData.GetCharacterCode != CharacterCode.Player && characterData.GetCharacterCode != CharacterCode.None)
            {
                if (gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler_1))
                {
                    enemyMovementHandler_1.sweetCollectionCount -= 1;

                    if (enemyMovementHandler_1.aIMovementType == AIMovementType.Stacking)
                    {
                        enemyMovementHandler_1.ChangeDestination();
                    }
                }
            }
        }
        else if (other.gameObject.tag == "Stair")
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                print(other.gameObject.tag);

                if (other.gameObject.TryGetComponent<StairHandler>(out StairHandler stairHandler))
                {
                    if (stairHandler.OwnerCode != characterData.GetCharacterCode)
                    {
                        stairHandler.ChangeStairColor(characterData.GetColorCode, characterData.GetCharacterCode);
                        characterSweetStackHandler.ReleaseSweet();
                        if(characterData.GetCharacterCode == CharacterCode.Player)
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
                    enemyMovementHandler_2.UpdateStage();
                    other.gameObject.GetComponent<BoxCollider>().enabled = false;
                    enemyMovementHandler_2.enabled = true;
                }
            }
            else if (gameObject.TryGetComponent<PlayerMovementHandler>(out PlayerMovementHandler playerMovementHandler))
            {
                if (other.gameObject.GetComponent<BridgeTopHandler>().stageNumber > playerMovementHandler.stage)
                    playerMovementHandler.UpdateStage();
            }
        }

        if (other.gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler) && characterData.GetCharacterCode == CharacterCode.Player )
        {
            if (enemyMovementHandler.characterSweetStackHandler.sweetStack.Count < characterSweetStackHandler.sweetStack.Count)
            {
                //enemyMovementHandler.ApplyStumbleForce((transform.position - other.gameObject.transform.position).normalized);
                enemyMovementHandler.characterSweetStackHandler.EnablePhysics();
                enemyMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                SoundManager.Instance.PlaySound(SoundType.Bump);

            }
            else
            {

                characterSweetStackHandler.EnablePhysics();
                if (playerMovementHandler)
                {
                    playerMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                }
            }
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
    #endregion
}
