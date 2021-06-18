using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSingleton : MonoBehaviour
{
    #region Properties
    public static PlayerSingleton Instance = null;

    [Header("Components Reference")]
    [SerializeField] private CharacterAnimationHandler characterAnimationHandler = null;
    [SerializeField] internal CharacterSweetStackHandler characterSweetStackHandler = null;

    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    #endregion

    #region Public functions

    public void Lose()
    {
        GetComponent<PlayerMovementHandler>().enabled = false;
        characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Defeat);
    }

    public void ShiftStack(Transform t)
    {
        characterSweetStackHandler.transform.SetParent(t);
        characterSweetStackHandler.transform.localPosition = Vector3.zero;
    }

    #endregion

    #region Getter And Setter
    public CharacterAnimationHandler GetCharacterAnimationHandler { get => characterAnimationHandler; }
    #endregion
}
