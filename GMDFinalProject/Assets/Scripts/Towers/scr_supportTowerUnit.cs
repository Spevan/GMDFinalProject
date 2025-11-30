using System;
using Unity.Netcode;
using UnityEngine;

public class scr_towerSupportUnit : scr_towerUnit
{
    private void OnTriggerStay(Collider other)
    {
        if(!other.isTrigger && other.gameObject.activeSelf && 
            gameObject.GetComponent<NetworkObject>().OwnerClientId == other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                //Debug.Log(this.cardData.name + " is attacking " + other.name);
                Support(other);
                timer = 0;
            }
        }
    }

    public void Support(Collider other)
    {
        foreach (scr_status status in cardData.statuses)
        {
            //If the status found is not healing, this unit provides the other with that status
            if (status.statusType != scr_status.statusTypes.Healing)
            { 
                other.gameObject.GetComponent<scr_unit>().GetStatus(status);
            }
            //If the status found is healing, heal that unit
            else
            {
                other.gameObject.GetComponent<scr_unit>().ChangeHealth(Convert.ToInt32(power * status.statusAmnt));
            }
        }
    }
}
