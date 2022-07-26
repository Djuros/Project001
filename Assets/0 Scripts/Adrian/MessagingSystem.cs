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
    public MyMPRef[] players;
    public List<string> playerNames = new List<string>();
    public List<int> playerScore = new List<int>();
    public List<bool> playersToPlayAgain = new List<bool>();
    public Button playAgainButton;
    public bool gameEnded = false;
    public Text startButtonStatus, PlayersTextStatus;

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
            SendDataOnGameFinish();
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            ShowScoreboard();
        }

        if (!gameEnded)
        {
            return;
        }

        if(playersToPlayAgain.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            PlayersTextStatus.text = "All of the players are ready to play!";
        }
        else
        {
            PlayersTextStatus.text = playersToPlayAgain.Count.ToString() + " out of " + PhotonNetwork.CurrentRoom.PlayerCount.ToString() + " players are ready...";
        }

        if (PhotonNetwork.IsMasterClient && gameEnded && playersToPlayAgain.Count >= PhotonNetwork.CurrentRoom.PlayerCount)
        {
            playAgainButton.interactable = true;
            startButtonStatus.text = "";
        }
        else if (playersToPlayAgain.Count < PhotonNetwork.CurrentRoom.PlayerCount)
        {
            startButtonStatus.text = "Not all players are ready to start the game!";
        }
        else if (!PhotonNetwork.IsMasterClient)
        {
            startButtonStatus.text = "Only Master Client can start the game!";
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
        photonView.RPC("PlayAgain", RpcTarget.All, "PlayAgain");
    }

    [PunRPC]
    void PlayAgain(string _action)
    {
        if(_action == "PlayAgain")
        {
            playersToPlayAgain.Add(true);
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
