using TMPro;
using Unity.Services.Lobbies.Models;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cardDetails : MonoBehaviour
{
    public scr_card cardData;
    [SerializeField] TextMeshProUGUI health, power, cooldown, speed, range;
    [SerializeField] Transform statusTransform;
    public GameObject statusPrefab, cardOrigin;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RangeAndSpeedCheck();
        HealthCheck();
        PowerCheck();
        CooldownCheck();

        foreach(scr_status status in cardData.statuses)
        {
            GameObject temp = Instantiate(statusPrefab, statusTransform);
            temp.GetComponent<scr_statusInMenu>().SetStatus(status);
        }
    }

    void RangeAndSpeedCheck()
    {
        if (cardData.unit.tag.Equals("Hero"))
        {
            speed.text = (cardData as scr_hero).speed.ToString();
            if (cardData is scr_rangeHero)
            {
                range.text = (cardData as scr_rangeHero).range.ToString();
            }
            else
            {
                range.text = 0.ToString();
            }
        }
        else if (cardData.unit.tag.Equals("Tower"))
        {
            range.text = (cardData as scr_tower).range.ToString();
            if (cardData is scr_vehicle)
            {
                speed.text = (cardData as scr_vehicle).speed.ToString();
            }
            else
            {
                speed.text = 0.ToString();
            }
        }
    }

    void HealthCheck()
    {
        if(cardData.health >= 1000000)
        {
            health.text = ((double)cardData.health / 1000000).ToString() + "M";
        }
        else if (cardData.health >= 1000)
        {
            health.text = ((double)cardData.health / 1000).ToString() + "K";
        }
        else
        {
            health.text = cardData.health.ToString();
        }    
    }

    void PowerCheck()
    {
        power.text = cardData.cost.ToString();
    }

    void CooldownCheck()
    {
        if (cardData.maxCooldown <= 0 || cardData.power <= 0)
        {
            cooldown.text = 0.ToString();
        }
        else
        {
            cooldown.text = (cardData.power / cardData.maxCooldown).ToString();
        }
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
