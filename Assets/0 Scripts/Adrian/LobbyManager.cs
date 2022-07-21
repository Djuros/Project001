using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class LobbyManager : Photon.MonoBehaviour
{
    public InputField roomInputField, joinRoomInput;
    public GameObject lobbyPanel, roomPanel;
    public Text roomName;

    public RoomItem roomItemPrefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contenctObject;
    public RoomInfo[] rooms;

    private void Start()
    {
        PhotonNetwork.playerName = "Adrian";
        PhotonNetwork.ConnectUsingSettings(Application.version);
      
    }


    public void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
        InvokeRepeating("RefreshRooms", 2, 2);
    }


    public virtual void OnJoinedLobby()
    {
        Debug.Log("OnJoinedLobby(). This client is connected and does get a room-list, which gets stored as PhotonNetwork.GetRoomList(). This script now calls: PhotonNetwork.JoinRandomRoom();");
    }
    void RefreshRooms()
    {
        if (rooms == null)
        {
            print("ROOMS IS NULL");
            return; }
        print("ROOMS " + rooms.Length);
        for (int i = 0; i < rooms.Length; i++)
        {
            print("Room Name "+rooms[i]);
        }

    }
    void GetRooms()
    {
        rooms = PhotonNetwork.GetRoomList();


    }
    public void OnClickCreate()
    {
        if(roomInputField.text.Length >= 1)
        {
            PhotonNetwork.CreateRoom(roomInputField.text);
            Invoke("GetRooms", 2);
        }
    }

    public void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        roomPanel.SetActive(true);
        roomName.text = "Room name: " + PhotonNetwork.room.name;
        PhotonNetwork.room.visible = true;
    }

    public void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room List Updated");
        UpdateRoomList(roomList);
    }

    public void UpdateRoomList(List<RoomInfo> list)
    {
        print("UPDATE ROOM");
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

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom(joinRoomInput.text);
    }
}
