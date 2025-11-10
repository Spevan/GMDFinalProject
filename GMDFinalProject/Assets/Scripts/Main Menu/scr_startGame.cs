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
        startGameBTN.SetActive(false);
    }

    private void Update()
    {
        NetworkManager.Singleton.OnServerStarted += ServerSetup;
    }

    void ServerSetup()
    {
        startGameBTN.SetActive(true);
    }
}
