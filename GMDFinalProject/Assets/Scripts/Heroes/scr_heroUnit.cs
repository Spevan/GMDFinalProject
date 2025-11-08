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
            rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(rb.position + (target.transform.position - rb.position).normalized * speed * Time.deltaTime);
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
                if (NetworkManager.Singleton.IsServer)
                {
                    AttackClientRpc();
                }
                else
                {
                    AttackServerRpc();
                }    
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
        AttackClientRpc();
    }

    [ClientRpc]
    public void AttackClientRpc()
    {
        if (NetworkManager.Singleton.IsServer)
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
                }

            }
            else if (target.gameObject.tag.Equals("Tower"))
            {
                Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
                target.GetComponent<scr_towerUnit>().ChangeHealth(-cardData.power);
                //Death();
            }
            else if (target.gameObject.tag.Equals("Generator"))
            {
                Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
                target.GetComponent<scr_generatorUnit>().ChangeHealth(-cardData.power);
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
                target = other.gameObject;
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
                speed -= condition.conditonAmnt * condition.speedPerLvl;
            }
        }
    }
}
