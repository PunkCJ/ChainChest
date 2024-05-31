using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsHelper : MonoBehaviour
{
    public int limitFPS = 60;

    private void Awake()
    {
        Application.targetFrameRate = limitFPS;
    }
}
