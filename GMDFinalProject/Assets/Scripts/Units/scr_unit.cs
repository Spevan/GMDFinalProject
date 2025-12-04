using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.VisualScripting;
using UnityEngine;

public class scr_unit : NetworkBehaviour
{
    public scr_card cardData;
    public SphereCollider range;
    public NetworkRigidbody rb;
    public float timer, cooldown, power, health;
    public List<scr_status> statuses; public List<scr_condition> conditions;
    public GameObject target;

    public virtual void Start()
    {
        if(cardData == null)
        {
            cardData = ScriptableObject.CreateInstance<scr_card>();
        }
        rb = GetComponent<NetworkRigidbody>();
        conditions = new List<scr_condition>();
        range = this.AddComponent<SphereCollider>();
        SetStatuses();
    }

    public virtual void Attack()
    {
        Debug.Log(this.cardData.name + " dealt " + power + " damage to " + target.name);
        target.GetComponent<scr_unit>().ChangeHealth(Convert.ToInt32(-power));
        foreach (scr_status status in statuses)
        {
            if (status.statusType == scr_status.statusTypes.Sleepy && (target.tag.Equals("Hero") || target.tag.Equals("Vehicle")))
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.exhausted, status.statusAmnt));
            }
            if (status.statusType == scr_status.statusTypes.Blinding)
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.blind, status.statusAmnt));
            }
            if (status.statusType == scr_status.statusTypes.Crushing)
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.weak, status.statusAmnt));
            }
            if (status.statusType == scr_status.statusTypes.Heated)
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.burnt, status.statusAmnt));
            }
            if (status.statusType == scr_status.statusTypes.Frigid)
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.frozen, status.statusAmnt));
            }
            if (status.statusType == scr_status.statusTypes.Tangled && (target.tag.Equals("Hero") || target.tag.Equals("Vehicle")))
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.entangled, status.statusAmnt));
            }
        }
    }

    public void ChangeHealth(float delta)
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

    public void GetStatus(scr_status newStatus)
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
            if (status.statusType == scr_status.statusTypes.Perceptive)
            {
                statuses.Add(status);
                range.radius += status.statusAmnt * status.rangePerLvl;
            }
            if (status.statusType == scr_status.statusTypes.Healing || status.statusType == scr_status.statusTypes.Frigid
                || status.statusType == scr_status.statusTypes.Heated || status.statusType == scr_status.statusTypes.Thief)
            {
                statuses.Add(status);
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if (condition.conditionType == scr_condition.conditionTypes.weak)
            {
                power -= condition.conditionAmnt * condition.powerPerLvl;
            }
            if(condition.conditionType == scr_condition.conditionTypes.brittle || condition.conditionType == scr_condition.conditionTypes.leaking)
            {
                health -= condition.conditionAmnt * condition.healthPerLvl;
            }
            if (condition.conditionType == scr_condition.conditionTypes.burnt)
            {
                StartCoroutine(DamageOverTime(condition.burnDuration * condition.conditionAmnt, condition.fireDmgTick, condition.conditionAmnt));
            }
            if(condition.conditionType == scr_condition.conditionTypes.frozen)
            {
                cooldown += condition.conditionAmnt * condition.cooldownPerLevel;
            }
            if (condition.conditionType == scr_condition.conditionTypes.blind)
            {
                range.radius -= condition.conditionAmnt * condition.rangePerLvl;
            }
        }
    }

    //Under reconsideration
    /*public void GetLeaking(int waterAmnt)
    {
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
            {
                player.GetComponent<scr_player>().ChangeWater(waterAmnt);
            }
        }
    }*/

    public IEnumerator DamageOverTime(float duration, float tickTime, int damage)
    {
        float timer = 0f;
        while(timer < duration)
        {
            ChangeHealth(damage);
            yield return new WaitForSeconds(tickTime);
            timer += tickTime;
        }
    }

    public void AddCondition(scr_condition condition)
    {
        conditions.Add(condition);
        SetStatuses();
    }

    public void AddCondition(scr_condition condition, scr_unit origin)
    {
        conditions.Add(condition);
        SetStatuses();
    }

    public void GetTarget(GameObject newTarget)
    {
        target = newTarget;
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

        foreach(GameObject player in scr_gameManager.instance.players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId != this.gameObject.GetComponent<NetworkObject>().OwnerClientId)
            {
                if (NetworkManager.IsServer)
                {
                    player.GetComponent<scr_player>().AddKillCount();
                }
                else
                {
                    player.GetComponent<scr_player>().AddKillCountServerRpc();
                }
            }
        }

        this.GetComponent<BoxCollider>().enabled = false;
        if (gameObject.transform.childCount > 0)
        {
            this.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
        //this.gameObject.SetActive(false);
    }
}
