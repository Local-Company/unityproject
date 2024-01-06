using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class showmethescore : MonoBehaviour
{
    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            GameObject scoreTextObject = GameObject.FindGameObjectWithTag("Score");
            scoreTextObject.SetActive(true);
            int score = GameObject.FindGameObjectWithTag("Event").GetComponent<comboANDscore>().GetScore();
            TextMeshProUGUI tmp = scoreTextObject.GetComponent<TextMeshProUGUI>();
            GameObject.FindGameObjectWithTag("Score");
            tmp.text = score.ToString();
        }
    }
}
