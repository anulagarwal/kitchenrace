using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private CharacterCode ownerCode = CharacterCode.None;

    [Header("Components Reference")]
    [SerializeField] internal List<GameObject> sweetObjs = new List<GameObject>();
    #endregion

    #region Getter And Setter
    public CharacterCode GetCharacterCode { get => ownerCode; }
    #endregion
}
