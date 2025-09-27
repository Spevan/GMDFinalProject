using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;

    public SphereCollider range;
    public float timer, cooldown, power, health;
    public GameObject target;

    public virtual void Start()
    {
        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
        range.isTrigger = true;

        power = cardData.power;
        cooldown = cardData.maxCooldown;
        health = cardData.health;
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
