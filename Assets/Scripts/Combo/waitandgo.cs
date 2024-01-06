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
                GameObject scoreTextObject = GameObject.FindGameObjectWithTag("Wait");
                TextMeshProUGUI tmp = scoreTextObject.GetComponent<TextMeshProUGUI>();
                tmp.text = "waiting for other players";
                wait = true;
            }
            else
            {
                GameObject scoreTextObject = GameObject.FindGameObjectWithTag("Wait");
                TextMeshProUGUI tmp = scoreTextObject.GetComponent<TextMeshProUGUI>();
                tmp.text = "";
                wait = false;
            }
        }
    }

}
