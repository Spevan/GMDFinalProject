using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : NetworkBehaviour
{
    public scr_hero cardData;
    public SphereCollider range;
    bool movementLock;

    public float timer, cooldown, power, health;

    public Collider target;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Set movement lock to false and hero position so it sits above ground
        movementLock = false;
        transform.position = transform.position + new Vector3 (0, 0.25f, 0);

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
            transform.position = Vector3.MoveTowards(transform.position, target.transform.position, cardData.speed * Time.deltaTime);
        }
    }

    void Attack()
    {
        if (target.gameObject.tag.Equals("Hero"))
        {
            Debug.Log(this.cardData.name + " is attacking " + target.name);
            target.GetComponent<scr_heroUnit>().ChangeHealth(-cardData.power);
            //Death();

        }
        else if (target.gameObject.tag.Equals("Tower"))
        {
            Debug.Log(this.cardData.name + " is attacking " + target.name);
            target.GetComponent<scr_towerUnit>().ChangeHealth(-cardData.power);
            //Death();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        //If the range collides with a tower
        if (target != null && target.Equals(collision) && target.gameObject.activeSelf &&
            collision.gameObject.GetComponent<NetworkObject>().OwnerClientId != gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            if (timer <= cooldown)
            {
                timer += Time.deltaTime;
            }
            else
            {
                //Set movement lock to true and move towards tower position
                Attack();
                timer = 0;
            }
        }
        else if (target == null || !target.gameObject.activeSelf)
        {
            Debug.Log(this.cardData.name + " has terminated " + collision.gameObject.name);
            movementLock = false;
            target = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if((other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower")) &&
            other.gameObject.GetComponent<NetworkObject>().OwnerClientId != gameObject.GetComponent<NetworkObject>().OwnerClientId &&
            !movementLock)
        {
            //Set movement lock to true and move towards tower position
            movementLock = true;
            timer = cooldown;
            target = other;
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
