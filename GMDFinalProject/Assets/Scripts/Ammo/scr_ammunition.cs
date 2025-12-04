using System;
using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ammunition : NetworkBehaviour
{
    public scr_ammo ammoData;
    NetworkRigidbody rb;

    public GameObject target, sourceObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<NetworkRigidbody>();
        Destroy();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        if (target != null && target.activeSelf)
        {
            Debug.Log("Tracking " +  target.name);
            Vector3 direction = (target.transform.position - transform.position).normalized;

            rb.MovePosition(transform.position + direction * ammoData.speed * Time.fixedDeltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            rb.MovePosition(transform.position + gameObject.transform.forward * ammoData.speed * Time.fixedDeltaTime);
            Debug.Log("Target lost.");
        }
    }

    public void GetTarget(GameObject newTarget, GameObject tower)
    {
        transform.position = tower.transform.position;
        target = newTarget;
        sourceObj = tower;
        this.GetComponent<BoxCollider>().enabled = true;
        this.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)//gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId && !other.isTrigger)
        {
            Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
            target.GetComponent<scr_unit>().ChangeHealth(-sourceObj.GetComponent<scr_unit>().power);
            foreach (scr_status status in sourceObj.GetComponent<scr_unit>().statuses)
            {
                if (status.statusType == scr_status.statusTypes.Vampiric && target.tag.Equals("Hero"))
                {
                    sourceObj.GetComponent<scr_unit>().ChangeHealth(Convert.ToInt32(sourceObj.GetComponent<scr_unit>().power * (0.1 * status.statusAmnt)));
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
                if(status.statusType == scr_status.statusTypes.Tangled && (target.tag.Equals("Hero") || target.tag.Equals("Vehicle")))
                {
                    target.GetComponent<scr_unit>().AddCondition(new scr_condition(scr_condition.conditionTypes.entangled, status.statusAmnt));
                }
            }
            Destroy();
        }
    }

    public void Destroy()
    {
        target = null;
        this.GetComponent<BoxCollider>().enabled = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        //this.gameObject.SetActive(false);
    }
}
