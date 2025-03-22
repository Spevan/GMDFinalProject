using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class scr_guiManager : MonoBehaviour
{
    public GameObject[] players;
    public TextMeshProUGUI cardCount, waterCount;
    public GameObject hand, cardPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DrawCard(scr_card nextCard)
    {
        Instantiate(cardPrefab, hand.transform);
    }

    public void ChangeCardCount(int delta)
    {
        //cardCount.text = 
    }
}
