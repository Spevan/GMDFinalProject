using System;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : scr_unit
{
    scr_hero heroData;

    public bool movementLock;
    public float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        heroData = (scr_hero)cardData;
        //Set movement lock to false and hero position so it sits above ground
        movementLock = false;
        transform.position = transform.position + new Vector3(0, 0.25f, 0);

        base.Start();
    }

    // Update is called once per frame
    public void Update()
    {
        Move(); //Call move function every frame
    }

    public virtual void Move()
    {
        if (!movementLock) //If the movement lock is false
        {
            //Hero moves forward
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + (target.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        //If the range collides with a tower
        if (target != null && target.Equals(collision.gameObject))
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                Attack();
                timer = 0;
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            //Debug.Log(this.cardData.name + " has terminated " + collision.gameObject.name);
            movementLock = false;
            target = null;
        }
    }

    [ServerRpc]
    public void AttackServerRpc()
    {
        Debug.Log("bullshit");
        //AttackClientRpc();
    }

    //[ClientRpc]
    public void Attack()
    {
        if (target.gameObject.tag.Equals("Hero"))
        {
            Debug.Log(this.cardData.name + " dealt " + power + " damage to " + target.name);
            target.GetComponent<scr_heroUnit>().ChangeHealth(Convert.ToInt32(-power));
            foreach (scr_status status in statuses)
            {
                if (status.statusType == scr_status.statusTypes.Vampiric)
                {
                    ChangeHealth(Convert.ToInt32(power));
                }
                if(status.statusType == scr_status.statusTypes.Sleepy)
                {
                    target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.exhausted, status.statusAmnt));
                }
                if(status.statusType == scr_status.statusTypes.Blinding)
                {
                    target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.blind, status.statusAmnt));
                }
                if(status.statusType == scr_status.statusTypes.Crushing)
                {
                    target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.weak, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Heated)
                {
                    target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.burnt, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Frigid)
                {
                    target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.frozen, status.statusAmnt));
                }    
            }

        }
        else if (target.gameObject.tag.Equals("Tower"))
        {
            Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
            target.GetComponent<scr_towerUnit>().ChangeHealth(-cardData.power);
            foreach (scr_status status in statuses)
            {
                if (status.statusType == scr_status.statusTypes.Blinding)
                {
                    target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.blind, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Crushing)
                {
                    target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.weak, status.statusAmnt));
                }
                if(status.statusType == scr_status.statusTypes.Heated)
                {
                    target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.burnt, status.statusAmnt));
                }
                if (status.statusType == scr_status.statusTypes.Frigid)
                {
                    target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.frozen, status.statusAmnt));
                }
            }
            //Death();
        }
        else if (target.gameObject.tag.Equals("Generator"))
        {
            Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
            target.GetComponent<scr_generatorUnit>().ChangeHealth(-cardData.power);
            foreach (scr_status status in statuses)
            {
                if (status.statusType == scr_status.statusTypes.Thief)
                {
                    target.GetComponent<scr_generatorUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.leaking, status.statusAmnt, this.gameObject));
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the range collides with a tower
        if((other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower") || other.gameObject.tag.Equals("Generator"))
            && !other.isTrigger && other.gameObject.activeSelf && !movementLock)
        {
            if(gameObject.GetComponent<NetworkObject>().OwnerClientId == other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
            {
                foreach (scr_status status in statuses)
                {
                    if (status.statusType == scr_status.statusTypes.Healing)
                    {

                    }
                }
            }
            else
            {
                movementLock = true;
                timer = cooldown;
                GetTarget(other.gameObject);
            } 
            //Set movement lock to true and move towards tower position
        }
    }

    public override void SetDefaultStats()
    {
        base.SetDefaultStats();
        speed = heroData.speed;
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.Swift)
            {
                statuses.Add(status);
                speed += status.statusAmnt;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if (condition.conditionType == scr_condition.conditionTypes.exhausted || condition.conditionType == scr_condition.conditionTypes.frozen)
            {
                speed -= condition.conditionAmnt * condition.speedPerLvl;
            }
        }
    }
}
