using Unity.Services.Lobbies.Models;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class scr_cards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, scr_IDataPersistence
{
    public bool loaded;
    public GameObject GUI, details, temp;
    public scr_card cardData;

    void scr_IDataPersistence.LoadData(scr_playerData gameData)
    {
        foreach( scr_card card in gameData.cards )
        {
            if(!card.loaded)
            {
                card.loaded = true;
                cardData = card;
            }
        }
        
    }

    void scr_IDataPersistence.SaveData(ref scr_playerData data)
    {
        cardData.loaded = false;
        data.cards.Add(cardData);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        temp = Instantiate(details, GUI.transform);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
        if (this.transform.localPosition.x < (GUI.GetComponentInParent<Camera>().scaledPixelWidth / 2))
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(100, 0, 5);
        }
        else
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(-100, 0, 5);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (temp != null)
        {
            Destroy(temp);
        }
    }
}
