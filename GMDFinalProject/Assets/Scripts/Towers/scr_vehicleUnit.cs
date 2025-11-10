using UnityEngine;

public class scr_vehicleUnit : scr_towerUnit
{
    public float speed;
    public bool movementLock;
    scr_vehicle vehicleData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        vehicleData = towerData as scr_vehicle;
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public virtual void Move()
    {
        if (!movementLock) //If the movement lock is false
        {
            //Hero moves forward
            rb.MovePosition(transform.position + transform.forward * speed * Time.deltaTime);
        }
        else
        {
            rb.MovePosition(transform.position + (target.transform.position - transform.position).normalized * speed * Time.deltaTime);
        }
    }

    public override void SetDefaultStats()
    {
        base.SetDefaultStats();
        speed = vehicleData.speed;
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.Swift)
            {
                statuses.Add(status);
                speed += status.statusAmnt * status.speedPerLvl;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if(condition.conditionType == scr_condition.conditionTypes.exhausted || condition.conditionType == scr_condition.conditionTypes.frozen)
            {
                speed -= condition.conditonAmnt * condition.speedPerLvl;
            }
        }
    }
}
