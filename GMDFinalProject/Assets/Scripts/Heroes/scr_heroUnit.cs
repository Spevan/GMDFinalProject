using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : NetworkBehaviour
{
    public scr_player player;
    public scr_hero cardData;
    public SphereCollider range;
    bool movementLock;

    private Collider target;

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

    }

    private void OnTriggerStay(Collider other)
    {
        //If the range collides with a tower
        if(!movementLock && other.gameObject.tag.Equals("Hero") || other.gameObject.tag.Equals("Tower"))
        {
            //Set movement lock to true and move towards tower position
            movementLock = true;
            target = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.Equals(target))
        {
            movementLock = false;
            target = null;
        }
    }
}
