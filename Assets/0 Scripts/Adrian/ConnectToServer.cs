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
        if(usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text;
            enterAName.SetActive(false);
            buttonText.text = "Connecting...";
            PhotonNetwork.ConnectUsingSettings();
        }
        else { enterAName.SetActive(true); }
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
