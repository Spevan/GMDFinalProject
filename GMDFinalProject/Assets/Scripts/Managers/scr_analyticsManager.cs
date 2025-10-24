using NUnit.Framework;
using Unity.Netcode;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using TMPro;

public class scr_analyticsManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName, deckSize, cardsPlayed, waterTotal, targetsDestroyed,
        enemyPlayerName, enemyDeckSize, enemyCardsPlayed, enemyWaterTotal, enemyTargetsDestroyed,
        timeElapsed;
    int time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time = (int)Time.timeSinceLevelLoad;
    }

    [ServerRpc]
    public int GetTimeElapsed()
    {
        return time;
    }

    public void UpdateAnalytics(scr_player player1, scr_player player2)
    {
        timeElapsed.text = time.ToString();

        if (player1 != null)
        {
            playerName.text = player1.playerName;
            deckSize.text = player1.Deck.Count.ToString();
            cardsPlayed.text = player1.cardsPlayed.ToString();
            waterTotal.text = player1.water.Value.ToString();
        }

        if (player2 != null)
        {
            enemyPlayerName.text = player2.playerName;
            enemyDeckSize.text = player2.Deck.Count.ToString();
            enemyCardsPlayed.text = player2.cardsPlayed.ToString();
            enemyWaterTotal.text = player2.water.Value.ToString();
        }
    }
}
