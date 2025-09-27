using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_cardsInMenu : scr_cards
{
    scr_mainMenuManager menuManager; 
    public GameObject deckEditBtns, deckList, alertText;

    string deckPath = "Assets/Scriptable Objects/Decks/";

    private void Start()
    {
        menuManager = GameObject.Find("gui_mainMenuButtons").GetComponent<scr_mainMenuManager>();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        temp = Instantiate(details, GUI.transform);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
        if (this.transform.localPosition.x < (GUI.GetComponentInChildren<Camera>().scaledPixelWidth / 2))
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(80, 0, 5);
        }
        else
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(-80, 0, 5);
        }

        if (menuManager.deckEditMode == true)
        {
            deckEditBtns.SetActive(true);
        }
        else
        {
            deckEditBtns.SetActive(false);
        }
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        deckEditBtns.SetActive(false);
    }    

    public void AddCard()
    {
        scr_deckEdit editList = deckList.GetComponent<scr_deckEdit>();
        scr_deck tempDeck = editList.selectedDeck;
        if (tempDeck != null)
        {
            if(cardData.GetType() == typeof(scr_productionPlant) && editList.selectedDeck.productionPlant != null)
            {
                GameObject temp = Instantiate(alertText, GUI.GetComponent<RectTransform>().TransformPoint(GUI.GetComponent<RectTransform>().rect.center), Quaternion.identity, GUI.transform);
                temp.GetComponent<scr_alertText>().ChangeText("You have already added a Production Plant to this deck.\n" +
                    "Please remove the previous Plant before adding a new one.");
            }
            else if(cardData.GetType() == typeof(scr_productionPlant) && editList.selectedDeck.productionPlant == null)
            {
                tempDeck.productionPlant = (scr_productionPlant)cardData;
                editList.HideDeck();
                editList.DisplayDeck();
            }
            else
            {
                tempDeck.cardsInDeck.Add(cardData);
                //Resources.UnloadAsset(tempDeck);
                editList.HideDeck();
                editList.DisplayDeck();
            }
        }
    }

    public void RemoveCard(scr_deck tempDeck)
    {
        scr_deckEdit editList = deckList.GetComponent<scr_deckEdit>();
        //scr_deck tempDeck = Resources.Load<scr_deck>(deckPath + editList.selectedDeck.name + ".asset");
        if (tempDeck != null)
        {
            OnPointerExit(null);
            tempDeck.cardsInDeck.Remove(cardData);
            //Resources.UnloadAsset(tempDeck);
            editList.HideDeck();
            editList.DisplayDeck();
        }
    }
}
