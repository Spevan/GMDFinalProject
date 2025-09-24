using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

public class scr_dataPersistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    [SerializeField] private bool useEncryption;
    public static scr_dataPersistenceManager instance { get; private set; }

    [SerializeField] public scr_playerData playerData;
    private List<scr_IDataPersistence> dataPersistenceObjects;
    private scr_fileDataHandler dataHandler;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        this.dataHandler = new scr_fileDataHandler(Application.persistentDataPath + "/", fileName, useEncryption);
        this.dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    public void NewGame()
    {
        this.playerData = new scr_playerData();
    }

    public void LoadGame()
    {
        playerData = dataHandler.Load();

        if (this.playerData == null)
        {
            Debug.Log("No data was found. Initializing data to defaults.");
            NewGame();
        }

        foreach (scr_IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            dataPersistenceObj.LoadData(playerData);
        }

        Debug.Log("Loaded collection");
        /*Debug.Log(playerData.decks.Count);
        foreach (scr_deck deck in playerData.decks)
        {
            Debug.Log(deck.ToString());
        }*/
    }

    public void SaveGame()
    {
        //foreach (scr_IDataPersistence dataPersistenceObj in dataPersistenceObjects)
        {
            //dataPersistenceObj.SaveData(ref playerData);
        }

        Debug.Log("Saved Inventory");

        dataHandler.Save(playerData);
    }

    private List<scr_IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<scr_IDataPersistence> dataPersistenceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.InstanceID).OfType<scr_IDataPersistence>();
        return new List<scr_IDataPersistence>(dataPersistenceObjects);
    }

    /*public void RemoveDeckFromCollection(scr_deck deck)
    {
        playerData.decks.Remove(deck);
        SaveGame();
    }*/
}