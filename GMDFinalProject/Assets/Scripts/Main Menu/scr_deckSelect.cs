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

    public scr_deck SetData(scr_deck newData, GameObject newList)
    {
        deckData = newData;
        deckList = newList;
        deckData.loaded = true;
        deckData.deck_id = scr_dataPersistenceManager.instance.playerData.decksCollected.Count;
        return deckData;
        //SaveData(ref scr_dataPersistenceManager.instance.playerData);
    }

    private void Start()
    {
        menuManager = GameObject.Find("gui_mainMenuButtons").GetComponent<scr_mainMenuManager>();

        if(nameField.GetComponent<TMP_InputField>() != null)
        {
            nameField.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { EditDeckName(deckData, nameField.GetComponent<TMP_InputField>().text); });
        }
        else
        {
            nameField.GetComponent<TMP_Text>().text = deckData.name;
        }
    }

    public void LoadData(scr_playerData gameData)
    {
        foreach (scr_deck deck in gameData.decksCollected)
        {
            for (int i = 0; i < gameData.decksCollected.Count; i++)
            {
                if (/*deck.deck_id == i &&*/ !deck.loaded)
                {
                    deck.loaded = true;
                    deckData = deck;
                    
                    Debug.Log(deck.name + " card data loaded.");
                    return;
                }
            }
        }
    }

    public void SaveData(ref scr_playerData data)
    {
        scr_dataPersistenceManager.instance.SaveGame();
    }

    public void EditDeckName(scr_deck deck, string name)
    {
        //scr_deck newDeck;
        //AssetDatabase.RenameAsset(path + "temp.asset", name + ".asset");
        //newDeck = Resources.Load<scr_deck>(path + "temp.asset");
        Debug.Log("changing deck name.");

        if( deck != null )
        {
            deck.name = name;
            //EditorUtility.SetDirty(newDeck);
            //AssetDatabase.SaveAssets();
        }
        else
        {
            Debug.Log(name + " could not be found.");
        }

        nameField.GetComponent<Image>().enabled = false;
        nameField.GetComponent<TMP_InputField>().enabled = false;
        SaveData(ref scr_dataPersistenceManager.instance.playerData);
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

    public void EquipDeck()
    {
        scr_dataPersistenceManager.instance.playerData.equippedDeck = deckData;
    }

    public void EditDeckCards()
    {
        menuManager.LockMenu();
        deckList.GetComponent<scr_deckEdit>().SelectDeck(deckData);
        deckList.SetActive(true);
        SaveData(ref scr_dataPersistenceManager.instance.playerData);
    }

    public void DeleteDeck()
    {
        //scr_dataPersistenceManager.instance.RemoveDeckFromCollection(deckData);
        Destroy(this.gameObject);
        SaveData(ref scr_dataPersistenceManager.instance.playerData);
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
