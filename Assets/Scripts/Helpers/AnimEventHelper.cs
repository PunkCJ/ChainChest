using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventHelper : MonoBehaviour
{
    RandomChest chest;

    private void Awake()
    {
        chest = transform.parent.GetComponent<RandomChest>();
    }

    public void ChestOpened()
    {
        chest.ChestOpened();
    }
}
