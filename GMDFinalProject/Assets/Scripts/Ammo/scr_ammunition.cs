using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ammunition : NetworkBehaviour
{
    [SerializeField] scr_ammo ammoData;

    GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;

        transform.position = transform.position + direction * ammoData.speed * Time.deltaTime;
    }

    public void GetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Hero"))
        {
            other.GetComponent<scr_heroUnit>().ChangeHealth(-ammoData.damage);
        }
        else if(other.gameObject.tag.Equals("Tower"))
        {
            other.GetComponent<scr_towerUnit>().ChangeHealth(-ammoData.damage);
        }
    }
}
