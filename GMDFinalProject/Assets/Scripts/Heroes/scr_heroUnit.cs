using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : NetworkBehaviour
{
    public scr_player player;
    public scr_hero cardData;
    public SphereCollider range;
    bool movementLock;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    void Start()
    {
        SpawnNetworkClientRpc();

        movementLock = false;
        transform.position = transform.position + new Vector3 (0, 0.25f, 0);

        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
        range.isTrigger = true;
    }

    [ClientRpc]
    void SpawnNetworkClientRpc()
    {
        this.GetComponent<NetworkObject>().Spawn();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move()
    {
        if (!movementLock)
        {
            Vector3 direction = new Vector3(0, 0, player.cam.transform.forward.z);
            transform.Translate(-direction * cardData.speed * Time.deltaTime);
        }
    }

    void Attack()
    {

    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.gameObject.tag.Equals("Tower"))
        {
            movementLock = true;
            transform.position = Vector3.MoveTowards(transform.position, collision.transform.position, cardData.speed * Time.deltaTime);
        }
    }
}
