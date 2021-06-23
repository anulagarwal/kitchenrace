using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private Animator characterAnimator = null;
    [SerializeField] private PlayerMovementHandler playerMovementHandler = null;
    [SerializeField] private EnemyMovementHandler enemyMovementHandler = null;
    #endregion

    #region Public Core Functions
    public void SwitchCharacterAnimation(CharacterAnimationState state)
    {
        switch (state)
        {
            case CharacterAnimationState.Idle:
                characterAnimator.SetBool("b_Stumble", false);
                characterAnimator.SetBool("b_Run", false);
                break;
            case CharacterAnimationState.Run:
                characterAnimator.SetBool("b_Stumble", false);
                characterAnimator.SetBool("b_Run", true);
                break;
            case CharacterAnimationState.Victory:
                characterAnimator.SetBool("b_Stumble", false);
                characterAnimator.SetBool("b_Run", false);
                characterAnimator.SetTrigger("t_Victory");
                break;
            case CharacterAnimationState.Defeat:
                characterAnimator.SetBool("b_Stumble", false);
                characterAnimator.SetBool("b_Run", false);
                characterAnimator.SetTrigger("t_Defeat");
                break;
            case CharacterAnimationState.Stumble:
                characterAnimator.SetTrigger("t_Stumble");
                characterAnimator.SetBool("b_Stumble", true);
                Invoke("DisableStumbling", 1f);
                break;
        }
    }
    #endregion

    #region Private Core Functions
    private void AnimEvent_StumbleStart()
    {
        if (playerMovementHandler)
        {
            playerMovementHandler.enabled = false;
        }
        else if (enemyMovementHandler)
        {
            enemyMovementHandler.enabled = false;
        }
    }

    private void AnimEvent_StumbleEnd()
    {
        if (playerMovementHandler)
        {
            playerMovementHandler.enabled = true;
        }
        else if (enemyMovementHandler)
        {
            enemyMovementHandler.enabled = true;
        }
    }
    #endregion

    #region Invoke Functions
    private void DisableStumbling()
    {
        characterAnimator.SetBool("b_Stumble", false);
    }
    #endregion
}
