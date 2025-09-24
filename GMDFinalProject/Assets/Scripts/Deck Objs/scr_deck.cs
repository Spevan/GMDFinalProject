using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

//[CreateAssetMenu(fileName = "deck", menuName = "Scriptable Sheets/deck")]
[System.Serializable]
public class scr_deck 
{
    public string name;
    public bool loaded;
    public int deck_id;
    [SerializeField] public scr_productionPlant productionPlant;
    [SerializeField] public List<scr_card> cardsInDeck;
    public scr_deck()
    {
        loaded = false;
        cardsInDeck = new List<scr_card>();
    }
}
