using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestContentObj : MonoBehaviour
{
    #region Singleton

    public static ChestContentObj instance = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        //
        gameObject.SetActive(false);
    }

    #endregion

    public GameObject winImg, loseImg;

    public UnityEvent onWin, onLose;

    public void ShowContent(bool isKill, Vector2 positon)
    {
        gameObject.SetActive(true);

        if(isKill) { onLose.Invoke(); } else { onWin.Invoke(); }

        winImg.SetActive(!isKill);
        loseImg.SetActive(isKill);
        //
        transform.position = positon;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
