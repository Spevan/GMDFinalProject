using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;

public class scr_towerUnit : NetworkBehaviour
{
    public scr_tower cardData;
    public SphereCollider range;

    public float timer, cooldown, power, health;

    public List<GameObject> pooledProj;
    public int amountPooledProj;

    public Collider target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set tower position so it sits above ground
        transform.position = transform.position + new Vector3(0, 0.25f, 0);

        //Setting range component, size and trigger status
        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
        range.isTrigger = true;

        power = cardData.power;
        cooldown = cardData.maxCooldown;
        health = cardData.health;

        pooledProj = new List<GameObject>();
        if (this.GetComponent<NetworkObject>().IsOwner)
        {
            for (int i = 0; i < amountPooledProj; i++)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    pooledProj.Add(
                        scr_gameManager.instance.SpawnNetworkAmmo(
                            cardData.ammunition.name, transform.position, new Quaternion(0, transform.rotation.y, 0, 0)));
                }
                else
                {
                    scr_gameManager.instance.SpawnNetworkAmmoServerRpc(this.GetComponent<NetworkObject>().NetworkObjectId,
                        cardData.ammunition.name, transform.position, new Quaternion(0, transform.rotation.y, 0, 0));
                }
            }
        }
    }



    void Attack()
    {
        for (int i = 0; i < pooledProj.Count; i++)
        {
            if (!pooledProj[i].activeSelf)
            {
                pooledProj[i].SetActive(true);
                pooledProj[i].transform.position = transform.position;
                pooledProj[i].GetComponent<scr_ammunition>().GetTarget(target.gameObject);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if((target == null || !target.gameObject.activeInHierarchy) && other.gameObject.tag.Equals("Hero"))
        {
            Debug.Log(other.name + " detected by " + this.cardData.name);
            timer = cooldown;
            target = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if (target != null && target.Equals(other) && target.gameObject.activeSelf && !IsOwner)
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                Debug.Log(this.cardData.name + " is attacking " + other.name);
                Attack();
                timer = 0;
            }
        }
        else if(target == null || !target.gameObject.activeSelf)
        {
            Debug.Log(this.cardData.name + " has terminated " + other.name);
            target = null;
        }
    }

    public void ChangeHealth(int delta)
    {
        health += delta;

        if (health <= 0)
        {
            Death();
        }
        else if(health > cardData.health)
        {
            health = cardData.health;
        }
    }

    void Death()
    {
        this.gameObject.SetActive(false);
    }
}
