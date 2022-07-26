using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    public MyMPRef[] players;
    public List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        MessagingSystem.ins.me = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity).GetComponent<MyMPRef>();
        if (PhotonNetwork.IsMasterClient)
        {
            Shufle_Spawn_Points();
            PositionDeleyed();
        }
    }
    void PositionDeleyed() {
        players = FindObjectsOfType<MyMPRef>();
        if (players.Count() < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Invoke("PositionDeleyed", 0.2f);
            return;
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].pv.RPC("Pos", players[i].pv.Owner, spawnPoints[i].position.x.ToString(), spawnPoints[i].position.y.ToString(), spawnPoints[i].position.z.ToString());
        }
    }

    void Shufle_Spawn_Points()
    {
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            Transform temp = spawnPoints[i];
            int randomIndex = Random.Range(i, spawnPoints.Count);
            spawnPoints[i] = spawnPoints[randomIndex];
            spawnPoints[randomIndex] = temp;
        }
    }
}
