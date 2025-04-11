using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_sceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        //SceneManager.LoadScene(sceneName);
    }
}
