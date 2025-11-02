using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class scr_deckCollection : scr_collection
{
    public scr_deck temp;
    public GameObject newDeck;
    public int total;

    private new void OnEnable()
    {
        base.OnEnable();
        total = scr_dataPersistenceManager.instance.playerData.decksCollected.Count;
    }

    public override void Update()
    {
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
        }
        else if (count < total) //&& temp == null)
        {
            CreateDeck(scr_dataPersistenceManager.instance.playerData.decksCollected[count]);
            count++;
            Debug.Log(count);
            timeRemain = 0.25f;
        }
    }

    public void CreateDeck()
    {
        GameObject tempObj = Instantiate(newDeck, grid.transform);

        temp = new scr_deck();//ScriptableObject.CreateInstance(typeof(scr_deck));

        //scr_deckSelect newSelect = tempObj.GetComponent<scr_deckSelect>();
        
        scr_dataPersistenceManager.instance.playerData.decksCollected.Add(
            tempObj.GetComponent<scr_deckSelect>().SetData((scr_deck)temp, deckList));
        tempObj.GetComponent<scr_deckSelect>().SaveData(ref scr_dataPersistenceManager.instance.playerData);
        //AssetDatabase.CreateAsset(temp, path + "temp.asset");
        //AssetDatabase.SaveAssets();

        /*tempObj.GetComponent<scr_deckSelect>().deckData = (scr_deck)temp;
        tempObj.GetComponent<scr_deckSelect>().deckList = deckList;
        tempObj.GetComponent<scr_deckSelect>().SaveData(ref scr_dataPersistenceManager.instance.playerData);*/
    }

    public void CreateDeck(scr_deck deck)
    {
        GameObject temp = Instantiate(prefab, grid.transform);
        temp.GetComponent<scr_deckSelect>().SetData(deck, deckList);
        //temp.GetComponent<scr_deckSelect>().LoadData(scr_dataPersistenceManager.instance.playerData);
        //temp.GetComponent<scr_deckSelect>().deckList = deckList;
    }

    private new void OnDisable()
    {
        base.OnDisable();
        temp = null;
    }
}
