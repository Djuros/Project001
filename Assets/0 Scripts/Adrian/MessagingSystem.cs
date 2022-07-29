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
    public PlayerSpawner ps;
    public List<string> playerNames = new List<string>();
    public List<int> playerscore = new List<int>();

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
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

        if (Input.GetKeyUp(KeyCode.Tab))
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
            for (int i = 0; i < ps.players.Length; i++)
            {
                if (_id == ps.players[i].GetComponent<MyMPRef>().my_id)
                {
                    print(_id + " Taking Damage " + _value);

                   // ps.players[i].GetComponent<MyMPRef>().Take_Damage((float)_value);
                }
            }
        }
    }

    [PunRPC]
    void SendScoreboardData(string _playerName, int _points)
    {
        playerNames.Add(_playerName);
        playerscore.Add(_points);
    }
    public void I_Died(int _id) {
        photonView.RPC("Death", RpcTarget.All, _id);
    }
    [PunRPC]
    void Death(int _id)
    {
        for (int i = 0; i < ps.players.Length; i++)
        {
            if (_id == ps.players[i].pv.ViewID && !ps.players[i].pv.IsMine)
            {
                ps.players[i].Death();
            }
        }
    }

    public void I_Fire(int _id, int _fireRate)
    {
        photonView.RPC("Fire", RpcTarget.All, _id, _fireRate);
    }
    [PunRPC]
    void Fire(int _id, int _fireRate)
    {
        for (int i = 0; i < ps.players.Length; i++)
        {
            if (_id == ps.players[i].pv.ViewID && !ps.players[i].pv.IsMine)
            {
                ps.players[i].Fire_Animation_MP(_fireRate);
                break;
            }
        }
    }
    public void I_Jump(int _id)
    {
        photonView.RPC("Jump", RpcTarget.All, _id);
    }
    [PunRPC]
    void Jump(int _id)
    {
        for (int i = 0; i < ps.players.Length; i++)
        {
            if (_id == ps.players[i].pv.ViewID && !ps.players[i].pv.IsMine)
            {
                ps.players[i].Jump_Mp();
                break;
            }
        }
    }
}
