using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : NetworkBehaviour
{
    public scr_hero cardData;
    public SphereCollider range;
    bool movementLock;

    public float timer, cooldown, power, health;

    public GameObject target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set movement lock to false and hero position so it sits above ground
        movementLock = false;
        transform.position = transform.position + new Vector3(0, 0.25f, 0);

        //Setting range component, size and trigger status
        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
        range.isTrigger = true;

        power = cardData.power;
        cooldown = cardData.maxCooldown;
        health = cardData.health;
    }

    // Update is called once per frame
    void Update()
    {
        Move(); //Call move function every frame
    }

    void Move()
    {
        if (!movementLock) //If the movement lock is false
        {
            //Hero moves forward
            transform.Translate(Vector3.forward * cardData.speed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(
                target.transform.position.x, transform.position.y, target.transform.position.z), cardData.speed * Time.deltaTime);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        //If the range collides with a tower
        if (target != null && target.Equals(collision.gameObject) && collision.gameObject.activeSelf &&
            gameObject.GetComponent<NetworkObject>().OwnerClientId != collision.gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                AttackClientRpc();
                timer = 0;
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            //Debug.Log(this.cardData.name + " has terminated " + collision.gameObject.name);
            movementLock = false;
            target = null;
        }
    }

    [ClientRpc]
    public void AttackClientRpc()
    {
        if (target.gameObject.tag.Equals("Hero"))
        {
            Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
            target.GetComponent<scr_heroUnit>().ChangeHealth(-cardData.power);
            //Death();

        }
        else if (target.gameObject.tag.Equals("Tower"))
        {
            Debug.Log(this.cardData.name + " dealt " + cardData.power + " damage to " + target.name);
            target.GetComponent<scr_towerUnit>().ChangeHealth(-cardData.power);
            //Death();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //If the range collides with a tower
        if((other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower")) &&
            gameObject.GetComponent<NetworkObject>().OwnerClientId != other.gameObject.GetComponent<NetworkObject>().OwnerClientId &&
            !movementLock)
        {
            //Set movement lock to true and move towards tower position
            movementLock = true;
            timer = cooldown;
            target = other.gameObject;
        }
    }

    public void ChangeHealth(int delta)
    {
        health += delta;

        if(health <= 0)
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
