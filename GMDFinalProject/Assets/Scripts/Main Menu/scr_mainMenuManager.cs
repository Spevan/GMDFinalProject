using UnityEngine;
using UnityEngine.UI;

public class scr_mainMenuManager : MonoBehaviour
{
    public GameObject[] menuLock;

    public void LockMenu()
    {
        foreach (GameObject go in menuLock)
        {
            go.GetComponent<Button>().interactable = false;
        }
    }

    public void UnlockMenu()
    {
        foreach (GameObject go in menuLock)
        {
            go.GetComponent<Button>().interactable = true;
        }
    }
}
