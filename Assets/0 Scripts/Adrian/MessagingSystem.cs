using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MessagingSystem : MonoBehaviourPunCallbacks
{
    public static MessagingSystem ins;
    public static HUD hudIns;
    public PhotonView photonView;
    public MyMPRef me;
    public MyMPRef[] players;
    public List<string> playerNames = new List<string>();
    public List<int> playerScore = new List<int>();

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        UpdatePlayers();
    }
    void UpdatePlayers()
    {
        if (PhotonNetwork.IsMasterClient) {
            players = FindObjectsOfType<MyMPRef>();
        }
     
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.D))
        {
            int pID = 0;
            var ray = new Ray(me.transform.position, me.transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                if(hit.transform.gameObject.CompareTag("Player"))
                {
                    pID = hit.transform.gameObject.GetPhotonView().ViewID;
                }
            }
            photonView.RPC("ChatMessage", RpcTarget.All, "TakeDamange", pID, 10);
        }

        if (Input.GetKeyUp(KeyCode.G))
        {
            int num = Random.Range(0, 10);
            photonView.RPC("SendScoreboardData", RpcTarget.All, me.name, num);
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            for(int i = 0; i < playerNames.Count; i++)
            {
                
            }
            HUD.ins.OnGameEnded();
        }
    }

    [PunRPC]
    void ChatMessage(string _action, int _id, int _value)
    {
        if (_action == "TakeDamange")
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (_id == players[i].GetComponent<MyMPRef>().my_id)
                {
                    print(_id + " Taking Damage " + _value);

                    players[i].GetComponent<MyMPRef>().Take_Damage((float)_value);
                }
            }
        }
    }

    [PunRPC]
    void SendScoreboardData(string _playerName, int _points)
    {
        playerNames.Add(_playerName);
        playerScore.Add(_points);
    }
}
