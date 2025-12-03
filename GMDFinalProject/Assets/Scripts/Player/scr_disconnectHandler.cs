using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            yield return null;
        }
        ReturnToMenuClientRpc();
    }

    [ClientRpc]
    public void ReturnToMenuClientRpc()
    {
        SceneManager.LoadScene("sce_mainMenu");
    }
}
