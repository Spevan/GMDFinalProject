using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInHand : MonoBehaviour, IDragHandler, IDropHandler
{
    public TextMeshProUGUI cost, health;
    public scr_card cardData;
    public scr_guiManager GUI;
    public scr_player player;
    public Camera playerCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        cost.text = cardData.cost.ToString();
        health.text = cardData.health.ToString();
        playerCam = player.GetComponent<Camera>();
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(GUI.gameObject.GetComponent<Canvas>().transform as RectTransform,
            Input.mousePosition, GUI.gameObject.GetComponent<Canvas>().worldCamera, out pos);
        transform.position = GUI.gameObject.GetComponent<Canvas>().transform.TransformPoint(pos);
    }

    void IDropHandler.OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Ray ray = playerCam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            Debug.Log(hit.point);
            Debug.DrawRay(Input.mousePosition, playerCam.transform.forward, Color.red, Mathf.Infinity);
            Debug.Log("Card played: " + name);
            player.PlayCard(cardData);
            GameObject unit = Instantiate(cardData.unit, hit.point, new Quaternion(0, player.transform.rotation.y, 0, 0));
            scr_gameManager.instance.SpawnNetworkObjServerRpc(unit, player);
            //unit.GetComponent<scr_heroUnit>().player = player;
            GUI.RemoveCard(player, gameObject);
            GUI.UpdateHand();
            Destroy(gameObject);
        }
        else
        {
            GUI.UpdateHand();
        }
    }
}