using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_vehicleUnit : scr_towerUnit
{
    public float speed;
    public bool movementLock;
    public scr_vehicle vehicleData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        if (cardData.GetType() == typeof(scr_vehicle))
        {
            vehicleData = cardData as scr_vehicle;
        }
        base.Start();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
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

    private void OnTriggerEnter(Collider other)
    {
        if ((target == null || !target.gameObject.activeInHierarchy) && (other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower") || other.gameObject.tag.Equals("Vehicle")))
        {
            //Debug.Log(other.name + " detected by " + this.cardData.name);
            timer = cooldown;
            GetTarget(other.gameObject);
        }
    }

    public override void SetDefaultStats()
    {
        speed = vehicleData.speed;
        base.SetDefaultStats();
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            switch(status.statusType)
            {
                case scr_status.statusTypes.Swift:
                    speed += status.statusAmnt * status.speedPerLvl;
                    break;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            switch(condition.conditionType)
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
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Health: " + health + ", CD: " + cooldown + ", Power: " + power + ", Range: " + range.radius + ", Speed:" + speed);
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().DisplayCardDetails(vehicleData, health, cooldown, power, range.radius, speed);
        }
    }

    public override void Death()
    {
        if(NetworkManager.IsServer)
        {
            scr_gameManager.instance.SpawnNetworkCard(vehicleData.unitEquipped.name, transform.position, transform.rotation, OwnerClientId);
        }
        else
        {
            scr_gameManager.instance.SpawnNetworkCardServerRpc(vehicleData.unitEquipped.name, transform.position, transform.rotation, OwnerClientId);
        }
        base.Death();
    }
}
