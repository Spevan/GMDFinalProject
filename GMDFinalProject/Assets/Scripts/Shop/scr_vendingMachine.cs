using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;

public class scr_vendingMachine : MonoBehaviour
{
    public scr_card[] cardsPossible;
    [SerializeField] GameObject cardPrefab, canvas;

    void Start()
    {
        canvas = GameObject.Find("gui_canvas");
    }

    public void Roll()
    {
        scr_card tempCard = cardsPossible[Random.Range(0, cardsPossible.Length)];
        GameObject temp = Instantiate(cardPrefab, this.transform.parent.parent);
        temp.GetComponent<scr_cardsInMenu>().cardData = tempCard;
        temp.GetComponent<scr_cardsInMenu>().GUI = canvas;
        temp.GetComponent<scr_cardsInMenu>().SetCardTXT();
        temp.GetComponent<scr_cardsInMenu>().SaveData(ref scr_dataPersistenceManager.instance.playerData);
    }
}
