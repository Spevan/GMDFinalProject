using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardsInHand : scr_cardsInMenu, IDragHandler, IDropHandler
{
    public scr_player player;
    public Camera playerCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        playerCam = player.GetComponent<Camera>();
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(playerCam.GetComponentInChildren<Canvas>().transform as RectTransform,
            Input.mousePosition, playerCam.GetComponentInChildren<Canvas>().worldCamera, out pos);
        transform.position =playerCam.GetComponentInChildren<Canvas>().transform.TransformPoint(pos);
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
            GUI.GetComponent<scr_guiManager>().RemoveCard(player, gameObject);
            GUI.GetComponent<scr_guiManager>().UpdateHand();
            Destroy(gameObject);
        }
        else
        {
            GUI.GetComponent<scr_guiManager>().UpdateHand();
        }
    }
}