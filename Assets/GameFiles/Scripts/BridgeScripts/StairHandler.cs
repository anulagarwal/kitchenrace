using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private bool isLast = false;
    [SerializeField] private float changeRate = 0.02f;

    private bool isChangingColor;
    private float colorChangeRate;
    private Color originalColor;

    [Header("Components Reference")]
    [SerializeField] private MeshRenderer meshRenderer = null;
    [SerializeField] private GameObject blockerObj = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        OwnerCode = CharacterCode.None;
    }

    private void Update()
    {
        if (isChangingColor)
        {
            GetComponent<MeshRenderer>().material.color = Color.Lerp(GetComponent<MeshRenderer>().material.color, originalColor, changeRate);            
        }
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
        originalColor = GetComponent<MeshRenderer>().material.color;

        isChangingColor = true;
        GetComponent<MeshRenderer>().material.color = Color.white;
        Invoke("StopShift", 2f); 
    }
    void StopShift()
    {
        isChangingColor = false;
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
