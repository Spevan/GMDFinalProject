using Unity.VisualScripting;
using UnityEngine;

public class scr_towerUnit : MonoBehaviour
{
    public scr_tower cardData;
    public SphereCollider range;

    public int cooldown, power, health;
    public float timer;

    public GameObject[] pooledProj;

    private Collider target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set tower position so it sits above ground
        transform.position = transform.position + new Vector3(0, 0.25f, 0);

        //Setting range component, size and trigger status
        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
        range.isTrigger = true;

        cooldown = (int)cardData.maxCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Attack()
    {
        if (timer <= cooldown)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ObjectPooling();
        }
    }

    void ObjectPooling()
    {
        for (int i = 0; i < pooledProj.Length; i++)
        {
            if (!pooledProj[i].activeSelf)
            {
                pooledProj[i].SetActive(true);
                pooledProj[i].transform.position = transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(target == null && other.gameObject.tag.Equals("Hero"))
        {
            target = other;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if (target != null && target.Equals(other))
        {
            //Set movement lock to true and move towards tower position
            Attack();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target != null && target.Equals(other))
        {
            target = null;
        }
    }
}
