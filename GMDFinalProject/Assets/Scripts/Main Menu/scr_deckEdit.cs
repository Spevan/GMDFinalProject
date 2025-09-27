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
    public GridLayoutGroup deckList;
    public TextMeshProUGUI deckName;

    private void OnEnable()
    {
        ChangeDisplayName();
        DisplayDeck();
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
                scr_cardsInMenu tempCard = temp.GetComponent<scr_cardsInMenu>();
                tempCard.cardData = selectedDeck.productionPlant;
                tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
                tempCard.deckList = this.gameObject;
            }

            foreach (scr_card card in selectedDeck.cardsInDeck)
            {
                GameObject temp = Instantiate(cardPrefab, deckList.transform);
                scr_cardsInMenu tempCard = temp.GetComponent<scr_cardsInMenu>();
                tempCard.cardData = card;
                tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
                tempCard.deckList = this.gameObject;
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
