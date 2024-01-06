using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class waitandgo : MonoBehaviour
{
    public bool wait = false;

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            if(GameObject.FindGameObjectsWithTag("Player").Length <= 1)
            {
                wait = false;
                GameObject.FindGameObjectWithTag("Event").GetComponent<PauseMenu>().isPaused = false;
            }
            else
            {
                wait = false;
                GameObject.FindGameObjectWithTag("Event").GetComponent<PauseMenu>().isPaused = false;
            }
            iwait();
        }
    }

    public void iwait()
    {
        if (wait == true)
        {
            GameObject scoreTextObject = GameObject.FindGameObjectWithTag("Wait");
            TextMeshProUGUI tmp = scoreTextObject.GetComponent<TextMeshProUGUI>();
            tmp.text = "waiting for other players";
        }
        else
        {
            GameObject scoreTextObject = GameObject.FindGameObjectWithTag("Wait");
            TextMeshProUGUI tmp = scoreTextObject.GetComponent<TextMeshProUGUI>();
            tmp.text = "";
        }
    }
}
