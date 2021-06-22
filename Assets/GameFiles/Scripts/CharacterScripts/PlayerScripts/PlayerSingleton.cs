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

    [Header("Shift Speed")]
    [SerializeField] private float shiftSpeed;
    private bool isShifting;    

    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Destroy(gameObject);
        }
        Instance = this;
    }

    private void Update()
    {
        if (isShifting)
        {
            characterSweetStackHandler.transform.localPosition = Vector3.Lerp(characterSweetStackHandler.transform.localPosition, Vector3.zero,shiftSpeed);            
        }
    }
    #endregion

    #region Public functions

    public void Lose()
    {
        GetComponent<PlayerMovementHandler>().enabled = false;
        characterAnimationHandler.SwitchCharacterAnimation(CharacterAnimationState.Defeat);
    }
    public void CompleteShift()
    {
        isShifting = false;
        GameManager.Instance.EatRemainingStack();
    }
    public void ShiftCam()
    {
        GameManager.Instance.SwitchCam();
    }
    public void ShiftStack(Transform t, float time)
    {
        characterSweetStackHandler.transform.SetParent(t);
        shiftSpeed = time;
        isShifting = true;
        Invoke("CompleteShift", 3.5f);
        Invoke("ShiftCam", 1f);
    }

    #endregion

    #region Getter And Setter
    public CharacterAnimationHandler GetCharacterAnimationHandler { get => characterAnimationHandler; }
    #endregion
}
