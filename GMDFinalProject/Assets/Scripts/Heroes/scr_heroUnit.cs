using Unity.VisualScripting;
using UnityEngine;

public class scr_heroUnit : MonoBehaviour
{
    public scr_hero cardData;
    public SphereCollider range;
    bool movementLock;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        movementLock = false;

        range = this.AddComponent<SphereCollider>();
        range.radius = cardData.range;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Move()
    {
        if (!movementLock)
        {
            transform.position = transform.forward * cardData.speed * Time.deltaTime;
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
