using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInMenu : MonoBehaviour, IDragHandler, IDropHandler
{
    public GameObject GUI;
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

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GUI.GetComponent<Canvas>().transform as RectTransform,
            Input.mousePosition, GUI.GetComponent<Canvas>().worldCamera, out pos);
        transform.position = GUI.GetComponent<Canvas>().transform.TransformPoint(pos);
    }

    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        Ray ray = GUI.GetComponentInChildren<Camera>().ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.point);
            Debug.DrawRay(Input.mousePosition, GUI.GetComponentInChildren<Camera>().transform.forward, Color.red, Mathf.Infinity);
            Debug.Log("Card played: " + cardData.name);

            //Use raycast to detect collision onto deck builder
        }
    }
}
