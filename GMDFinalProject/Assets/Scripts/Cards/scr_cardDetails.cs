using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardDetails : MonoBehaviour, IPointerExitHandler
{
    public scr_card cardData;
    [SerializeField] TextMeshProUGUI health, power, cooldown, speed, range;
    [SerializeField] Transform statusTransform;
    public GameObject statusPrefab, cardOrigin;
    public bool detailLock, cardLock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cardData.unit.tag.Equals("Hero"))
        {
            speed.text = (cardData as scr_hero).speed.ToString();
            if(cardData is scr_rangeHero)
            {
                range.text = (cardData as scr_rangeHero).range.ToString();
            }
            else
            {
                range.text = 0.ToString();
            }
        }
        else if(cardData.unit.tag.Equals("Tower"))
        {
            range.text = (cardData as scr_tower).range.ToString();
            if(cardData is scr_vehicle)
            {
                speed.text = (cardData as scr_vehicle).speed.ToString();
            }
            else
            {
                speed.text = 0.ToString();
            }
        }
        health.text = cardData.health.ToString();
        power.text = cardData.cost.ToString();
        if(cardData.maxCooldown <= 0 || cardData.power <= 0)
        {
            cooldown.text = 0.ToString();
        }
        else
        {
            cooldown.text = (cardData.power / cardData.maxCooldown).ToString();
        }

        foreach(scr_status status in cardData.statuses)
        {
            GameObject temp = Instantiate(statusPrefab, statusTransform);
            temp.GetComponent<scr_statusInMenu>().SetStatus(status);
        }
        detailLock = false;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        Destroy(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
