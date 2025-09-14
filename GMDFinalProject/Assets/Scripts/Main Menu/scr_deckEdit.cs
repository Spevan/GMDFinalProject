using UnityEngine;
using UnityEngine.UI;

public class scr_deckEdit : MonoBehaviour
{
    public GameObject cardPrefab;
    bool isVisible = true;
    Vector3 moveDistance = new Vector3(160, 0, 0);
    public scr_deck selectedDeck; public scr_mainMenuManager menuManager;
    public VerticalLayoutGroup deckList;

    private void OnEnable()
    {
        DisplayDeck();
    }

    private void OnDisable()
    {
        HideDeck();
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
        HideDeck();
        menuManager.UnlockMenu();
        this.gameObject.SetActive(false);
    }
}
