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
        sweetTransform.GetComponent<SweetsHandler>().StackSweet(stackStartTransform, stackingPosition);
        stackingPosition = new Vector3(0, stackingPosition.y + sweetSizeYOffset, 0);
        sweetTransform.localRotation = Quaternion.identity;

        if (C_SweetsPacketHandler)
        {
            C_SweetsPacketHandler.sweetObjs.Remove(sweetTransform.gameObject);
            C_SweetsPacketHandler.UpdateSweetPacket();
        }
    }

    public void ReleaseSweet()
    {
        if (sweetStack.Count > 0)
        {
            sweetStack[sweetStack.Count - 1].transform.position = sweetStack[sweetStack.Count - 1].GetComponent<SweetsHandler>().LocationTransform.position;
            sweetStack[sweetStack.Count - 1].transform.parent = C_SweetsPacketHandler.transform;
            sweetStack[sweetStack.Count - 1].transform.rotation = Quaternion.identity;
            C_SweetsPacketHandler.sweetObjs.Add(sweetStack[sweetStack.Count - 1].transform.gameObject);
            sweetStack[sweetStack.Count - 1].GetComponent<BoxCollider>().enabled = true;
            sweetStack.RemoveAt(sweetStack.Count - 1);
            stackingPosition.y -= sweetSizeYOffset;
        }
    }

    public void EnablePhysics()
    {
        foreach (Transform t in sweetStack)
        {
            t.GetComponent<Rigidbody>().isKinematic = false;            
            t.GetComponent<BoxCollider>().isTrigger = false;
            t.GetComponent<BoxCollider>().enabled = true;
            t.parent = null;
            stackingPosition.y -= sweetSizeYOffset;
            t.GetComponent<SweetsHandler>().SweetCode = CharacterCode.None;
            t.GetComponent<SweetsHandler>().GetCookieMeshRenderer.material.color = new Color(.7f, .7f, .7f, 1); 
        }
        sweetStack.Clear();
    }

    public void EatStack(int pointsPerStack, float eatSpeed)
    {
        StartCoroutine(RemoveStack(pointsPerStack, eatSpeed));
    }

    IEnumerator RemoveStack(int points, float speed)
    {
        int x = sweetStack.Count;
        while(sweetStack.Count != 0) {
            GameManager.Instance.AddScore(points,sweetStack[sweetStack.Count-1].position);
            Destroy(sweetStack[sweetStack.Count - 1].gameObject);
            sweetStack.RemoveAt(sweetStack.Count - 1);
            yield return new WaitForSeconds(speed);
        }
        GameManager.Instance.ShowWinUI();
    }
    #endregion
}
