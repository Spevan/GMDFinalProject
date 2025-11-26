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
                StartCoroutine(Burning(1, condition.conditionAmnt));
            }
            if(condition.conditionType == scr_condition.conditionTypes.frozen)
            {
                cooldown += condition.conditionAmnt * condition.cooldownPerLevel;
            }
        }
    }

    public void GetLeaking(int waterAmnt)
    {
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
            {
                player.GetComponent<scr_player>().ChangeWater(waterAmnt);
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

        this.gameObject.SetActive(false);
    }
}
