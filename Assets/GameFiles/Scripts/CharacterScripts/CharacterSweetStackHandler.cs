using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSweetStackHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private float sweetSizeYOffset = 0f;

    [Header("Components Reference")]
    [SerializeField] private Transform stackStartTransform = null;

    private Vector3 stackingPosition = Vector3.zero;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        stackingPosition = new Vector3(0, sweetSizeYOffset, 0);
    }
    #endregion

    #region Public Core Functions
    public void StackSweet(Transform sweetTransform)
    {
        sweetTransform.parent = stackStartTransform;
        sweetTransform.localPosition = stackingPosition;
        stackingPosition = new Vector3(0, stackingPosition.y + sweetSizeYOffset, 0);        
        sweetTransform.localRotation = Quaternion.identity;
    }
    #endregion
}
