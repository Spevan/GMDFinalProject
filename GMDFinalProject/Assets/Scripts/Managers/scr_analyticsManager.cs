using NUnit.Framework;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class scr_analyticsManager : MonoBehaviour
{
    int minsElapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        minsElapsed = (int)Time.fixedTime;
    }

    [ServerRpc]
    public int GetTimeElapsed()
    {
        return minsElapsed;
    }
}
