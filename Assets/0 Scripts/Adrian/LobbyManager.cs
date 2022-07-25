using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using System;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public InputField roomInputField, joinRoomInput;
    public GameObject lobbyPanel, roomPanel, startButton;
    public Text roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contenctObject;
    public float timeBetweenUpdates = 1.5f;
    float nextUpdateTime;

    private void Start()
    {
        PhotonNetwork.JoinLobby(); // Make player join lobby
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 1) // Show play button only to room master
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);

        }
    }

    public void OnClickCreate() // On click button check the value if true create new room
    {
        if(roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text);
        }
    }

    public override void OnJoinedRoom() // On joining room set room name and display new panel
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Welcome to room: " + PhotonNetwork.CurrentRoom.Name;

        // send to everyone your id
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) // Update active room list
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

    public void OnClickLeaveRoom() // Leave Room
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
}
