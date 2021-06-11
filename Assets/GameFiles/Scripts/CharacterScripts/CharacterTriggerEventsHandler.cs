using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterTriggerEventsHandler : MonoBehaviour
{
    #region Properties 
    [Header("Attributes")]
    [SerializeField] private CharacterCode characterCode = CharacterCode.None;

    [Header("Components Reference")]
    [SerializeField] private CharacterSweetStackHandler characterSweetStackHandler = null;
    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sweet" && other.gameObject.GetComponent<SweetsHandler>().SweetCode == characterCode)
        {
            characterSweetStackHandler.StackSweet(other.transform);
            other.gameObject.GetComponent<BoxCollider>().enabled = false;

            if (characterCode != CharacterCode.Player && characterCode != CharacterCode.None)
            {
                gameObject.GetComponent<EnemyMovementHandler>().ChangeDestination();
            }
        }
    }
    #endregion
}
