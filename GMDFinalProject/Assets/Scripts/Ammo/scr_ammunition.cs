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
        Destroy();
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Hero") && !IsOwner)
        {
            collision.collider.GetComponent<scr_heroUnit>().ChangeHealth(-ammoData.damage);
            Destroy();

        }
        else if (collision.gameObject.tag.Equals("Tower") && !IsOwner)
        {
            collision.collider.GetComponent<scr_towerUnit>().ChangeHealth(-ammoData.damage);
            Destroy();
        }
    }

    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}
