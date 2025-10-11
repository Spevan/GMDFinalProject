using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;

    public Rigidbody rb;
    public float timer, cooldown, power, health;
    public List<scr_status> statuses;
    public GameObject target;

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetStatuses();
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

    public void GetSupport(scr_status newStatus)
    {
        int count = 0;
        foreach (scr_status oldStatus in statuses)
        {
            count++;
            if (oldStatus.statusType == newStatus.statusType)
            {
                oldStatus.statusAmnt++;
            }
            else if(count == statuses.Count)
            {
                statuses.Add(newStatus);
            }
        }
        SetStatuses();
    }

    public virtual void SetDefaultStats()
    {
        power = cardData.power;
        cooldown = cardData.maxCooldown;
        health = cardData.health;
    }

    public virtual void SetStatuses()
    {
        SetDefaultStats();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.strong)
            {
                statuses.Add(status);
                power += status.statusAmnt;
            }
            if (status.statusType == scr_status.statusTypes.fortified)
            {
                statuses.Add(status);
                health += status.statusAmnt;
            }
        }
    }

    public virtual void Death()
    {
        foreach (scr_status status in statuses)
        {
            if(status.statusType == scr_status.statusTypes.recyclable)
            {
                scr_player.instance.ChangeWater(Convert.ToInt32((float)cardData.cost * ((float)status.statusAmnt * 0.5f)));
            }
        }
        this.gameObject.SetActive(false);
    }
}
