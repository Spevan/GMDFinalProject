using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ammunition : NetworkBehaviour
{
    public scr_ammo ammoData;

    public GameObject target, sourceObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Destroy();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Move();
    }

    void Move()
    {
        if (target != null && target.activeSelf)
        {
            Debug.Log("Tracking " +  target.name);
            Vector3 direction = (target.transform.position - transform.position).normalized;

            transform.position = transform.position + direction * ammoData.speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            transform.position = transform.position + gameObject.transform.forward * ammoData.speed * Time.deltaTime;
            Debug.Log("Target lost.");
        }
    }

    public void GetTarget(GameObject newTarget, GameObject tower)
    {
        target = newTarget;
        sourceObj = tower;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            if (target.tag.Equals("Hero"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_heroUnit>().ChangeHealth(-ammoData.damage);
                foreach (scr_status status in sourceObj.GetComponent<scr_heroUnit>().statuses)
                {
                    if(status.statusType == scr_status.statusTypes.Vampiric && sourceObj.tag.Equals("Hero"))
                    {
                        sourceObj.GetComponent<scr_heroUnit>().ChangeHealth(ammoData.damage);
                    }
                    else if(status.statusType == scr_status.statusTypes.Vampiric && sourceObj.tag.Equals("Tower"))
                    {
                        sourceObj.GetComponent<scr_towerUnit>().ChangeHealth(ammoData.damage);
                    }
                }
            }
            else if (target.tag.Equals("Tower"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_towerUnit>().ChangeHealth(-ammoData.damage);
            }
            else if (target.tag.Equals("ProductionPlant"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_prodPlantUnit>().ChangeHealth(-ammoData.damage);
            }
        }
        Destroy();
    }

    public void Destroy()
    {
        target = null;
        this.gameObject.SetActive(false);
    }
}
