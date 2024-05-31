using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSpawnHelper : MonoBehaviour
{
    public GameObject gameToSpawn;

    public void SpawnGame()
    {
        Instantiate(gameToSpawn);
    }
}
