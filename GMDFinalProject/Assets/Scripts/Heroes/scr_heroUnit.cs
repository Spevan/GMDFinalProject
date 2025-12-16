using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_heroUnit : scr_unit
{
    public scr_hero heroData;

    public bool movementLock;
    bool targetLock, resurrected;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        heroData = (scr_hero)cardData;
        //Set movement lock to false and hero position so it sits above ground
        movementLock = false;
        targetLock = false;
        resurrected = false;
        transform.position = transform.position + new Vector3(0, 0.25f, 0);

        base.Start();
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Move(); //Call move function every frame
    }

    public virtual void Move()
    {
        if (!movementLock) //If the movement lock is false
        {
            //Hero moves forward
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            if (!targetLock)
            {
                rb.MovePosition(transform.position + (target.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime);
            }
            else
            {
                rb.SetLinearVelocity(Vector3.zero);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Collided with " +  collision.gameObject.name);
        //If the range collides with a tower
        if (target != null && target.Equals(collision.gameObject))
        {
            targetLock = true;
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                Debug.Log(this.cardData.name + " deals " + power + " damage to " + collision.gameObject.name);
                timer = 0;
                Attack();
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            Debug.Log(this.cardData.name + " has terminated " + collision.gameObject.name);
            movementLock = false;
            targetLock = false;
            target = null;
        }
    }

    /*[ServerRpc]
    public void AttackServerRpc()
    {
        Debug.Log("bullshit");
        //AttackClientRpc();
    }*/

    public override void Attack()
    {
        
        foreach (scr_status status in statuses)
        {
            if (status.statusType == scr_status.statusTypes.Grippy)
            {
                target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.grappled, status.statusAmnt));
            }
        }
        base.Attack();
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the range collides with a tower
        if((other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower") || other.gameObject.tag.Equals("Generator") ||
            other.gameObject.tag.Equals("Vehicle"))
            && !other.isTrigger && !movementLock)
        {
            Debug.Log("Should attack here.");
            if (gameObject.GetComponent<NetworkObject>().OwnerClientId == other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
            {
                foreach (scr_status status in statuses)
                {
                    if (status.statusType == scr_status.statusTypes.Healing && 
                        other.gameObject.GetComponent<scr_unit>().health < other.gameObject.GetComponent<scr_unit>().cardData.health)
                    {
                        Debug.Log("Healing");
                        movementLock = true;
                        targetLock = false;
                        timer = cooldown;
                        GetTarget(other.gameObject);
                    }
                    else if(status.statusType == scr_status.statusTypes.Miraculous && !target.GetComponent<BoxCollider>().enabled)
                    {
                        Debug.Log("Resurrected");
                        movementLock = true;
                        targetLock = false;
                        timer = cooldown;
                        GetTarget(other.gameObject);
                    }
                }
            }
            else
            {
                Debug.Log("Attack");
                movementLock = true;
                targetLock = false;
                timer = cooldown;
                GetTarget(other.gameObject);
            } 
            //Set movement lock to true and move towards tower position
        }
    }

    public override void ChangeHealth(float delta)
    {
        foreach(scr_status status in statuses)
        {
            if(status.statusType == scr_status.statusTypes.Grippy && target != null && target.tag.Equals("Hero"))
            {
                foreach(scr_condition condition in target.GetComponent<scr_unit>().conditions)
                {
                    if(condition.conditionType == scr_condition.conditionTypes.grappled)
                    {
                        target.GetComponent<scr_unit>().ChangeHealth(delta);
                        break;
                    }
                }
            }
        }
        base.ChangeHealth(delta);
    }

    public override void SetDefaultStats()
    {
        speed = heroData.speed;
        base.SetDefaultStats();
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            switch (status.statusType)
            {
                case scr_status.statusTypes.Swift:
                    speed += status.statusAmnt * status.speedPerLvl;
                    break;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            switch (condition.conditionType)
            {
                case scr_condition.conditionTypes.frozen:
                case scr_condition.conditionTypes.exhausted:
                    speed -= condition.conditionAmnt * condition.speedPerLvl;
                    break;
                case scr_condition.conditionTypes.entangled:
                case scr_condition.conditionTypes.stunned:
                    StartCoroutine(Entangled(condition.entangledDuration));
                    break;
                case scr_condition.conditionTypes.resurrected:
                    if (!resurrected)
                    {
                        Resurrect(power * condition.conditionAmnt);
                    }
                    break;
                case scr_condition.conditionTypes.grappled:
                    StartCoroutine(Entangled(condition.entangledDuration));
                    StartCoroutine(Stunned(condition.entangledDuration));
                    break;
            }
            RemoveConditionOnTimer(condition, condition.conditionTimer);
        }
    }

    public IEnumerator Entangled(int duration)
    {
        movementLock = true;
        yield return new WaitForSeconds(duration);
        movementLock = false;
    }

    public void Resurrect(float healthToSpawn)
    {
        SetDefaultStats();
        ChangeHealth(-cardData.health);
        ChangeHealth(healthToSpawn);
        movementLock = false;
        targetLock = false;
        resurrected = true;
        this.GetComponent<BoxCollider>().enabled = true;
        if (gameObject.transform.childCount > 0)
        {
            this.GetComponentInChildren<SpriteRenderer>().enabled = true;
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Health: " + health + ", CD: " + cooldown + ", Power: " + power + ", Range: " + range.radius + ", Speed:" + speed);
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().DisplayCardDetails(heroData, health, cooldown, power, range.radius, speed);
        }
    }
}
