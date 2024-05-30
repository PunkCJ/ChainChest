using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : MonoBehaviour
{
    public GameObject winImg, loseImg;
    public TMP_Text msgTxt;

    public string loseMsg = "You lost all your winnings";
    public string winMsg = "You just won ";

    public void ShowMessage(bool isLose, int winnings = 0)
    {
        gameObject.SetActive(true);

        if(isLose)
        {
            msgTxt.text = loseMsg;

            winImg.SetActive(false);
            loseImg.SetActive(true);
        }
        else
        {
            msgTxt.text = winMsg + winnings;

            winImg.SetActive(true);
            loseImg.SetActive(false);
        }
    }
}
