using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;

    public Rigidbody rb;
    public float timer, cooldown, power, health;
    public GameObject target;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        
        foreach(scr_card.status status in cardData.statuses)
        {
            
            if(status.statusType == scr_card.status.statusTypes.strong)
            {
                power += status.statusAmnt;
            }
            if(status.statusType == scr_card.status.statusTypes.fortified)
            {
                health += status.statusAmnt;
            }
        }

        power += cardData.power;
        cooldown += cardData.maxCooldown;
        health += cardData.health;
    }

    public void ChangeHealth(int delta)
    {
        health += delta;

        if (health <= 0)
        {
            Death();
        }
        else if (health > cardData.health)
        {
            health = cardData.health;
        }
    }

    public virtual void Death()
    {
        this.gameObject.SetActive(false);
    }
}
