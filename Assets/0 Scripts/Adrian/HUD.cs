using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class HUD : MonoBehaviourPunCallbacks 
{
    public static HUD ins;
    public Image fill_bar;
    public GameObject LeaveButtonGO, GameEndedGO, HealthGO;
    public GameObject[] playerNames, playerScores, playerReadyNames, playerStatus;
    private void Awake()
    {
        ins = this;
    }

    public void OnGameEnded()
    {
        HealthGO.SetActive(false);
        LeaveButtonGO.SetActive(false);
        GameEndedGO.SetActive(true);
    }

    public void LeaveButton()
    {
        PhotonNetwork.LeaveRoom();
        Invoke("Leave", 1);
    }
    void Leave()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}
