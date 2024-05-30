using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
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

    // Start is called before the first frame update
    void Start()
    {
        SpawnChests();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnChests()
    {
        ClearChests();

        for (int i = 0; i < chestsToSpawn; i++)
        {
            RandomChest newChest = GetChestFromPool();
            newChest.transform.SetParent(rChestsParent);
            newChest.InitChest(chestValues[Random.Range(0, chestValues.Count)]);
            rChests.Add(newChest);
        }
    }

    void ClearChests()
    {
        for (int i = 0; i < rChests.Count; i++)
        {
            rChests[i].Deactivate();
        }
    }

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
        chest.GetComponent<RectTransform>().parent = rChestsPoolParent;
    }

    #endregion

    #region UI

    public void RespositionLight(Vector3 newPos)
    {
        selectLight.transform.position = new Vector3(newPos.x, newPos.y, selectLight.transform.position.z);
    }

    public void DeselectAllChests()
    {
        for(int i = 0; i < rChests.Count; i++)
        {
            rChests[i].DeselectChest();
        }
    }

    #endregion
}
