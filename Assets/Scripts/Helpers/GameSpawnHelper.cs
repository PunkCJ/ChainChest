using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameSpawnHelper : MonoBehaviour
{
    public GameObject gameToSpawn;
    public GameObject playButton;

    [Header("UI")]
    public TMP_Text winningsTxt;
    int totalWinnings = 0;

    public void SpawnGame()
    {
        Instantiate(gameToSpawn);
    }

    public void BackToMenu(int winnings = 0)
    {
        playButton.SetActive(true);
        //
        totalWinnings += winnings;
        winningsTxt.text = "Winnings: $" + totalWinnings;
        //
        GC.Collect();
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}
