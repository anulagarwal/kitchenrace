using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SweetsPacketManager : MonoBehaviour
{
    #region Properties
    [Header("Attributes")]
    [SerializeField] private int sweetSpawnAmount = 0;

    [Header("Components Reference")]
    [SerializeField] internal List<SweetsPacketHandler> sweetsPacketHandlers = new List<SweetsPacketHandler>();
    [SerializeField] internal List<Transform> spawnPoints = new List<Transform>();
    #endregion

    #region MonoBehaviour Functions
    private void Awake()
    {
       
    }
    #endregion

    public void SpawnSweets()
    {
        foreach (SweetsPacketHandler sph in sweetsPacketHandlers)
        {
            sph.SpawnSweets(sweetSpawnAmount);
        }
    }
}
