using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_rangeHeroUnit : scr_heroUnit
{
    scr_rangeHero rangeHeroData;
    public GameObject ammunition;
    public SphereCollider range;

    public List<GameObject> pooledProj = new List<GameObject>();
    public int amountPooledProj;

    public override void Start()
    {
        rangeHeroData = cardData as scr_rangeHero;
        range = this.AddComponent<SphereCollider>();
        base.Start();
        ammunition = rangeHeroData.ammo;

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
            rb.MovePosition(rb.position + transform.forward * speed * Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if (target != null && target.Equals(other.gameObject))
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                //Debug.Log(this.cardData.name + " is attacking " + other.name);
                RangeAttack();
                timer = 0;
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            Debug.Log(this.cardData.name + " has terminated " + other.name);
            target = null;
        }
    }

    public void RangeAttack()
    {
        for (int i = 0; i < pooledProj.Count; i++)
        {
            if (!pooledProj[i].activeSelf)
            {
                Debug.Log("This should show up, no?");
                pooledProj[i].GetComponent<scr_ammunition>().GetTarget(target.gameObject);
                pooledProj[i].transform.position = transform.position;
                pooledProj[i].SetActive(true);
                break;
            }
        }
    }

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

    public override void SetDefaultStats()
    {
        base.SetDefaultStats();
        range.radius = rangeHeroData.range;
        range.isTrigger = true;
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.perceptive)
            {
                statuses.Add(status);
                range.radius += status.statusAmnt;
            }
        }
    }
}
