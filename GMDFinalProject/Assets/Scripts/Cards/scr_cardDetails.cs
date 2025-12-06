using TMPro;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
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
    public void Start()
    {
        RangeAndSpeedCheck();
        HealthCheck();
        //PowerCheck();
        CooldownCheck();

        foreach(scr_status status in cardData.statuses)
        {
            GameObject temp = Instantiate(statusPrefab, statusTransform);
            temp.GetComponent<scr_statusInMenu>().SetStatus(status);
        }
    }

    /*private void OnEnable()
    {
        Start();
    }*/

    public void DetailUpdate(float currentHealth, float currentCooldown, float currentPower, float currentRange, float currentSpeed)
    {
        RangeAndSpeedCheck(currentRange, currentSpeed);
        HealthCheck(currentHealth);
        //PowerCheck(currentPower);
        CooldownCheck(currentPower, currentCooldown);
    }

    void RangeAndSpeedCheck()
    {
        if (cardData.unit.tag.Equals("Hero"))
        {
            speed.text = (cardData as scr_hero).speed.ToString();
            range.text = cardData.range.ToString();
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
        else
        {
            speed.text = 0.ToString();
            range.text = cardData.range.ToString();
        }
    }

    public void RangeAndSpeedCheck(float currentRange, float currentSpeed)
    {
        if (cardData.unit.tag.Equals("Hero"))
        {
            speed.text = currentSpeed.ToString();
            range.text = currentRange.ToString();
        }
        else if (cardData.unit.tag.Equals("Tower"))
        {
            range.text = currentRange.ToString();
            if (cardData is scr_vehicle)
            {
                speed.text = currentSpeed.ToString();
            }
            else
            {
                speed.text = 0.ToString();
            }
        }
        else
        {
            speed.text = 0.ToString();
            range.text = cardData.range.ToString();
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

    void HealthCheck(float currentHealth)
    {

        /*if (currentHealth >= 1000000)
        {
            health.text = ((double)currentHealth / 1000000).ToString() + "M";
        }
        else if (currentHealth >= 1000)
        {
            health.text = ((double)currentHealth / 1000).ToString() + "K";
        }
        else
        {*/
            health.text = currentHealth.ToString();
        //}
    }

    void PowerCheck()
    {
        power.text = cardData.cost.ToString();
    }

    public void PowerCheck(float currentPower)
    {
        power.text = currentPower.ToString();
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

    public void CooldownCheck(float currentPower, float currentCooldown)
    {
        if (currentCooldown <= 0 || currentPower <= 0)
        {
            cooldown.text = 0.ToString();
        }
        else
        {
            cooldown.text = (currentPower / currentCooldown).ToString();
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
