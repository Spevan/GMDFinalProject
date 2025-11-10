using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_sceneManager : MonoBehaviour
{
    public void ChangeSceneNetwork(string sceneName)
    {
        if (NetworkManager.Singleton.IsServer)
        {
            NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        }
        //SceneManager.LoadScene(sceneName);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
