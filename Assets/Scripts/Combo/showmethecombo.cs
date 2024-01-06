using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class showmethecombo : MonoBehaviour
{
    public GameObject C1;
    public GameObject C2;
    public GameObject C3;

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            whatcombo();
        }
    }

    public void whatcombo()
    {
        int ac = GameObject.FindGameObjectWithTag("Event").GetComponent<comboANDscore>().actualCombo;
        C1.SetActive(false);
        C2.SetActive(false);
        C3.SetActive(false);

        if(ac == 1)
        {
            C1.SetActive(true);
        }
        else if (ac == 2)
        {
            C2.SetActive(true);
        }
        else if (ac ==  3)
        {
            C3.SetActive(true);
        }
    }
}
