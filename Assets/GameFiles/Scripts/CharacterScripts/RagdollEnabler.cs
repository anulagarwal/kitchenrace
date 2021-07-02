using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollEnabler : MonoBehaviour
{
    #region Properties
    [Header("Components Reference")]
    [SerializeField] private CharacterRagdoll characterRagdoll = null;
    #endregion

    #region MonoBehaviour Functions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Edge")
        {
            characterRagdoll.EnableRagdoll(true);
            characterRagdoll.RagdollImpact((other.gameObject.transform.position - characterRagdoll.transform.position).normalized);

            Invoke("DisableSelf", 3f);
        }
    }
    #endregion

    #region Invoke Functions
    private void DisableSelf()
    {
        this.gameObject.SetActive(false);
        CancelInvoke();
    }
    #endregion
}
