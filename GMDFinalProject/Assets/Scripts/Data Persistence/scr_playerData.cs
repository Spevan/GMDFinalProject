using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class scr_playerData 
{
    public string username;
    public int currency;
    public scr_deck equippedDeck;
    [SerializeField] public List<scr_deck> decksCollected;
    [SerializeField] public List<scr_card> cardsCollected;
    [SerializeField] public List<scr_productionPlant> productionPlants;
    public scr_playerData()
    {
        username = "";
        currency = 0;
        equippedDeck = new scr_deck();
        decksCollected = new List<scr_deck>();
        cardsCollected = new List<scr_card>();
        productionPlants = new List<scr_productionPlant>();
    }
}
