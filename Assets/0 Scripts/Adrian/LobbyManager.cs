using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using System;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInputField, joinRoomInput;
    public GameObject lobbyPanel, roomPanel, startButton;
    public GameObject[] playersInRoomTextsGO;
    public Text roomName;
    public TextMeshProUGUI aloneInRoomText;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contenctObject;
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;
    private string masterClientName;

    private void Start()
    {
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (!PhotonNetwork.InRoom) {return;}
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].IsMasterClient)
            {
                masterClientName = PhotonNetwork.PlayerList[i].NickName;
            }
        }
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startButton.GetComponent<Button>().interactable = true;
            aloneInRoomText.text = "";
        }
        else if (!PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            startButton.GetComponent<Button>().interactable = false;
            aloneInRoomText.text = "Only player " + masterClientName +  " can start the game!";
        }
        else
        {
            startButton.GetComponent<Button>().interactable = false;
            aloneInRoomText.text = "You are alone in the room. Wait for someone to join!";
        }

        for (int i = 0; i < playersInRoomTextsGO.Length; i++)
        {
            playersInRoomTextsGO[i].SetActive(false);
        }
        
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            playersInRoomTextsGO[i].SetActive(true);
            playersInRoomTextsGO[i].GetComponent<Text>().text = PhotonNetwork.PlayerList[i].NickName;
        }
    }

    public void OnClickCreate() 
    {
        if(roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text);
        }
    }

    public override void OnJoinedRoom() 
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Welcome to room: " + PhotonNetwork.CurrentRoom.Name;
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) 
    {
        if(Time.time >= nextUpdateTime)
        {
            nextUpdateTime = Time.time + timeBetweenUpdates;
            UpdateRoomList(roomList);
        }
    }

    public void UpdateRoomList(List<RoomInfo> list)
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();

        foreach(RoomInfo room in list)
        {
            RoomItem newRoom = Instantiate(roomItemPrefab, contenctObject);
            newRoom.SetRoomName(room.Name);
            roomItemsList.Add(newRoom);
        }
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinSpecificRoom()
    {
        if(joinRoomInput.text.Length < 1) { return; }

        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }

    public void OnClickLeaveRoom() 
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        lobbyPanel.SetActive(true);
        roomPanel.SetActive(false);
    }

    public void OnClickPlayButton()
    {
        PhotonNetwork.LoadLevel("AdrianGameScene");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
