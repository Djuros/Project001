using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    public MyMPRef[] players;
    public List<Transform> spawnPoints = new List<Transform>();
    public Cinemachine.CinemachineVirtualCamera vc;
    private void Start()
    {
        MessagingSystem.ins.me = PhotonNetwork.Instantiate("Player", transform.position, Quaternion.identity).GetComponent<MyMPRef>();
        Setup_Local_Systems();
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
    void Setup_Local_Systems() {
        players = FindObjectsOfType<MyMPRef>();
        if (players.Count() < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            Invoke("Setup_Local_Systems", 0.2f);
            return;
        }
        for (int i = 0; i < players.Length; i++)
        {
            players[i].name_label.text = players[i].pv.Owner.NickName;
        }
        vc.Follow = MessagingSystem.ins.me.transform;
        vc.LookAt = MessagingSystem.ins.me.transform;
        vc.enabled = true;
        MessagingSystem.ins.me.ch.enabled = true;
        MessagingSystem.ins.me.pc.enabled = true;
        BulletPool.ins.muzzle = MessagingSystem.ins.me.gameObject.GetComponent<PlayerController>().muzzle;
        BulletPool.ins.Spawn_Bullets();
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
