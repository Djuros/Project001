using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    LobbyManager manager;


    private void Start()
    {
        manager = FindObjectOfType<LobbyManager>(); // Find Lobby Manager 
    }

    public void SetRoomName(string _roomName) // Set button's text to room name
    {
        roomName.text = _roomName;
    }

    public void OnClickItem() // On button click call "JoinRoom" from Lobby Manager
    {
        manager.JoinRoom(roomName.text);
    }
}
