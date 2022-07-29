using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MessagingSystem : MonoBehaviourPunCallbacks
{
    public static MessagingSystem ins;
    public static HUD hudIns;
    public PhotonView photonView;
    public MyMPRef me;
    public PlayerSpawner ps;
    public List<string> playerNames = new List<string>();
    public List<int> playerScore = new List<int>();
    public Button playAgainButton;
    public bool gameEnded = false;
    public Text startButtonStatus, PlayersTextStatus;
    public int readyCounter = 0;

    private void Awake()
    {
        ins = this;
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
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
            SendDataOnGameFinish();
        }

        if (Input.GetKeyUp(KeyCode.Tab))
        {
            ShowScoreboard();
        }

        if (!gameEnded)
        {
            return;
        }

        UpdatePlayers();

        SetPlayerStatuses();
    }

    void SetPlayerStatuses()
    {
        readyCounter = 0;
        for (int i = 0; i < HUD.ins.playerStatus.Length; i++)
        {
            HUD.ins.playerReadyNames[i].SetActive(false);
            HUD.ins.playerStatus[i].SetActive(false);
        }
        for (int i = 0; i < players.Length; i++)
        {
            HUD.ins.playerReadyNames[i].SetActive(true);
            HUD.ins.playerStatus[i].SetActive(true);
            HUD.ins.playerReadyNames[i].GetComponent<Text>().text = players[i].name.ToString();
            if (players[i].playAgain)
            {
                HUD.ins.playerStatus[i].GetComponent<Text>().text = "READY";
                HUD.ins.playerStatus[i].GetComponent<Text>().color = Color.green;
                readyCounter++;
            }
            else
            {
                HUD.ins.playerStatus[i].GetComponent<Text>().color = Color.yellow;
                HUD.ins.playerStatus[i].GetComponent<Text>().text = "WAITING";
            }
        }

        if (readyCounter >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PlayersTextStatus.text = "All of the players are ready to play!";
        }
        else
        {
            PlayersTextStatus.text = readyCounter.ToString() + " out of " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " players are ready...";
        }

        if (PhotonNetwork.IsMasterClient && gameEnded && readyCounter >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            playAgainButton.interactable = true;
            startButtonStatus.text = "";
        }
        else if (readyCounter < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            startButtonStatus.text = "Not all players are ready to start the game!";
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponentInParent<PhotonView>().Owner.IsMasterClient)
                {
                    startButtonStatus.text = "Only player: " + players[i].name.ToString() + " can start the game!";
                }
            }
        }
    }

    public void SendDataOnGameFinish()
    {
        int num = Random.Range(2, 20);
        photonView.RPC("SendScoreboardData", RpcTarget.All, me.name, num);
    }

    public void ShowScoreboard()
    {
        int largest = 0, pointer = 0, counter = 0;
        string name = "";
        for (int i = 0; i < playerScore.Count;)
        {
            for (int j = 0; j <= playerScore.Count - 1; j++)
            {
                if (playerScore[j] > largest)
                {
                    largest = playerScore[j];
                    name = playerNames[j];
                    pointer = j;
                }
            }

            playerScore.RemoveAt(pointer);
            playerNames.RemoveAt(pointer);
            HUD.ins.playerNames[counter].SetActive(true);
            HUD.ins.playerScores[counter].SetActive(true);
            HUD.ins.playerNames[counter].GetComponent<Text>().text = name;
            HUD.ins.playerScores[counter].GetComponent<Text>().text = largest.ToString();
            counter++;
            largest = 0;
            name = "";
        }
        gameEnded = true;
        HUD.ins.OnGameEnded();
    }

    public void StartButton()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("RestartGame", RpcTarget.All, "Restart");
        }
    }

    public void PlayAgainButton()
    {
        me.playAgain = true;
        photonView.RPC("PlayAgain", RpcTarget.All, "PlayAgain", me.my_id);
    }

    [PunRPC]
    void PlayAgain(string _action, int _playerID)
    {
        if(_action == "PlayAgain")
        {
            for(int i = 0; i < players.Length; i++)
            {
                if (players[i].my_id == _playerID)
                {
                    players[i].playAgain = true;
                }
            }
        }
    }

    [PunRPC]
    void RestartGame(string _action)
    {
        if(_action == "Restart")
        {
            PhotonNetwork.LoadLevel(SceneManager.GetActiveScene().name);
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

    public override void OnLeftRoom()
    {
        print("Bla� je zani?");
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
