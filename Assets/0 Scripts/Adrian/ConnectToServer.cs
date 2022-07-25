using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject enterAName;
    public InputField usernameInput;
    public Text buttonText;

    public void OnClickConnect()
    {
        if(usernameInput.text.Length >= 1) // Check if username input is not null
        {
            PhotonNetwork.NickName = usernameInput.text; // Set inputed string as Nickname
            enterAName.SetActive(false);
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings(); // Connect to a server
        }
        else { enterAName.SetActive(true); }
    }

    public override void OnConnectedToMaster() // If connected open Lobby scene
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
