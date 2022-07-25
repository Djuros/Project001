using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;

    private void Start()
    {
        MessagingSystem.ins.me = PhotonNetwork.Instantiate(playerPrefab.name, transform.position, Quaternion.identity).GetComponent<MyMPRef>();
    }
}
