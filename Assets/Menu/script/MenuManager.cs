using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MenuStart;
    public GameObject MenuConnect;

    public void GoToConnect()
    {
        MenuStart.SetActive(false);
        MenuConnect.SetActive(true);
    }

    public void GoToGame()
    {
        MenuConnect.SetActive(false);
    }

    public void Quit()
    {
    }
}
