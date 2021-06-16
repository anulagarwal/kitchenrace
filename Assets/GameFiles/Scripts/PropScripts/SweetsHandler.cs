using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] internal MeshRenderer cookieMeshRenderer = null;
    #endregion

    #region Getter And Setter
    public CharacterCode SweetCode { get; set; }
    
    public Transform LocationTransform { get; set; }
    #endregion
}
