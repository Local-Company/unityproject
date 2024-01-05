using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror; 
using TMPro;

public class HostAndJoin : MonoBehaviour
{
    NetworkManager manager;
    public GameObject pauseMenuUI;
    public GameObject MenuStart;
    public TMP_InputField inputField;

    void Awake()
    {
        manager = GetComponent<NetworkManager>();
    }

    public void Host()
    {
        manager.StartHost();
    }

    public void Join()
    {
        string inputText = inputField.text;
        manager.StartClient();
        manager.networkAddress = inputText;
    }

    public void Quit()
    {
        Camera.main.transform.SetParent(null);
        Camera.main.transform.position = new Vector3(100,100,100);
        GameObject.FindGameObjectWithTag("Event").GetComponent<PauseMenu>().isPaused = false;
        pauseMenuUI.SetActive(false);
        MenuStart.SetActive(true);
        if (NetworkServer.active && NetworkClient.isConnected)
        {
            manager.StopHost();
            manager.StopClient();
        }
        else if (NetworkClient.isConnected)
        {
            manager.StopClient();
        }
        else if (NetworkServer.active)
        {
            manager.StopServer();
        }
    }
}
