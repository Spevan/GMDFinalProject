using Unity.Netcode;
using UnityEngine;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;

    public SphereCollider range;
    public float timer, cooldown, power, health;
    public GameObject target;

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

    

    void Death()
    {
        this.gameObject.SetActive(false);
    }
}
