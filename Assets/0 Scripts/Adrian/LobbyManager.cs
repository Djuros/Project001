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
        PhotonNetwork.JoinLobby();
    }

    private void Update()
    {
        if (PhotonNetwork.IsMasterClient && PhotonNetwork.CurrentRoom.PlayerCount >= 2) 
        {
            startButton.SetActive(true);
        }
        else
        {
            startButton.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.L)) { OnClickPlayButton(); }
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
        PhotonNetwork.LoadLevel("GameScene");
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
}
