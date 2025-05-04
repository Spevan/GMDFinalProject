using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Linq;
using Unity.Netcode;

public class scr_guiManager : MonoBehaviour
{
    public TextMeshProUGUI cardCount, waterCount;
    public GameObject hand, cardPrefab, statsTab;

    public float fanSpread;
    public int cardSpacing, verticalSpacing;
    public Canvas canvas;

    public List<GameObject> cardsInHand, playersList;

    public scr_analyticsManager analyticsManager;
    public TextMeshProUGUI cardsPlayed, timeElapsed;

    public GameObject playerStatPrefab;
    public List<GameObject> waterList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playersList = GameObject.FindGameObjectsWithTag("Player").ToList();
        CreateAnalytics();
        canvas = GetComponent<Canvas>();
        //Find all player objects to reference
        analyticsManager = GameObject.Find("obj_analyticsManager").GetComponent<scr_analyticsManager>();
        UpdateAnalytics();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            UpdateAnalytics();
            statsTab.SetActive(true);
            timeElapsed.text = analyticsManager.GetTimeElapsed().ToString();
        }
        else
        {
            statsTab.SetActive(false);
        }
    }

    //Function to draw a card to the player's hand
    public void DrawCard(scr_card nextCard, scr_player thisPlayer)
    {
        //Create a new card prefab
        cardsInHand.Add(Instantiate(cardPrefab, hand.transform));//.position, Quaternion.identity, hand.transform));
        scr_cardsInHand drawnCard = cardsInHand[cardsInHand.Count - 1].GetComponent<scr_cardsInHand>();
        drawnCard.cardData = nextCard;
        drawnCard.player = thisPlayer;
        drawnCard.GUI = this;
        UpdateHand();
    }

    public void ChangeCardCount(int deckCount)
    {
        cardCount.text = deckCount.ToString();
    }

    public void UpdateHand()
    {
        if (cardsInHand.Count == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.identity;
            cardsInHand[0].transform.localPosition = Vector3.zero;
            return;
        }

        for (int i = 0; i < cardsInHand.Count; i++)
        {
            float rotationalAngle = (fanSpread * (i - (cardsInHand.Count - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationalAngle);

            float horizontalOffset = (cardSpacing * (i - (cardsInHand.Count - 1) / 2f));

            float normalizedPosition = (2f * i / (cardsInHand.Count - 1) - 1f);
            float verticalOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            cardsInHand[i].transform.localPosition = new Vector3(horizontalOffset, verticalOffset, 0);
        }
    }

    public void RemoveCard(scr_player player, GameObject removedCard)
    {
        foreach (var card in cardsInHand)
        {
            if(card.Equals(removedCard))
            {
                cardsInHand.Remove(card);

                UpdateAnalytics();

                return;
            }
        }
    }

    public void UpdateWater(int water)
    {
        waterCount.text = water.ToString();
    }

    [ServerRpc]
    public void CreateAnalytics()
    {
        for(int i = 0; i < cardsInHand.Count; i++)
        {
            waterList.Add(Instantiate(playerStatPrefab, (statsTab.transform.position + new Vector3(0, i * 10, 0)),
                new Quaternion(0, 0, 0, 0), statsTab.transform));
        }
    }

    [ClientRpc]
    public void UpdateAnalytics()
    {
        for(int i = 0; i < playersList.Count; i++)
        {
            for (int j = 0; j < waterList.Count; j++)
            {
                //water updated
                waterList[j].GetComponent<TextMeshProUGUI>().text = playersList[j].GetComponent<scr_player>().NetworkObject.NetworkObjectId.ToString();
                waterList[j].GetComponentInChildren<TextMeshProUGUI>().text = playersList[j].GetComponent<scr_player>().water.Value.ToString();
                //Debug.Log("player " + (j + 1) + "'s water text updated on player " + (i + 1) + "'s screen");
            }
        }
    }
}
