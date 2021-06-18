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
        if (other.gameObject.tag == "Sweet" && other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterData.GetCharacterCode)
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            characterSweetStackHandler.StackSweet(other.transform);
            Vibration.Vibrate(30);

            if (characterData.GetCharacterCode != CharacterCode.Player && characterData.GetCharacterCode != CharacterCode.None)
            {
                if (gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler))
                {
                    enemyMovementHandler.sweetCollectionCount -= 1;

                    if (enemyMovementHandler.aIMovementType == AIMovementType.Stacking)
                    {
                        enemyMovementHandler.ChangeDestination();
                    }
                }
            }
        }
        else if (other.gameObject.tag == "Stair")
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                Handheld.Vibrate();
                if (other.gameObject.TryGetComponent<StairHandler>(out StairHandler stairHandler))
                {
                    if (stairHandler.OwnerCode != characterData.GetCharacterCode)
                    {
                        stairHandler.ChangeStairColor(characterData.GetColorCode, characterData.GetCharacterCode);
                        characterSweetStackHandler.ReleaseSweet();

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
            if (gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler))
            {
                if (other.gameObject.GetComponent<BridgeTopHandler>().stageNumber > enemyMovementHandler.stage)
                {
                    enemyMovementHandler.enabled = false;
                    enemyMovementHandler.NullifyTargetDestinationTransform();
                    enemyMovementHandler.aIMovementType = AIMovementType.ChangingStage;
                    enemyMovementHandler.targetLocationTransform = other.gameObject.GetComponent<BridgeTopHandler>().GetNextStageTransform;
                    enemyMovementHandler.UpdateStage();
                    other.gameObject.GetComponent<BoxCollider>().enabled = false;
                    enemyMovementHandler.enabled = true;
                }
            }
            else if (gameObject.TryGetComponent<PlayerMovementHandler>(out PlayerMovementHandler playerMovementHandler))
            {
                if (other.gameObject.GetComponent<BridgeTopHandler>().stageNumber > playerMovementHandler.stage)
                    playerMovementHandler.UpdateStage();
            }
        }


        if (other.gameObject.tag == "Enemy")
        {
            if (other.gameObject.TryGetComponent<EnemyMovementHandler>(out EnemyMovementHandler enemyMovementHandler))
            {
                if (enemyMovementHandler.characterSweetStackHandler.sweetStack.Count < characterSweetStackHandler.sweetStack.Count)
                {
                    //enemyMovementHandler.ApplyStumbleForce((transform.position - other.gameObject.transform.position).normalized);
                    enemyMovementHandler.characterSweetStackHandler.EnablePhysics();
                    enemyMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                }
                else
                {
                    characterSweetStackHandler.EnablePhysics();
                    playerMovementHandler.characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Stumble);
                }
            }
            //Handheld.Vibrate();
        }

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
