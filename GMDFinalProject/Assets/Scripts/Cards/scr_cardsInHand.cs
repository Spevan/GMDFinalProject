using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInHand : MonoBehaviour, IDragHandler, IDropHandler
{
    public TextMeshProUGUI[] cost;
    public TextMeshProUGUI health;
    public scr_card cardData;
    public scr_guiManager GUI;
    public scr_player player;
    public Camera playerCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //GUI = GameObject.Find("gui_canvas").GetComponent<scr_guiManager>();
        for(int i = 0; i < cardData.cost.Length; i++)
        {
            cost[i].text = cardData.cost[i].ToString();
        }
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
            Debug.Log("Card played: " + cardData.name);

            player.PlayCard(cardData);

            if (NetworkManager.Singleton.IsServer)
            {
                scr_gameManager.instance.SpawnNetworkCard(cardData.name, hit.point, new Quaternion(0, player.transform.rotation.y, 0, 0), player.GetComponent<NetworkObject>().OwnerClientId);
            }
            else
            {
                scr_gameManager.instance.SpawnNetworkCardServerRpc(cardData.name, hit.point, new Quaternion(0, player.transform.rotation.y, 0, 0), player.GetComponent<NetworkObject>().OwnerClientId);
            }
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