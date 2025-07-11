using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInMenu : MonoBehaviour, IDragHandler, IDropHandler
{
    public TextMeshProUGUI[] cost;
    public TextMeshProUGUI health;
    public scr_card cardData;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public virtual void Start()
    {
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        for (int i = 0; i < cardData.cost.Length; i++)
        {
            cost[i].text = cardData.cost[i].ToString();
        }
        health.text = cardData.health.ToString();
    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {

    }
}
