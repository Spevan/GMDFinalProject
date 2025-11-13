using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInDeck : scr_cards
{
    scr_mainMenuManager menuManager;
    int deckCount;
    public GameObject deckEditBtns, deckList;
    public TextMeshProUGUI countTXT;

    private void Start()
    {
        menuManager = GameObject.Find("gui_mainMenuButtons").GetComponent<scr_mainMenuManager>();
    }

    public override void SetCardTXT()
    {
        nameTXT.text = "[" + cardData.name + "]";
        deckCount++;
        countTXT.text = "X" + deckCount.ToString();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

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

    public void RemoveCard()
    {
        scr_deckEdit editList = deckList.GetComponent<scr_deckEdit>();
        //scr_deck tempDeck = Resources.Load<scr_deck>(deckPath + editList.selectedDeck.name + ".asset");
        if (editList != null)
        {
            OnPointerExit(null);
            editList.selectedDeck.cardsInDeck.Remove(cardData);
            deckCount--;
            //Resources.UnloadAsset(tempDeck);
            editList.HideDeck();
            editList.DisplayDeck();
            if(deckCount <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
