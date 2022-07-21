using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectToServer : MonoBehaviour
{
    public InputField usernameInput;
    public Text buttonText;

    public void OnClickConnect()
    {
        if(usernameInput.text.Length >= 1)
        {
            PhotonNetwork.playerName = usernameInput.text;
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings(Application.version);
        }
    }

    public void OnConnectedToMaster()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
