using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private bool isLast = false;

    [Header("Components Reference")]
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private GameObject blockerObj = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        OwnerCode = CharacterCode.None;
    }
    #endregion

    #region Getter And Setter
    public CharacterCode OwnerCode { get; set; }
    #endregion

    #region Public Core Functions
    public void ChangeStairColor(Color c, CharacterCode code)
    {
        OwnerCode = code;
        meshRenderer.enabled = true;
        meshRenderer.material.color = c;
    }

    public void EnableBlocker(bool value)
    {
        if (!isLast)
        {
            blockerObj.SetActive(value);
        }
    }
    #endregion
}
