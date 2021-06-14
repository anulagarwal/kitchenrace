using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollisionAndTriggerEventsHandler : MonoBehaviour
{
    #region Properties 
    [Header("Components Reference")]
    [SerializeField] private CharacterData characterData = null;
    [SerializeField] private CharacterSweetStackHandler characterSweetStackHandler = null;
    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sweet" && other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterData.GetCharacterCode)
        {
            other.gameObject.GetComponent<BoxCollider>().enabled = false;
            characterSweetStackHandler.StackSweet(other.transform);

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
            else
            {
                print("StackEmpty");
            }
        }
        else if (other.gameObject.tag == "Blocker")
        {
            if (characterSweetStackHandler.GetSweetStackSize > 0)
            {
                other.gameObject.transform.parent.GetComponent<StairHandler>().EnableBlocker(false);
            }
        }

        if (other.gameObject.tag == "StairEnd")
        {
            if (gameObject.tag == "Enemy")
            {
                GetComponent<EnemyMovementHandler>().UpdateStage();
            }

            if(gameObject.tag =="Player")
            {

            }
        }
    }
    #endregion
}
