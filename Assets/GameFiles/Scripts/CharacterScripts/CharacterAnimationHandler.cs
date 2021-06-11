using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private Animator characterAnimator = null;
    #endregion

    #region Public Core Functions
    public void SwitchCharacterAnimation(CharacterAnimationState state)
    {
        switch (state)
        {
            case CharacterAnimationState.Idle:
                characterAnimator.SetBool("b_Run", false);
                break;
            case CharacterAnimationState.Run:
                characterAnimator.SetBool("b_Run", true);
                break;
            case CharacterAnimationState.Victory:
                characterAnimator.SetBool("b_Run", false);
                characterAnimator.SetTrigger("t_Victory");
                break;
            case CharacterAnimationState.Defeat:
                characterAnimator.SetBool("b_Run", false);
                characterAnimator.SetTrigger("t_Defeat");
                break;
        }
    }
    #endregion
}
