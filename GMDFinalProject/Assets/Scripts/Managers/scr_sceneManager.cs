using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_sceneManager : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
