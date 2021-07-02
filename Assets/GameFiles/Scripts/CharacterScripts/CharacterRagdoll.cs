using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRagdoll : MonoBehaviour
{//
    #region Properties
    [Header("Attributes")]
    [SerializeField] private float impactForce = 0f;

    [Header("Components Reference")]
    [SerializeField] private List<Rigidbody> rbs = new List<Rigidbody>();
    [SerializeField] private List<Collider> colliders = new List<Collider>();
    [SerializeField] private Animator animator = null;
    [SerializeField] private Rigidbody chestRB = null;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        EnableRagdoll(false);
    }
    #endregion

    #region Public Core Functions
    public void EnableRagdoll(bool value)
    {
        animator.enabled = !value;
        for (int i = 0; i < rbs.Count; i++)
        {
            rbs[i].isKinematic = !value;
            colliders[i].enabled = value;
        }
    }

    public void RagdollImpact(Vector3 direction)
    {
        chestRB.AddForce(direction * impactForce, ForceMode.Impulse);
    }
    #endregion
}
