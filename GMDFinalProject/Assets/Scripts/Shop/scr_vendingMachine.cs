using NUnit.Framework;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class scr_vendingMachine : MonoBehaviour
{
    public scr_card[] cardsPossible;
    [SerializeField] GameObject packPrefab, cardPrefab, canvas;

    void Start()
    {
        canvas = GameObject.Find("gui_canvas");
    }

    public void Roll()
    {
        GameObject pack = Instantiate(packPrefab, canvas.transform);

        scr_card tempCard = cardsPossible[Random.Range(0, cardsPossible.Length)];
        GameObject temp = Instantiate(cardPrefab, pack.GetComponentInChildren<HorizontalLayoutGroup>().transform);
        temp.GetComponent<scr_cardsInMenu>().cardData = tempCard;
        temp.GetComponent<scr_cardsInMenu>().GUI = canvas;
        temp.GetComponent<scr_cardsInMenu>().SetCardTXT();
        temp.GetComponent<scr_cardsInMenu>().SaveData(ref scr_dataPersistenceManager.instance.playerData);
    }
}
