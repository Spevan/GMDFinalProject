using System;
using System.Collections;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

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
            rb.MovePosition(transform.position + (target.transform.position - transform.position).normalized * speed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
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

    /*[ServerRpc]
    public void AttackServerRpc()
    {
        Debug.Log("bullshit");
        //AttackClientRpc();
    }*/

    public override void Attack()
    {
        base.Attack();
        foreach (scr_status status in statuses)
        {
            if (status.statusType == scr_status.statusTypes.Vampiric && target.tag.Equals("Hero"))
            {
                ChangeHealth(Convert.ToInt32(power * (0.1 * status.statusAmnt)));
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
                    StartCoroutine(Entangled(condition.entangledDuration));
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

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Health: " + health + ", CD: " + cooldown + ", Power: " + power + ", Range: " + range.radius + ", Speed:" + speed);
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().DisplayCardDetails(heroData, health, cooldown, power, range.radius, speed);
        }
    }
}
