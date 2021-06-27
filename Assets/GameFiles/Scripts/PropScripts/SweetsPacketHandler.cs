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
            switch (ownerCode)
            {
                case CharacterCode.Player:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().player, sweetsPacketManager.spawnPoints[index].position, Quaternion.Euler(0,90,0)));

                    break;
                case CharacterCode.Enemy_1:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().enemy1, sweetsPacketManager.spawnPoints[index].position, Quaternion.Euler(0, 90, 0)));

                    break;

                case CharacterCode.Enemy_2:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().enemy2, sweetsPacketManager.spawnPoints[index].position, Quaternion.Euler(0, 90, 0)));

                    break;

                case CharacterCode.Enemy_3:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().enemy3, sweetsPacketManager.spawnPoints[index].position, Quaternion.Euler(0, 90, 0)));

                    break;

                case CharacterCode.Enemy_4:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().enemy4, sweetsPacketManager.spawnPoints[index].position, Quaternion.Euler(0, 90, 0)));

                    break;
                case CharacterCode.Enemy_5:
                    sweetObjs.Add(Instantiate(SweetsManager.Instance.GetSweetType().enemy5, sweetsPacketManager.spawnPoints[index].position, Quaternion.identity));

                    break;
            }
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
            sweetObjs[i].GetComponent<SweetsHandler>().cookieMeshRenderer.enabled = true;
        }
    }

   
    #endregion

    #region Getter And Setter
    public CharacterCode GetCharacterCode { get => ownerCode; }
    #endregion
}
