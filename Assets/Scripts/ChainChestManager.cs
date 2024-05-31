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
    [Space(10)]
    public float lightMoveTime = .3f;
    public AnimationCurve lightMoveCurve;
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
        FindFirstObjectByType<GameSpawnHelper>().BackToMenu(GetTotalScore());
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
        if (chestsToSpawn > 40)
        {
            Debug.LogWarning("Max chests to spawn is 40, sorry :(");

            for (int i = 0; i < 40; i++)
            {
                RandomChest newChest = GetChestFromPool();
                newChest.transform.SetParent(rChestsParent);
                newChest.InitChest(chestValues[Random.Range(0, chestValues.Count)]);
                rChests.Add(newChest);
            }
        }
        else if (chestsToSpawn < 2)
        {
            Debug.LogWarning("Min chests to spawn is 2, sorry :(");

            for (int i = 0; i < 2; i++)
            {
                RandomChest newChest = GetChestFromPool();
                newChest.transform.SetParent(rChestsParent);
                newChest.InitChest(chestValues[Random.Range(0, chestValues.Count)]);
                rChests.Add(newChest);
            }
        }
        else
        {
            for (int i = 0; i < chestsToSpawn; i++)
            {
                RandomChest newChest = GetChestFromPool();
                newChest.transform.SetParent(rChestsParent);
                newChest.InitChest(chestValues[Random.Range(0, chestValues.Count)]);
                rChests.Add(newChest);
            }
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

    Vector2 startLightPos = Vector2.zero;

    public void RespositionLight(Vector2 newPos)
    {
        selectLight.gameObject.SetActive(true);
        startLightPos = selectLight.transform.position;
        StartCoroutine(LerpLightPos(newPos, lightMoveTime));
    }

    IEnumerator LerpLightPos(Vector2 end, float timeToMove)
    {
        float t = 0;
        while (t < 1)
        {
            selectLight.transform.position = Vector2.Lerp(startLightPos, end, lightMoveCurve.Evaluate(t));
            t = t + Time.deltaTime / timeToMove;
            yield return new WaitForEndOfFrame();
        }
        selectLight.transform.position = end;
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
