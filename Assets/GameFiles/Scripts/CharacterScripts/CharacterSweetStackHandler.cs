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

    internal List<Transform> sweetStack = new List<Transform>();
    private Vector3 stackingPosition = Vector3.zero;
    #endregion

    #region MonoBehaviour Functions
    private void Start()
    {
        stackingPosition = new Vector3(0, sweetSizeYOffset, 0);
    }
    #endregion

    #region Getter And Setter
    public int GetSweetStackSize { get => sweetStack.Count; }

    public SweetsPacketHandler C_SweetsPacketHandler { get; set; }
    #endregion

    #region Public Core Functions
    public void StackSweet(Transform sweetTransform)
    {
        sweetStack.Add(sweetTransform);
        sweetTransform.parent = stackStartTransform;
        sweetTransform.localPosition = stackingPosition;
        stackingPosition = new Vector3(0, stackingPosition.y + sweetSizeYOffset, 0);        
        sweetTransform.localRotation = Quaternion.identity;
    }

    public void ReleaseSweet()
    {
        if (sweetStack.Count > 0)
        {
            sweetStack[sweetStack.Count - 1].transform.position = sweetStack[sweetStack.Count - 1].GetComponent<SweetsHandler>().LocationTransform.position;
            sweetStack[sweetStack.Count - 1].transform.parent = C_SweetsPacketHandler.transform;
            sweetStack[sweetStack.Count - 1].GetComponent<BoxCollider>().enabled = true;
            //Destroy(sweetStack[sweetStack.Count - 1].gameObject);
            sweetStack.RemoveAt(sweetStack.Count - 1);  
        }
    }
    #endregion
}
