using System;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_towerUnit : scr_unit
{
    //public new override scr_tower cardData;
    public scr_tower towerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        if (cardData.GetType() == typeof(scr_tower))
        {
            towerData = (scr_tower)cardData;
        }
        //Set tower position so it sits above ground
        transform.position = transform.position + new Vector3(0, 0.5f, 0);
        base.Start();
    }

    public override void Attack()
    {
        if (target.gameObject.tag.Equals("Hero"))
        {
            base.Attack();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((target == null || !target.gameObject.activeInHierarchy) && (other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Vehicle")))
        {
            //Debug.Log(other.name + " detected by " + this.cardData.name);
            timer = cooldown;
            GetTarget(other.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if (target != null && target.Equals(other.gameObject) && !other.isTrigger && other.gameObject.activeSelf &&
            gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                //Debug.Log(this.cardData.name + " is attacking " + other.name);
                Attack();
                timer = 0;
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            //Debug.Log(this.cardData.name + " has terminated " + other.name);
            target = null;
        }
    }

    public override void SetStatuses()
    {

    }
}