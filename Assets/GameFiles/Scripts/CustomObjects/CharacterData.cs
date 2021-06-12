using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "CustomObjects/CharacterDataObj")]
public class CharacterData : ScriptableObject
{//
    #region Properties
    [Header("Attributes")]
    [SerializeField] private CharacterCode characterCode = CharacterCode.None;
    [SerializeField] private Color colorCode = Color.white;
    #endregion

    #region Getter And Setter
    public CharacterCode GetCharacterCode { get => characterCode; }

    public Color GetColorCode { get => colorCode; }
    #endregion
}
