using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ammunition : NetworkBehaviour
{
    public scr_ammo ammoData;
    NetworkRigidbody rb;

    public GameObject target, sourceObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<NetworkRigidbody>();
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

            rb.MovePosition(transform.position + direction * ammoData.speed * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            rb.MovePosition(transform.position + gameObject.transform.forward * ammoData.speed * Time.deltaTime);
            Debug.Log("Target lost.");
        }
    }

    public void GetTarget(GameObject newTarget, GameObject tower)
    {
        target = newTarget;
        sourceObj = tower;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId && !other.isTrigger)
        {
            if (target.tag.Equals("Hero"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_heroUnit>().ChangeHealth(-ammoData.damage);
                foreach (scr_status status in sourceObj.GetComponent<scr_heroUnit>().statuses)
                {
                    if (status.statusType == scr_status.statusTypes.Vampiric && sourceObj.tag.Equals("Hero"))
                    {
                        sourceObj.GetComponent<scr_heroUnit>().ChangeHealth(ammoData.damage);
                    }
                    else if (status.statusType == scr_status.statusTypes.Vampiric && sourceObj.tag.Equals("Tower"))
                    {
                        sourceObj.GetComponent<scr_towerUnit>().ChangeHealth(ammoData.damage);
                    }
                    if (status.statusType == scr_status.statusTypes.Sleepy)
                    {
                        target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.exhausted, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Blinding)
                    {
                        target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.blind, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Crushing)
                    {
                        target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.weak, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Heated)
                    {
                        target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.burnt, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Frigid)
                    {
                        target.GetComponent<scr_heroUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.frozen, status.statusAmnt));
                    }
                }
                Destroy();
            }
            else if (target.tag.Equals("Tower"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_towerUnit>().ChangeHealth(-ammoData.damage);
                foreach (scr_status status in sourceObj.GetComponent<scr_towerUnit>().statuses)
                {
                    if (status.statusType == scr_status.statusTypes.Blinding)
                    {
                        target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.blind, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Crushing)
                    {
                        target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.weak, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Heated)
                    {
                        target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.burnt, status.statusAmnt));
                    }
                    if (status.statusType == scr_status.statusTypes.Frigid)
                    {
                        target.GetComponent<scr_towerUnit>().AddCondition(new scr_condition(scr_condition.conditionTypes.frozen, status.statusAmnt));
                    }
                }
                Destroy();
            }
            else if (target.tag.Equals("Generator"))
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_generatorUnit>().ChangeHealth(-ammoData.damage);
                Destroy();
            }
        }
    }

    public void Destroy()
    {
        target = null;
        this.gameObject.SetActive(false);
    }
}
