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
    public Button startButton, playAgainButton, leaveButton;
    public bool gameEnded = false;
    public Text startButtonStatus, PlayersTextStatus;
    public int readyCounter = 0;
    public int flags_to_turn_in;
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
            ps.players = FindObjectsOfType<MyMPRef>();
        }
    }
    public void End_Game() {
        SendMeData();
        Invoke("ShowScoreboard", 0.6f);
        playAgainButton.interactable = true;
        leaveButton.interactable = true;
        photonView.RPC("End_The_Game", RpcTarget.All);
    }
    [PunRPC]
    public void End_The_Game()
    {
        SendMeData();
        Invoke("ShowScoreboard", 0.6f);
        playAgainButton.interactable = true;
        leaveButton.interactable = true;
    }
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.G))
        {
            SendDataOnGameFinish();
        }

        if (Input.GetKeyUp(KeyCode.Tab) && !gameEnded)
        {
            playAgainButton.interactable = false;
            leaveButton.interactable = false;
            playerNames.Clear();
            playerScore.Clear();
            SendMeData();
            Invoke("ShowScoreboard", 0.6f);
        }

        if (!gameEnded)
        {
            return;
        }
        playAgainButton.interactable = true;
        leaveButton.interactable = true;
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
        for (int i = 0; i < ps.players.Length; i++)
        {
            HUD.ins.playerReadyNames[i].SetActive(true);
            HUD.ins.playerStatus[i].SetActive(true);
            HUD.ins.playerReadyNames[i].GetComponent<Text>().text = ps.players[i].name.ToString();
            if (ps.players[i].playAgain)
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
            startButton.interactable = true;
            startButtonStatus.text = "";
        }
        else if (readyCounter < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            startButtonStatus.text = "Not all players are ready to start the game!";
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < ps.players.Length; i++)
            {
                if (ps.players[i].GetComponentInParent<PhotonView>().Owner.IsMasterClient)
                {
                    startButtonStatus.text = "Only player: " + ps.players[i].name.ToString() + " can start the game!";
                }
            }
        }
    }

    public void SendDataOnGameFinish()
    {
        photonView.RPC("SendScoreboardData", RpcTarget.All, me.name, me.score);
    }

    public void SendMeData()
    {
        photonView.RPC("SendMeData", RpcTarget.All, "Send");
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
            for(int i = 0; i < ps.players.Length; i++)
            {
                if (ps.players[i].my_id == _playerID)
                {
                    ps.players[i].playAgain = true;
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
    void SendScoreboardData(string _playerName, int _points)
    {
        playerNames.Add(_playerName);
        playerScore.Add(_points);
    }

    [PunRPC]
    void SendMeData(string _info)
    {
        if (_info == "Send")
        {
            photonView.RPC("SendScoreboardData", RpcTarget.All, me.name, me.score);
        }
    }

    public override void OnLeftRoom()
    {
        print("Blaz je zanic");
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
    public void I_Respawned(int _id)
    {
        photonView.RPC("Respawn", RpcTarget.All, _id);
    }
    [PunRPC]
    public void Respawn(int _id) {
        for (int i = 0; i < ps.players.Length; i++)
        {
            if (_id == ps.players[i].pv.ViewID && !ps.players[i].pv.IsMine)
            {
                ps.players[i].anim.Play("Idle",0);
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
    public void Flag_Collected() {
        photonView.RPC("On_Flag_Collected", RpcTarget.All);
    }
    [PunRPC]
    public void On_Flag_Collected()
    {
        flags_to_turn_in--;
        if (flags_to_turn_in == 0) { End_Game(); }
    }
}
