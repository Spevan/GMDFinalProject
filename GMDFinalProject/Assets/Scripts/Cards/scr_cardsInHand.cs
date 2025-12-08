using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_cardsInHand : scr_cards, IDragHandler, IDropHandler
{
    public scr_player player;
    public Camera playerCam;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        playerCam = player.GetComponent<Camera>();
        ChangeTransparency(0.5f);
    }

    public void CardDrawn(scr_card thisCard, scr_player thisPlayer, GameObject thisGUI)
    {
        cardData = thisCard;
        player = thisPlayer;
        GUI = thisGUI;
        SetCardTXT();
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        ChangeTransparency(1f);

        if (this.transform.position.x < (GUI.GetComponentInParent<Camera>().scaledPixelWidth / 2))
        {
            Vector3 spawnPos = new Vector3(gameObject.transform.position.x + (gameObject.transform.right.x / 2), gameObject.transform.position.y, gameObject.transform.position.z);
            temp = Instantiate(details, spawnPos, this.transform.parent.parent.rotation, this.transform.parent.parent);
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(100, 0, 5);
        }
        else
        {
            Vector3 spawnPos = new Vector3(gameObject.transform.position.x - (gameObject.transform.right.x / 2), gameObject.transform.position.y, gameObject.transform.position.z);
            temp = Instantiate(details, spawnPos, this.transform.parent.parent.rotation, this.transform.parent.parent);
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(-100, 0, 5);
        }
        //temp.GetComponent<scr_cardDetails>().LockCard(true);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        ChangeTransparency(0.5f);
        base.OnPointerExit(eventData);
    }

    public void ChangeTransparency(float alphaAmnt)
    {
        Color currentColor = gameObject.GetComponent<Image>().color;
        currentColor.a = alphaAmnt;
        gameObject.GetComponent<Image>().color = currentColor;

        foreach (TextMeshProUGUI text in GetComponentsInChildren<TextMeshProUGUI>())
        {
            currentColor = text.color;
            currentColor.a = alphaAmnt;
            text.color = currentColor;
        }

        foreach(Image image in GetComponentsInChildren<Image>())
        {
            currentColor = image.color;
            currentColor.a = alphaAmnt;
            image.color = currentColor;
        }
    }

    void IDragHandler.OnDrag(UnityEngine.EventSystems.PointerEventData eventData)
    {
        foreach(scr_cardsInHand card in player.GetComponentInChildren<scr_guiManager>().hand.GetComponentsInChildren<scr_cardsInHand>())
        {
            if(!card.gameObject.Equals(gameObject))
            {
                card.ChangeTransparency(0f);
            }
            else
            {
                card.ChangeTransparency(1f);
            }
        }
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(playerCam.GetComponentInChildren<Canvas>().transform as RectTransform,
            Input.mousePosition, playerCam.GetComponentInChildren<Canvas>().worldCamera, out pos);
        transform.position = playerCam.GetComponentInChildren<Canvas>().transform.TransformPoint(pos);
    }

    void IDropHandler.OnDrop(UnityEngine.EventSystems.PointerEventData eventData)
    {
        foreach (scr_cardsInHand card in player.GetComponentInChildren<scr_guiManager>().hand.GetComponentsInChildren<scr_cardsInHand>())
        {
            card.ChangeTransparency(0.5f);
        }

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
        base.OnPointerExit(eventData);
    }
}