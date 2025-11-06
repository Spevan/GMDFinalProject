using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class scr_towerUnit : scr_unit
{
    //public new override scr_tower cardData;
    public List<GameObject> pooledProj = new List<GameObject>();
    public int amountPooledProj;
    public scr_tower towerData;
    public GameObject ammunition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        towerData = (scr_tower)cardData;
        //Set tower position so it sits above ground
        transform.position = transform.position + new Vector3(0, 0.5f, 0);

        range = this.AddComponent<SphereCollider>();
        base.Start();
        ammunition = towerData.ammunition;

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

    void Attack()
    {
        for (int i = 0; i < pooledProj.Count; i++)
        {
            if (!pooledProj[i].activeSelf)
            {
                //Debug.Log("This should show up, no?");
                pooledProj[i].GetComponent<scr_ammunition>().GetTarget(target.gameObject, this.gameObject);
                pooledProj[i].transform.position = transform.position;
                pooledProj[i].SetActive(true);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((target == null || !target.gameObject.activeInHierarchy) && other.gameObject.tag.Equals("Hero"))
        {
            //Debug.Log(other.name + " detected by " + this.cardData.name);
            timer = cooldown;
            target = other.gameObject;
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
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.Perceptive)
            {
                statuses.Add(status);
                range.radius += status.statusAmnt * status.rangePerLvl;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if (condition.conditionType == scr_condition.conditionTypes.blind)
            {
                range.radius -= condition.conditonAmnt * condition.rangePerLvl;
            }
        }
    }
}