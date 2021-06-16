using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsPacketHandler : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private CharacterCode ownerCode = CharacterCode.None;

    [Header("Components Reference")]
    [SerializeField] private GameObject sweetPrefab = null;
    [SerializeField] internal List<GameObject> sweetObjs = new List<GameObject>();
    [SerializeField] private SweetsPacketManager sweetsPacketManager = null;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();
    #endregion

    #region Public Core Functions
    public void SpawnSweets(int amount = 0)
    {
        for (int i = 0; i < amount; i++)
        {
            int index = Random.Range(0, sweetsPacketManager.spawnPoints.Count);
            sweetObjs.Add(Instantiate(sweetPrefab, sweetsPacketManager.spawnPoints[index].position, Quaternion.identity));
            spawnPoints.Add(sweetsPacketManager.spawnPoints[index]);
            sweetsPacketManager.spawnPoints.RemoveAt(index);
            sweetObjs[sweetObjs.Count - 1].GetComponent<SweetsHandler>().SweetCode = ownerCode;
            sweetObjs[sweetObjs.Count - 1].GetComponent<SweetsHandler>().LocationTransform = spawnPoints[spawnPoints.Count - 1];
            sweetObjs[sweetObjs.Count - 1].transform.parent = this.transform;
        }
    }

    public void UpdateSweetPacket()
    {
        sweetObjs.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            sweetObjs.Add(transform.GetChild(i).gameObject);
        }
    }

    public void EnableSweetsMeshRenderer()
    {
        for (int i = 0; i < sweetObjs.Count; i++)
        {
            print("Working");
            sweetObjs[i].GetComponent<SweetsHandler>().cookieMeshRenderer.enabled = true;
        }
    }
    #endregion

    #region Getter And Setter
    public CharacterCode GetCharacterCode { get => ownerCode; }
    #endregion
}
