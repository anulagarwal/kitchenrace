using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsHandler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] internal MeshRenderer cookieMeshRenderer = null;

    [Header("Lerp properties")]
    private Vector3 targetPos;
    private bool isStacking;
    private float stackSpeed = 0.2f;
    #endregion

    #region MonoBehaviour Functions
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            gameObject.layer = 0;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<BoxCollider>().isTrigger = true;
        }
    }

    private void Update()
    {
        if (isStacking)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, targetPos, stackSpeed);
            if(Vector3.Distance(transform.localPosition, targetPos)<= 0.1f)
            {
                isStacking = false;
                transform.localPosition = targetPos;
            }
        }
    }
    #endregion

    #region Public Functions

    public void StackSweet(Transform target, Vector3 stackingPos)
    {
        transform.parent = target;   
        targetPos = stackingPos;
        isStacking = true;      
    }

    #endregion
    #region Getter And Setter
    public CharacterCode SweetCode { get; set; }
    
    public Transform LocationTransform { get; set; }

    public MeshRenderer GetCookieMeshRenderer { get => cookieMeshRenderer; }
    #endregion
}
