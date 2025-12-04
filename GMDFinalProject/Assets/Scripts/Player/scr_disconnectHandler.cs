using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;
using WebSocketSharp.Server;

public class scr_disconnectHandler : MonoBehaviour
{
    scr_player player;

    private void OnEnable()
    {
        player = GetComponentInParent<scr_player>();
    }

    public void Disconnect()
    {
        NetworkManager.Singleton.Shutdown();
        WaitForNetworkShutdownAndLoadScene();
    }

    private IEnumerator WaitForNetworkShutdownAndLoadScene()
    {
        while (NetworkManager.Singleton.ShutdownInProgress)
        {
            Debug.Log("Something is preventing the server from shutting down.");
            yield return null;            
        }
        ReturnToMenu();
    }

    //[ClientRpc]
    public void ReturnToMenu()
    {
        SceneManager.LoadScene("sce_mainMenu");
    }
}
