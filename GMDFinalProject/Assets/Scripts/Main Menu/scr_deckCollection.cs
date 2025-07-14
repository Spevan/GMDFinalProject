using UnityEditor;
using UnityEngine;

public class scr_deckCollection : scr_collection
{
    string path = "Assets/Scriptable Objects/Decks/";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        foreach (scr_deck deck in scr_dataPersistenceManager.instance.playerData.decks)
        {
            CreateDeck(deck);
        }
    }

    public void CreateDeck()
    {
        GameObject tempObj = Instantiate(prefab, grid.transform);

        ScriptableObject temp = ScriptableObject.CreateInstance(typeof(scr_deck));
        AssetDatabase.CreateAsset(temp, path + "temp.asset");
        AssetDatabase.Refresh();

        tempObj.GetComponent<scr_deckEdit>().deckData = (scr_deck)temp;
        scr_dataPersistenceManager.instance.AddDeckToCollection((scr_deck)temp);
    }

    public void CreateDeck(scr_deck deck)
    {
        GameObject temp = Instantiate(prefab, grid.transform);
        temp.GetComponent<scr_deckEdit>().deckData = deck;
    }
}
