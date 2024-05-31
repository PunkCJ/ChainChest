using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class ChainChestManager : MonoBehaviour
{
    #region Singleton

    public static ChainChestManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
        //
        gameCanvas.worldCamera = Camera.main;
        CreateChestPool();
    }

    #endregion

    [Header("Game Config")]
    public int chestsToSpawn = 3;
    public int numberOfRounds = 5;
    int currentRound = 1;
    [HideInInspector] public bool isLastRound = false;

    int currentScore = 0;

    public static bool canSelectChests = true;

    [Header("Chest Config")]
    public List<int> chestValues = new List<int>();

    [Header("Chest Spawning Config")]
    public RandomChest rChestPrefab;
    public Transform rChestsParent;
    List<RandomChest> rChests = new List<RandomChest>();
    [Space(10)]
    public int poolInitCount = 50;
    public Transform rChestsPoolParent;
    List<RandomChest> rChestPool = new List<RandomChest>();

    [Header("UI")]
    public Canvas gameCanvas;
    public Light2D selectLight;
    [Space(10)]
    public TMP_Text roundTxt;
    public TMP_Text scoreTxt;  

    public void StartGame()
    {
        currentRound = 1;
        currentScore = 0;
        SpawnChests(currentRound); ;
    }

    public void AddScore(int score)
    {
        currentScore += score;
        //
        UpdateUI();
    }

    public int GetTotalScore()
    {
        return currentScore;
    }

    public void KillScore()
    {
        currentScore = 0;
        //
        UpdateUI();
    }

    public void QuitGame()
    {
        Destroy(gameObject);
    }


    #region Chest Spawning

    public void SpawnChests(int round)
    {
        ClearChests();
        canSelectChests = true;

        switch (round)
        {
            case 1:
                InternalChestSpawn();
                break;
            default:
                InternalChestSpawn();
                AssignKillChest();
                break;
        }
        //
        ChestContentObj.instance.Hide();
        //
        selectLight.gameObject.SetActive(false);
        UpdateUI();
    }

    void InternalChestSpawn()
    {
        for (int i = 0; i < chestsToSpawn; i++)
        {
            RandomChest newChest = GetChestFromPool();
            newChest.transform.SetParent(rChestsParent);
            newChest.InitChest(chestValues[Random.Range(0, chestValues.Count)]);
            rChests.Add(newChest);
        }
    }

    void AssignKillChest()
    {
        int k = Random.Range(0, rChests.Count);
        rChests[k].MakeKillChest();
#if UNITY_EDITOR
        Debug.LogWarning("Chest #" + k + " is the kill chest");
#endif
    }

    void ClearChests()
    {
        for (int i = 0; i < rChests.Count; i++)
        {
            rChests[i].Deactivate();
        }
        //
        rChests.Clear();
    }

    #endregion

    #region Round Management

    public void ToNextRound()
    {
        currentRound++;
        SpawnChests(currentRound);
        //
        UpdateUI();
        isLastRound = (currentRound == numberOfRounds);
    }

    #endregion

    #region Chest Pooling

    void CreateChestPool()
    {
        for (int i = 0; i < poolInitCount; i++)
        {
            InstanciateChest(i);
        }
    }

    void InstanciateChest(int index)
    {
        RandomChest newChest = Instantiate(rChestPrefab, rChestsPoolParent);
        rChestPool.Add(newChest);
        newChest.gameObject.SetActive(false);
    }

    RandomChest GetChestFromPool()
    {
        for (int i = 0; i < rChestPool.Count; i++) 
        {
            if(rChestPool[i].isActive == false)
                return rChestPool[i];
        }
        return null;
    }

    public void ReturnChestToPool(RandomChest chest)
    {
        chest.DeselectChest();
        chest.GetComponent<RectTransform>().SetParent(rChestsPoolParent);
    }

    #endregion

    #region UI

    public void RespositionLight(Vector3 newPos)
    {
        selectLight.gameObject.SetActive(true);
        selectLight.transform.position = new Vector3(newPos.x, newPos.y, selectLight.transform.position.z);
    }

    public void DeselectAllChests()
    {
        for(int i = 0; i < rChests.Count; i++)
        {
            rChests[i].DeselectChest();
        }
    }

    void UpdateUI()
    {
        roundTxt.text = "Round: " + currentRound + "/" + numberOfRounds;
        scoreTxt.text = "$" + currentScore;
    }

    #endregion
}
