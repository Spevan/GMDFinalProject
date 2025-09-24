using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class scr_playerData 
{
    public string username;
    public int currency;
    [SerializeField] public List<scr_deck> decksCollected;
    [SerializeField] public List<scr_card> cardsCollected;

    public scr_playerData()
    {
        username = "";
        currency = 0;
        decksCollected = new List<scr_deck>();
        cardsCollected = new List<scr_card>();
    }
}
