using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class MessagingSystem : MonoBehaviourPunCallbacks
{
    public static MessagingSystem ins;
    public PhotonView photonView;
    public MyMPRef me;
    public MyMPRef[] players;
    public float cnt;
    
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
        players = FindObjectsOfType<MyMPRef>();
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
                print("Ray");
                if(hit.transform.gameObject.CompareTag("Player"))
                {
                    print("Player");
                    pID = hit.transform.gameObject.GetPhotonView().ViewID;
                }
                print(hit.transform.gameObject);
            }
            photonView.RPC("ChatMessage", RpcTarget.All, "TakeDamange", pID, 10);
        }
    }

    [PunRPC]
    void ChatMessage(string _action, int _id, int _value)
    {
        print("Message");
        if (_action == "TakeDamange")
        {
            print("Damage");
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
}
