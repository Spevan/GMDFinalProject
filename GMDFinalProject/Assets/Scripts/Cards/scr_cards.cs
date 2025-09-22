using System.Collections.Generic;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class scr_cards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, scr_IDataPersistence
{
    public GameObject GUI, details, temp;
    public scr_card cardData;

    public void LoadData(scr_playerData gameData)
    {
        foreach( scr_card card in gameData.cardsCollected )
        {
            for(int i = 0; i < gameData.cardsCollected.Count; i++)
            {
                Debug.Log(gameData.cardsCollected[i].name + " loaded: " + gameData.cardsCollected[i].loaded);
                if (card.card_id == i && !card.loaded)
                {
                    cardData = card;
                    card.loaded = true;
                    Debug.Log(card.name + " card data loaded.");
                    return;
                }
            }
        }
    }
    
    public void SaveData(ref scr_playerData data)
    {
        cardData.loaded = false;
        cardData.card_id = data.cardsCollected.Count;
        //data.cardCount.Add(data.cardCount.Count);
        data.cardsCollected.Add(cardData);
        scr_dataPersistenceManager.instance.SaveGame();
        Debug.Log("Added card: " +  cardData.name + " to collection.");
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

    private void OnDestroy()
    {
        cardData.loaded = false;
    }

    private void OnApplicationQuit()
    {
        OnDestroy();
    }
}
