using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_pack : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject options;
    GameObject canvas;
    public scr_packs packData;
    public GameObject packPrefab, cardPrefab;
    public List<scr_card> cards;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        canvas = GameObject.Find("gui_canvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        options.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        options.gameObject.SetActive(false);
    }

    public void BuyPack()
    {
        if(scr_dataPersistenceManager.instance.playerData.currency >= packData.packPrice)
        {
            scr_dataPersistenceManager.instance.playerData.ChangeCurrnecy(-packData.packPrice);
        }
    }

    public void OpenPack()
    {
        for(int i = 0; i < packData.cardsNum; i++)
        {
            int randomNum = Random.Range(0, 99);
            if(randomNum >= 0 && randomNum <= 63)
            {
                cards.Add(packData.commonsPossible[Random.Range(0, packData.commonsPossible.Length)]);
            }
            else if(randomNum >= 64 && randomNum <= 87)
            {
                cards.Add(packData.uncommonsPossible[Random.Range(0, packData.uncommonsPossible.Length)]);
            }
            else if(randomNum >= 88 && randomNum <= 95)
            {
                cards.Add(packData.raresPossible[Random.Range(0, packData.raresPossible.Length)]);
            }
            else if(randomNum >= 96)
            {
                cards.Add(packData.legendariesPossible[Random.Range(0, packData.legendariesPossible.Length)]);
            }
        }

        GameObject packReveal = Instantiate(packPrefab, canvas.transform);
        foreach(scr_card card in cards)
        {
            GameObject tempCard = Instantiate(cardPrefab, packReveal.GetComponentInChildren<HorizontalLayoutGroup>().transform);
            tempCard.GetComponent<scr_cardsInMenu>().cardData = card;
            tempCard.GetComponent<scr_cardsInMenu>().GUI = canvas;
            tempCard.GetComponent<scr_cardsInMenu>().SetCardTXT();
            tempCard.GetComponent<scr_cardsInMenu>().SaveData(ref scr_dataPersistenceManager.instance.playerData);
        }
        cards.Clear();
    }
}
