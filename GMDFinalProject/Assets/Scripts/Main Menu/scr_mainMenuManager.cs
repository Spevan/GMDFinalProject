using UnityEngine;
using UnityEngine.UI;

public class scr_mainMenuManager : MonoBehaviour
{
    public GameObject[] menuLock;
    public bool deckEditMode;

    private void Start()
    {
        deckEditMode = false;
    }

    public void LockMenu()
    {
        foreach (GameObject go in menuLock)
        {
            go.GetComponent<Button>().interactable = false;
            deckEditMode = true;
        }
    }

    public void UnlockMenu()
    {
        foreach (GameObject go in menuLock)
        {
            go.GetComponent<Button>().interactable = true;
            deckEditMode = false;
        }
    }
}
