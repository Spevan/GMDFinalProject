using TMPro;
using UnityEngine;

public class scr_cardDetails : MonoBehaviour
{
    public scr_card cardData;
    [SerializeField] TextMeshProUGUI health, power, cooldown, dps, speed, range;
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
        cooldown.text = cardData.maxCooldown.ToString();
        dps.text = (cardData.power / cardData.maxCooldown).ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
