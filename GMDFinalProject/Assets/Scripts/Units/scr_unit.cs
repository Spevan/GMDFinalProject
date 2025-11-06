using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;
    public SphereCollider range;
    public Rigidbody rb;
    public float timer, cooldown, power, health;
    public List<scr_status> statuses; public List<scr_condition> conditions;
    public GameObject target;
    

    public virtual void Start()
    {
        rb = GetComponent<Rigidbody>();
        conditions = new List<scr_condition>();
        range = this.AddComponent<SphereCollider>();
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
        range.radius = cardData.range;
        range.isTrigger = true;
    }

    public virtual void SetStatuses()
    {
        SetDefaultStats();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.Strong)
            {
                statuses.Add(status);
                power += status.statusAmnt * status.powerPerLvl;
            }
            if (status.statusType == scr_status.statusTypes.Fortified)
            {
                statuses.Add(status);
                health += status.statusAmnt * status.healthPerLvl;
            }
            if(status.statusType == scr_status.statusTypes.Healing || status.statusType == scr_status.statusTypes.Frigid
                || status.statusType == scr_status.statusTypes.Heated)
            {
                statuses.Add(status);
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if (condition.conditionType == scr_condition.conditionTypes.weak)
            {
                power -= condition.conditonAmnt * condition.powerPerLvl;
            }
            if(condition.conditionType == scr_condition.conditionTypes.brittle)
            {
                health -= condition.conditonAmnt * condition.healthPerLvl;
            }
            if (condition.conditionType == scr_condition.conditionTypes.burnt)
            {
                StartCoroutine(Burning(1, condition.conditonAmnt));
            }
            if(condition.conditionType == scr_condition.conditionTypes.frozen)
            {
                cooldown += condition.conditonAmnt * condition.cooldownPerLevel;
            }
        }
    }

    IEnumerator Burning(float waitTime, int burnDMG)
    {
        ChangeHealth(burnDMG);
        yield return new WaitForSeconds(waitTime);
    }

    public void AddCondition(scr_condition condition)
    {
        conditions.Add(condition);
        SetStatuses();
    }

    public virtual void Death()
    {
        foreach (scr_status status in statuses)
        {
            if(status.statusType == scr_status.statusTypes.Recyclable)
            {
                scr_player.instance.ChangeWater(Convert.ToInt32((float)cardData.cost * ((float)status.statusAmnt * 0.5f)));
            }
        }
        this.gameObject.SetActive(false);
    }
}
