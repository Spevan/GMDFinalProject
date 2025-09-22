using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

//This is the GO that players will manipulate to edit their decks
public class scr_deckSelect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, scr_IDataPersistence
{
    public scr_mainMenuManager menuManager;
    public scr_deck deckData;
    public GameObject nameField, options, deckList;
    string path = "Assets/Scriptable Objects/Decks/";

    private void Start()
    {
        menuManager = GameObject.Find("gui_mainMenuButtons").GetComponent<scr_mainMenuManager>();

        if(nameField.GetComponent<TMP_InputField>() != null)
        {
            nameField.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { EditDeckName(nameField.GetComponent<TMP_InputField>().text); });
        }
    }

    public void LoadData(scr_playerData gameData)
    {
        foreach (scr_deck deck in gameData.decksCollected)
        {
            for (int i = 0; i < gameData.decksCollected.Count; i++)
            {
                if (deck.deck_id == i && !deck.loaded)
                {
                    deck.loaded = true;
                    deckData = deck;
                    nameField.GetComponent<TextMeshProUGUI>().text = deck.name;
                    Debug.Log(deck.name + " card data loaded.");
                    return;
                }
            }
        }
    }

    public void SaveData(ref scr_playerData data)
    {
        deckData.loaded = false;
        deckData.deck_id = data.decksCollected.Count;
        data.decksCollected.Add(deckData);
        scr_dataPersistenceManager.instance.SaveGame();
    }

    public void EditDeckName(string name)
    {
        AssetDatabase.RenameAsset(path + "temp.asset", name + ".asset");
        scr_deck newDeck = AssetDatabase.LoadAssetAtPath<scr_deck>(path + name + ".asset");

        if( newDeck != null )
        {
            newDeck.name = name;
            EditorUtility.SetDirty(newDeck);
            AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.Log(name + " could not be found.");
        }

        nameField.GetComponent<Image>().enabled = false;
        nameField.GetComponent<TMP_InputField>().enabled = false;
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        TMP_InputField editName = nameField.GetComponent<TMP_InputField>();
        if (editName == null || !editName.isActiveAndEnabled)
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
        deckList.GetComponent<scr_deckEdit>().SelectDeck(deckData);
        deckList.SetActive(true);
    }

    public void DeleteDeck()
    {
        //scr_dataPersistenceManager.instance.RemoveDeckFromCollection(deckData);
        Destroy(this.gameObject);
    }

    private void OnDestroy()
    {
        deckData.loaded = false;
    }

    private void OnApplicationQuit()
    {
        OnDestroy();
    }
}
