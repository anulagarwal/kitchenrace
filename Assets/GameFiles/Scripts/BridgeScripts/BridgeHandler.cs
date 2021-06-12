using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BridgeHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private List<MeshRenderer> bridgeMRPack = new List<MeshRenderer>();
    #endregion

    #region MonoBehaviour Functions    
    #endregion

    #region Public Core Functions
    public void EnableStairMR(Color color, GameObject stairObj)
    {
        stairObj.GetComponent<MeshRenderer>().material.color = color;
    }
    #endregion
}
