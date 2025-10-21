using System.Collections.Generic;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UIElements;
using UnityEditor.UI;

public class scr_cards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, scr_IDataPersistence
{
    public GameObject GUI, details, temp;
    public UnityEngine.UI.Image typeRarity;
    public TextMeshProUGUI nameTXT, costTXT, typeTXT, descTXT;
    public scr_card cardData;
    bool detailsON;
    Vector3 detailPosDelta = new Vector3(135, 5, 0);

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
                    SetCardTXT();
                    Debug.Log(card.name + " card data loaded.");
                    return;
                }
            }
        }
    }

    public void SetCardTXT()
    {
        nameTXT.text = cardData.name;
        costTXT.text = cardData.cost.ToString();
        typeTXT.text = cardData.unit.tag;
        descTXT.text = cardData.description.ToString();
        typeRarity.sprite = cardData.typeRarity;
    }
    
    public void SaveData(ref scr_playerData data)
    {
        cardData.loaded = false;
        cardData.card_id = data.cardsCollected.Count;
        //data.cardCount.Add(data.cardCount.Count);
        if(cardData.count <= 0)
        {
            data.cardsCollected.Add(cardData);
            
        }
        cardData.count++;
        scr_dataPersistenceManager.instance.SaveGame();
        Debug.Log("Added card: " +  cardData.name + " to collection.");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (this.transform.position.x < (GUI.GetComponentInParent<Camera>().scaledPixelWidth / 2))
        {
            temp = Instantiate(details, this.GetComponent<RectTransform>().position + detailPosDelta - new Vector3(0, 0, -100), Quaternion.identity, this.transform.parent.parent);
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(100, 0, 5);
        }
        else
        {
            temp = Instantiate(details, this.GetComponent<RectTransform>().position - detailPosDelta - new Vector3(0, 0, -100), Quaternion.identity, this.transform.parent.parent);
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(-100, 0, 5);
        }
        detailsON = temp.GetComponent<scr_cardDetails>().HoveredOver(this.gameObject);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        detailsON = false;
        if (temp != null || !detailsON)
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
