using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "deck", menuName = "Scriptable Sheets/scr_deck")]
public class scr_deck : ScriptableObject
{
    public bool loaded;
    public new string name;
    public List<scr_card> cardsInDeck;

    public scr_deck()
    {
        loaded = false;
        name = "";
        cardsInDeck = new List<scr_card>();
    }
}
