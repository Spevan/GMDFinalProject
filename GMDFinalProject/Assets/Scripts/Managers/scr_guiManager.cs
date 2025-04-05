using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class scr_guiManager : MonoBehaviour
{
    public GameObject[] players;
    public TextMeshProUGUI cardCount, waterCount;
    public GameObject hand, cardPrefab, statsTab;

    public float fanSpread;
    public int cardSpacing, verticalSpacing;
    public Canvas canvas;

    public List<GameObject> cardsInHand;

    public scr_analyticsManager analyticsManager;
    public TextMeshProUGUI cardsPlayed, waterUsed, timeElapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GetComponent<Canvas>();
        //Find all player objects to reference
        players = GameObject.FindGameObjectsWithTag("Player");
        analyticsManager = GameObject.Find("obj_analyticsManager").GetComponent<scr_analyticsManager>();
    }

    private void Update()
    {
        if(Input.GetKey(KeyCode.Tab))
        {
            statsTab.SetActive(true);
            timeElapsed.text = analyticsManager.GetTimeElapsed().ToString();
        }
        else
        {
            statsTab.SetActive(false);
        }
    }

    //Function to draw a card to the player's hand
    public void DrawCard(scr_card nextCard)
    {
        //Create a new card prefab
        cardsInHand.Add(Instantiate(cardPrefab, hand.transform));//.position, Quaternion.identity, hand.transform));
        cardsInHand[cardsInHand.Count - 1].GetComponent<scr_cardsInHand>().cardData = nextCard;

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

    public void RemoveCard(GameObject removedCard)
    {
        foreach (var card in cardsInHand)
        {
            if(card.Equals(removedCard))
            {
                cardsInHand.Remove(card);

                cardsPlayed.text = analyticsManager.GetCardsPlayed().ToString();

                waterUsed.text = analyticsManager.GetWaterUsed(card.GetComponent<scr_cardsInHand>().cardData.cost).ToString();

                return;
            }
        }
    }

    public void UpdateWater(int water)
    {
        waterCount.text = water.ToString();

        if (analyticsManager != null)
        {
            
        }
    }
}
