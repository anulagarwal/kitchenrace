using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterTriggerEventsHandler : MonoBehaviour
{
    [Header("Components Reference")]
    [SerializeField] private PlayerMovementHandler playerMovementHandler = null;
    [SerializeField] private CharacterAnimationHandler characterAnimationHandler = null;

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Finish")
        {
            playerMovementHandler.enabled = false;
            characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Victory);
        }
    }
    #endregion
}
