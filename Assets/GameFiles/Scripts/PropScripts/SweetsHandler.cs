using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] internal MeshRenderer cookieMeshRenderer = null;
    #endregion

    #region MonoBehaviour Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }
    #endregion

    #region Getter And Setter
    public CharacterCode SweetCode { get; set; }
    
    public Transform LocationTransform { get; set; }

    public MeshRenderer GetCookieMeshRenderer { get => cookieMeshRenderer; }
    #endregion
}
