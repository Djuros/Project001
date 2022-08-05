using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public GameObject networkPopup, chooseNamePanel;
    public GameObject enterAName;
    public InputField usernameInput;
    public Dropdown regionDropdown;
    public Text buttonText;
    private string serverRegion = "eu";
    bool thereIsConnection = false;

    private void Start()
    {
        StartCoroutine(TestConnection());
    }
    public void OnClickConnect()
    {
        if(!thereIsConnection)
        {
            networkPopup.SetActive(true);
            chooseNamePanel.SetActive(false);
            return;
        }
        if(usernameInput.text.Length >= 1)
        {
            PhotonNetwork.NickName = usernameInput.text; 
            enterAName.SetActive(false);
            buttonText.text = "Connecting...";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.ConnectUsingSettings(); 
        }
        else { enterAName.SetActive(true); }
    }

    public override void OnConnectedToMaster() 
    {
        SceneManager.LoadScene("LobbyScene");
    }

   
    IEnumerator TestConnection()
    {
        float timeTaken = 0.0F;
        float maxTime = 2.0F;

        while (true)
        {
            Ping testPing = new Ping("8.8.8.8");

            timeTaken = 0.0F;

            while (!testPing.isDone)
            {
                timeTaken += Time.deltaTime;

                if (timeTaken > maxTime)
                {
                    thereIsConnection = false;
                    break;
                }

                yield return null;
            }
            if (timeTaken <= maxTime) thereIsConnection = true;
            yield return null;
        }
    }
}
