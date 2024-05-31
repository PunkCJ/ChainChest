using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanel : MonoBehaviour
{
    #region Singleton

    public static ScorePanel instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        //
        gameObject.SetActive(false);
    }

    #endregion

    [Header("Config")]
    public float delayTime = 1.0f;
    public GameObject winImg, loseImg, totalImg;
    [Header("Buttons")]
    public Button yesButton;
    public Button retryButton, noButton;
    [Header("Message")]
    public TMP_Text msgTxt;
    public string loseMsg = "You lost all your winnings";
    public string winMsg = "You just won $";
    public string congratsMsg = "Congratulations! You won $";

    public void ShowMessageDelayed(bool isLose, int winnings = 0)
    {
        DelayedMessageAsync(isLose, winnings);
    }

    async void DelayedMessageAsync(bool isLose, int winnings = 0)
    {
        await Task.Delay((int)(delayTime * 1000));
        ShowMessage(isLose, winnings);
    }

    public void ShowMessage(bool isLose, int winnings = 0)
    {
        gameObject.SetActive(true);

        winImg.SetActive(!isLose && !ChainChestManager.instance.isLastRound);
        totalImg.SetActive(!isLose && ChainChestManager.instance.isLastRound);
        loseImg.SetActive(isLose);

        if(isLose)
        {
            msgTxt.text = loseMsg;
        }
        else
        {
            if(ChainChestManager.instance.isLastRound)
            {
                msgTxt.text = congratsMsg + ChainChestManager.instance.GetTotalScore();
            }
            else
            {
                msgTxt.text = winMsg + winnings;
            }
        }

        yesButton.gameObject.SetActive(!ChainChestManager.instance.isLastRound && !isLose);
        retryButton.gameObject.SetActive(ChainChestManager.instance.isLastRound || isLose);
    }
}
