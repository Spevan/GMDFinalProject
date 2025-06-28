using Unity.Netcode;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class scr_ammunition : NetworkBehaviour
{
    [SerializeField] scr_ammo ammoData;

    public GameObject target;

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
            Vector3 direction = (target.transform.position - transform.position).normalized;

            transform.position = transform.position + direction * ammoData.speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        else
        {
            transform.position = transform.position + Vector3.forward * ammoData.speed * Time.deltaTime;
        }
    }

    public void GetTarget(GameObject newTarget)
    {
        target = newTarget;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (target != null && target.Equals(collision.gameObject) && target.gameObject.activeSelf &&
            gameObject.GetComponent<NetworkObject>().OwnerClientId != target.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            BulletHitClientRpc(target);
        }
    }

    [ClientRpc]
    public void BulletHitClientRpc(NetworkObjectReference tryTarget)
    {
        if (tryTarget.TryGet(out NetworkObject target))
            {
            if (target.tag.Equals("Hero") &&
                gameObject.GetComponent<NetworkObject>().OwnerClientId != target.GetComponent<NetworkObject>().OwnerClientId)
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_heroUnit>().ChangeHealth(-ammoData.damage);
            }
            else if (target.tag.Equals("Tower") &&
                gameObject.GetComponent<NetworkObject>().OwnerClientId != target.GetComponent<NetworkObject>().OwnerClientId)
            {
                Debug.Log(name + " dealt " + ammoData.damage + " damage to " + target.gameObject.name);
                target.GetComponent<scr_towerUnit>().ChangeHealth(-ammoData.damage);
            }
        }
        Destroy();
    }

    private void Destroy()
    {
        this.gameObject.SetActive(false);
    }
}
