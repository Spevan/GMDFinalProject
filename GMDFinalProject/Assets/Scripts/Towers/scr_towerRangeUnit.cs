using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_towerRangeUnit : scr_towerUnit
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
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

    // Update is called once per frame
    void Update()
    {
        
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
}
