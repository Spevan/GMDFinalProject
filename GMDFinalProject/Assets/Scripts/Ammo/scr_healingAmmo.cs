using Unity.Netcode;
using UnityEngine;

public class scr_healingAmmo : scr_ammunition
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger &&
            gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            if (target.tag.Equals("Hero"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_heroUnit>().ChangeHealth(ammoData.damage);
            }
        }
        Destroy();
    }
}
