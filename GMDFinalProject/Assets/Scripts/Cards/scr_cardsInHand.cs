using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInHand : MonoBehaviour, IPointerMoveHandler
{
    public TextMeshProUGUI cost, health;
    public scr_card cardData;
    public scr_guiManager GUI;
    public scr_player player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        cost.text = cardData.cost.ToString();
        health.text = cardData.health.ToString();
    }

    void IPointerMoveHandler.OnPointerMove(PointerEventData eventData)
    {
        Debug.Log("mouse over card " + gameObject.name);
        if (Input.GetMouseButton(0))
        {
            //transform.position = Input.mousePosition;
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(GUI.gameObject.GetComponent<Canvas>().transform as RectTransform, Input.mousePosition, GUI.gameObject.GetComponent<Canvas>().worldCamera, out pos);
            transform.position = GUI.gameObject.GetComponent<Canvas>().transform.TransformPoint(pos);
            //if (transform.position.y > 100)
            //{
                player.PlayCard(cardData);
                GUI.RemoveCard(player, gameObject);
                GUI.UpdateHand();
                Destroy(gameObject);
            //}
        }
        else
        {
            GUI.UpdateHand();
        }
    }
}