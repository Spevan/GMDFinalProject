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
        if (other.gameObject.tag.Equals("Hero"))
        {
            foreach (scr_status status in cardData.statuses)
            {
                other.gameObject.GetComponent<scr_heroUnit>().GetSupport(status);
            }
        }
        else if (other.gameObject.tag.Equals("Tower"))
        {
            foreach (scr_status status in cardData.statuses)
            {
                other.gameObject.GetComponent<scr_towerUnit>().GetSupport(status);
            }
        }
    }
}
