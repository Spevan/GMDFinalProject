using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Components;
using Unity.Services.Lobbies.Models;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_unit : NetworkBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public scr_card cardData;
    public SphereCollider range;
    public NetworkRigidbody rb;
    public float timer, cooldown, power, health;
    public bool attackLock;
    public List<scr_status> statuses; public List<scr_condition> conditions;
    public GameObject target, details, temp, owner;

    public virtual void Start()
    {
        if (cardData == null)
        {
            cardData = ScriptableObject.CreateInstance<scr_card>();
        }
        rb = GetComponent<NetworkRigidbody>();
        conditions = new List<scr_condition>();
        range = this.AddComponent<SphereCollider>();
        attackLock = false;
        SetDefaultStats();

        foreach (GameObject player in scr_gameManager.instance.players)
        {
            if (player.GetComponent<NetworkObject>().OwnerClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId)
            {
                owner = player;
            }
        }
    }

    public virtual void Attack()
    {
            foreach (scr_status status in statuses)
            {
                if (status.statusType == scr_status.statusTypes.Healing && target.tag.Equals("Hero"))
                {
                    target.GetComponent<scr_unit>().ChangeHealth(power * (int)status.statusAmnt);
                    return;
                }
                if (status.statusType == scr_status.statusTypes.Miraculous && target.tag.Equals("Hero"))
                {
                    target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.resurrected, status.statusAmnt));
                    return;
                }
                if (status.statusType == scr_status.statusTypes.Vampiric && target.tag.Equals("Hero"))
                {
                    ChangeHealth(Convert.ToInt32(power * (0.1 * status.statusAmnt)));
                }
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
                if (status.statusType == scr_status.statusTypes.Paralyzer)
                {
                    target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.stunned, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Pacifist)
                {
                    target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.compromised, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Conspiracist)
                {
                    target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.mutinied, status.statusAmnt));
                    target.GetComponent<scr_unit>().Mutinied(gameObject.GetComponent<NetworkObject>().OwnerClientId, 5f);
                }
            }
            Debug.Log(this.cardData.name + " dealt " + power + " damage to " + target.name);
            target.GetComponent<scr_unit>().ChangeHealth(Convert.ToInt32(-power));
    }

    public virtual void ChangeHealth(float delta)
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
                return;
            }
            else if (count == statuses.Count)
            {
                statuses.Add(newStatus);
                return;
            }
        }

    }

    public virtual void SetDefaultStats()
    {
        power = cardData.power;
        cooldown = cardData.maxCooldown;
        health = cardData.health;
        range.radius = cardData.range;
        range.isTrigger = true;
        SetStatuses();
    }

    public virtual void SetStatuses()
    {
        foreach (scr_status status in cardData.statuses)
        {
            statuses.Add(status);
            switch (status.statusType)
            {
                case scr_status.statusTypes.Strong:
                    power += status.statusAmnt * status.powerPerLvl;
                    break;
                case scr_status.statusTypes.Fortified:
                    health += status.statusAmnt * status.healthPerLvl;
                    break;
                case scr_status.statusTypes.Perceptive:
                    range.radius += status.statusAmnt * status.rangePerLvl;
                    break;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            switch (condition.conditionType)
            {
                case scr_condition.conditionTypes.weak:
                    power -= condition.conditionAmnt * condition.powerPerLvl;
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.conditionTimer));
                    break;
                case scr_condition.conditionTypes.brittle:
                case scr_condition.conditionTypes.leaking:
                    health -= condition.conditionAmnt * condition.healthPerLvl;
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.conditionTimer));
                    break;
                case scr_condition.conditionTypes.burnt:
                    StartCoroutine(DamageOverTime(condition.burnDuration * condition.conditionAmnt, condition.fireDmgTick, condition.conditionAmnt));
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.burnDuration * condition.conditionAmnt));
                    break;
                case scr_condition.conditionTypes.frozen:
                    cooldown += condition.conditionAmnt * condition.cooldownPerLevel;
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.conditionTimer));
                    break;
                case scr_condition.conditionTypes.blind:
                    range.radius -= condition.conditionAmnt * condition.rangePerLvl;
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.conditionTimer));
                    break;
                case scr_condition.conditionTypes.stunned:
                case scr_condition.conditionTypes.grappled:
                case scr_condition.conditionTypes.compromised:
                    StartCoroutine(Stunned(condition.entangledDuration));
                    StartCoroutine(RemoveConditionOnTimer(condition, condition.entangledDuration));
                    break;
                case scr_condition.conditionTypes.mutinied:
                    break;
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

    public IEnumerator Stunned(int duration)
    {
        attackLock = true;
        yield return new WaitForSeconds(duration);
        attackLock = false;
    }

    public IEnumerator DamageOverTime(float duration, float tickTime, int damage)
    {
        float timer = 0f;
        while (timer < duration)
        {
            ChangeHealth(damage);
            yield return new WaitForSeconds(tickTime);
            timer += tickTime;
        }
    }

    public IEnumerator Mutinied(ulong newOwner, float duration)
    {
        ulong previousOwner = gameObject.GetComponent<NetworkObject>().OwnerClientId;
        gameObject.GetComponent<NetworkObject>().ChangeOwnership(newOwner);
        rb.SetRotation(Quaternion.Inverse(gameObject.transform.rotation));
        yield return new WaitForSeconds(duration);
        gameObject.GetComponent<NetworkObject>().ChangeOwnership(previousOwner);
        rb.SetRotation(Quaternion.Inverse(gameObject.transform.rotation));
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

    public void RemoveCondition(scr_condition condition)
    {
        conditions.Remove(condition);
        SetStatuses();
    }

    public IEnumerator RemoveConditionOnTimer(scr_condition condition, float duration)
    {
        yield return new WaitForSeconds(duration);
        RemoveCondition(condition);
    }

    public void GetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Health: " + health + ", CD: " + cooldown + ", Power: " + power + ", Range: " + range.radius + ", Speed:" + 0);
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().DisplayCardDetails(cardData, health, cooldown, power, range.radius, 0);
        }
        /* spawnPos;
        if (this.transform.position.x < (owner.GetComponent<Camera>().scaledPixelWidth / 2))
        {
            spawnPos = owner.GetComponent<Camera>().WorldToScreenPoint(new Vector3
                (gameObject.transform.position.x + gameObject.GetComponentInChildren<SpriteRenderer>().size.x, gameObject.transform.position.y, gameObject.transform.position.z));
            //, this.transform.parent.parent.rotation, this.transform.parent.parent);
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(100, 0, 5);
        }
        else
        {
            spawnPos = owner.GetComponent<Camera>().WorldToScreenPoint(new Vector3
                (gameObject.transform.position.x - gameObject.GetComponentInChildren<RectTransform>().rect.width, gameObject.transform.position.y, gameObject.transform.position.z));
            //temp.transform.localPosition = this.transform.localPosition + new Vector3(-100, 0, 5);
        }
        temp = Instantiate(details, gameObject.transform.position, Quaternion.identity, owner.GetComponentInChildren<Canvas>().transform);
        //temp.GetComponent<scr_cardDetails>().LockCard(true);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;*/
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().HideCardDetails();
        }
        /*if (temp != null)
        {
            Destroy(temp);
        }*/

        //temp.GetComponent<scr_cardDetails>().LockCard(false);
    }

    public virtual void Death()
    {
        foreach(GameObject player in scr_gameManager.instance.players)
        {
            foreach (scr_status status in statuses)
            {
                if (player.GetComponent<NetworkObject>().OwnerClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId &&
                    status.statusType == scr_status.statusTypes.Recyclable)
                {
                    player.GetComponent<scr_player>().ChangeWater(Convert.ToInt32((float)cardData.cost * ((float)status.statusAmnt * 0.25f)));
                }
            }

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
        this.gameObject.SetActive(false);
    }
}
