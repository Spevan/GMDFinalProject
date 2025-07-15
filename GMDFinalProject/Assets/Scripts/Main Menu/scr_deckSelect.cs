using TMPro;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_deckSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public scr_mainMenuManager menuManager;
    public scr_deck deckData;
    public GameObject nameField, options, editList;
    string path = "Assets/Scriptable Objects/Decks/";

    private void Start()
    {
        menuManager = GameObject.Find("gui_mainMenuButtons").GetComponent<scr_mainMenuManager>();

        if(nameField.GetComponent<TMP_InputField>() != null)
        {
            nameField.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { EditDeckName(nameField.GetComponent<TMP_InputField>().text); });
        }
        else
        {
            nameField.GetComponent<TMP_Text>().text = deckData.name;
        }
    }

    public void EditDeckName(string name)
    {
        AssetDatabase.RenameAsset(path + "temp.asset", name + ".asset");
        deckData.name = name;
        AssetDatabase.Refresh();
        nameField.GetComponent<Image>().enabled = false;
        nameField.GetComponent<TMP_InputField>().enabled = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (nameField.GetComponent<TMP_InputField>() == null)
        {
            options.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        options.SetActive(false);
    }

    public void EditDeckCards()
    {
        menuManager.LockMenu();
        editList.SetActive(true);
    }

    public void DeleteDeck()
    {

    }
}
