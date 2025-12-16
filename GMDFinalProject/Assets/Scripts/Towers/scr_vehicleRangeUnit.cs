using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_vehicleRangeUnit : scr_vehicleUnit
{
    public scr_vehicleRange vehicleRangeData;

    public List<GameObject> pooledProj = new List<GameObject>();
    public int amountPooledProj;
    public GameObject ammunition;

    public override void Start()
    {
        if (cardData.GetType() == typeof(scr_vehicleRange))
        {
            vehicleRangeData = cardData as scr_vehicleRange;
        }

        base.Start();
        ammunition = vehicleRangeData.ammunition;
        if (this.GetComponent<NetworkObject>().IsOwner)
        {
            for (int i = 0; i < amountPooledProj; i++)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    SpawnNetworkAmmo(ammunition.name, transform.position, new Quaternion(0, transform.rotation.y, 0, 0),
                        this.GetComponent<NetworkObject>().OwnerClientId);
                }
                else
                {
                    SpawnNetworkAmmoServerRpc(ammunition.name, transform.position, new Quaternion(0, transform.rotation.y, 0, 0),
                        this.GetComponent<NetworkObject>().OwnerClientId);
                }
            }
        }
    }

    public override void Move()
    {
        if (!movementLock) //If the movement lock is false
        {
            //Hero moves forward
            rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
        }
        else
        {
            rb.SetLinearVelocity(Vector3.zero);
        }
    }

    //Spawn a new network object based on parameters
    public void SpawnNetworkAmmo(string ammoName, Vector3 pos, Quaternion rot, ulong clientID)
    {
        //Debug.Log("Spawning ammo");
        //Instantiate and spawn the network object of that card's unit value
        GameObject unit = Instantiate(ammunition, pos, rot);
        unit.GetComponent<NetworkObject>().SpawnWithOwnership(clientID);
        ReturnAmmoClientRpc(unit);
    }

    //Request the server to...
    [ServerRpc(RequireOwnership = false)]
    public void SpawnNetworkAmmoServerRpc(string ammoName, Vector3 pos, Quaternion rot, ulong clientID)
    {
        SpawnNetworkAmmo(ammoName, pos, rot, clientID);
    }

    //BULLSHIT!!!!
    [ClientRpc(RequireOwnership = false)]
    public void ReturnAmmoClientRpc(NetworkObjectReference ammo)
    {
        //Debug.Log("tower: " + this.ToString() + " pools obj: " + ammo.ToString());
        pooledProj.Add(ammo);
    }

    public override void Attack()
    {
        for (int i = 0; i < pooledProj.Count; i++)
        {
            if (!pooledProj[i].activeSelf)
            {
                //Debug.Log("This should show up, no?");
                pooledProj[i].GetComponent<scr_ammunition>().GetTarget(target.gameObject, this.gameObject);
                //pooledProj[i].transform.position = transform.position;
                //pooledProj[i].SetActive(true);
                break;
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("Health: " + health + ", CD: " + cooldown + ", Power: " + power + ", Range: " + range.radius + ", Speed:" + 0);
        foreach (GameObject player in scr_gameManager.instance.players)
        {
            player.GetComponentInChildren<scr_guiManager>().DisplayCardDetails(vehicleRangeData, health, cooldown, power, range.radius, 0);
        }
    }
}
