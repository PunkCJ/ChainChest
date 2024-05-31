using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomChest : MonoBehaviour
{
    [Header("Config")]
    public bool isActive = false;
    public bool isKillChest = false;
    public bool isSelected = false;
    int chestValue = 0;

    [Header("UI")]
    public Button openButton;

    [Header("Sprites")]
    public List<SpriteVisual> chestVisuals = new List<SpriteVisual>();
    SpriteVisual selectedVisual = null;

    public void InitChest(int value)
    {
        chestValue = value;

        SelectRenderer();

        gameObject.SetActive(true);
        isActive = true;
    }

    public void MakeKillChest()
    {
        isKillChest = true;
        chestValue = 0;
    }

    public void SelectChest()
    {
        if (ChainChestManager.canSelectChests)
        {
            ChainChestManager.instance.RespositionLight(transform.position);
            ChainChestManager.instance.DeselectAllChests();
            isSelected = true;
            openButton.gameObject.SetActive(true);
        }
    }

    public void DeselectChest()
    {
        isSelected = false;
        openButton.gameObject.SetActive(false);
    }

    public void OpenChest()
    {
        selectedVisual.spriteAnimator.SetBool("Open", true);
        ChainChestManager.canSelectChests = false;
    }

    public void ChestOpened()
    {
        if (isActive && isSelected)
        {
            ChestContentObj.instance.ShowContent(isKillChest,transform.position);
            ScorePanel.instance.ShowMessageDelayed(isKillChest, chestValue);
            //
            if(isKillChest)
            {
                ChainChestManager.instance.KillScore();
            }
            else
            {
                ChainChestManager.instance.AddScore(chestValue);
            }
        }
    }

    public void Deactivate()
    {
        isActive = false;
        chestValue = 0;
        isKillChest = false;
        gameObject.SetActive(false);
        ChainChestManager.instance.ReturnChestToPool(this);
    }

    #region Chest Visuals

    void SelectRenderer()
    {
        DeactivateAllVisuals();
        //
        selectedVisual = chestVisuals[Random.Range(0, chestVisuals.Count)];
        selectedVisual.SetActivate(true);
    }

    void DeactivateAllVisuals()
    {
        for (int i = 0; i < chestVisuals.Count; i++) 
        {
            chestVisuals[i].SetActivate(false);
        }
    }

    #endregion
}

[System.Serializable]
public class SpriteVisual
{
    public SpriteRenderer spriteRenderer;
    public Animator spriteAnimator;

    public void SetActivate(bool activate)
    {
        spriteRenderer.gameObject.SetActive(activate);
    }
}
