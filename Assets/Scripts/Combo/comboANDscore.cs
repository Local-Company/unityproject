using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class comboANDscore : MonoBehaviour
{
    public int score = 0;
    public int actualCombo = 1;

    void Start()
    {
        score = 0;
    }

    public int GetScore()
    {
        return score;
    }

    public int GetCombo()
    {
        return actualCombo;
    }

    public void MoreCombo()
    {
        if (actualCombo >= 3)
        {
            actualCombo = 3;
        }
        else
        {
            actualCombo += 1;
        }
    }

    public void ResetCombo()
    {
        actualCombo = 1;
    }

    public void AddScore()
    {
        score = 1 * actualCombo;   
    }
    
    public void ResetScore()
    {
        score = 0;
    }

}
