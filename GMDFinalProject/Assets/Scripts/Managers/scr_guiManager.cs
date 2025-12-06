using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;
using UnityEngine;

public class scr_guiManager : MonoBehaviour
{
    public TextMeshProUGUI cardCount, waterCount;
    public GameObject hand, cardPrefab, statsTab, endingTab, cardDetails;

    public float fanSpread;
    public int cardSpacing, verticalSpacing;
    public Canvas canvas;

    public List<GameObject> cardsInHand, playersList;

    public scr_analyticsManager analyticsManager;
    public TextMeshProUGUI timeElapsed;

    public GameObject playerStatPrefab;
    public List<GameObject> waterList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playersList = GameObject.FindGameObjectsWithTag("Player").ToList();
        CreateAnalytics();
        canvas = GetComponent<Canvas>();
        //Find all player objects to reference
        //analyticsManager = GameObject.Find("obj_analyticsManager").GetComponent<scr_analyticsManager>();
        //UpdateAnalytics();
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
        drawnCard.CardDrawn(nextCard, thisPlayer, this.gameObject);
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

    public void CreateAnalytics()
    {
        for(int i = 0; i < cardsInHand.Count; i++)
        {
            waterList.Add(Instantiate(playerStatPrefab, (statsTab.transform.position + new Vector3(0, i * 10, 0)),
                new Quaternion(0, 0, 0, 0), statsTab.transform));
        }
    }

    public void UpdateAnalytics()
    {
        switch(playersList.Count)
        {
            default:
            case 0:
                analyticsManager.UpdateAnalytics(new scr_player(), new scr_player());
                break;
            case 1:
                analyticsManager.UpdateAnalytics(playersList[0].GetComponent<scr_player>(), new scr_player()); 
                break;
            case 2:
                analyticsManager.UpdateAnalytics(playersList[0].GetComponent<scr_player>(), playersList[1].GetComponent<scr_player>());
                break;
        }
    }

    public void DisplayCardDetails(scr_card cardData, float health, float cooldown, float power, float range, float speed)
    {
        cardDetails.SetActive(true);
        cardDetails.GetComponentInChildren<scr_cardDetails>().cardData = cardData;
        //cardDetails.GetComponentInChildren<scr_cardDetails>().Start();
        cardDetails.GetComponentInChildren<scr_cardDetails>().DetailUpdate(health, cooldown, power, range, speed);
    }

    public void HideCardDetails()
    {
        cardDetails.SetActive(false);
    }
}
