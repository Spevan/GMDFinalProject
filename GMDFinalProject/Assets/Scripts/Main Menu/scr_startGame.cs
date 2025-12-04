using System;
using System.Runtime.CompilerServices;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class scr_startGame : MonoBehaviour
{
    [SerializeField] GameObject startGameBTN;
    private void Start()
    {
        CheckStartServer();
    }

    private void Update()
    {
        
    }

    public void CheckStartServer()
    {
        if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsServer)
        {
            ServerSetup();
        }
        else
        {
            startGameBTN.SetActive(false);
        }
    }

    void ServerSetup()
    {
        startGameBTN.SetActive(true);
    }

    void ServerSetup(ulong ownerClientID)
    {
        startGameBTN.SetActive(true);
    }
}
