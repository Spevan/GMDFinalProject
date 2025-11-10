using TMPro;
using UnityEngine;
using UnityEngine.UI;

//This is the GO for the deck edit list that displays all cards in a deck
public class scr_deckEdit : MonoBehaviour
{
    public GameObject cardPrefab;
    bool isVisible = true;
    Vector3 moveDistance = new Vector3(160, 0, 0);
    public scr_deck selectedDeck; public scr_mainMenuManager menuManager;
    public VerticalLayoutGroup deckList;
    public TextMeshProUGUI deckName;
    Canvas canvas;

    private void OnEnable()
    {
        ChangeDisplayName();
        DisplayDeck();
        canvas = GameObject.Find("gui_canvas").GetComponent<Canvas>();
    }

    private void OnDisable()
    {
        HideDeck();
    }

    public void ChangeDisplayName()
    {
        deckName.text = selectedDeck.name;
    }

    public void ToggleViewList()
    {
        if(isVisible)
        {
            transform.Translate(moveDistance);
            HideDeck();
            isVisible = false;
        }
        else
        {
            transform.Translate(-moveDistance);
            DisplayDeck();
            isVisible = true;
        }
    }

    public void SelectDeck(scr_deck deck)
    {
        selectedDeck = deck;
    }

    public void DisplayDeck()
    {
        if(selectedDeck != null)
        {
            if (selectedDeck.productionPlant != null)
            {
                GameObject temp = Instantiate(cardPrefab, deckList.transform);
                scr_cardsInDeck tempCard = temp.GetComponent<scr_cardsInDeck>();
                tempCard.cardData = selectedDeck.productionPlant;
                tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
                tempCard.deckList = this.gameObject;
                tempCard.SetCardTXT();
            }

            foreach (scr_card card in selectedDeck.cardsInDeck)
            {
                card.deckCount = 0;
            }

            foreach (scr_card card in selectedDeck.cardsInDeck)
            {
                card.deckCount++;

                if (card.deckCount <= 1)
                {
                    GameObject temp = Instantiate(cardPrefab, deckList.transform);
                    scr_cardsInDeck tempCard = temp.GetComponent<scr_cardsInDeck>();
                    tempCard.cardData = card;
                    tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
                    tempCard.deckList = this.gameObject;
                    tempCard.SetCardTXT();
                }
                else
                {
                    foreach(scr_cardsInDeck cardPrefab in this.gameObject.GetComponentsInChildren<scr_cardsInDeck>())
                    {
                        if(cardPrefab.cardData == card)
                        {
                            cardPrefab.SetCardTXT();
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("No deck was selected to be edited.");
        }
    }

    public void HideDeck()
    {
        foreach (Transform child in deckList.transform)
        {
            if (!child.tag.Equals("PleaseDontDestroy"))
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void FinishEdit()
    {
        scr_dataPersistenceManager.instance.SaveGame();
        HideDeck();
        menuManager.UnlockMenu();
        this.gameObject.SetActive(false);
    }
}
