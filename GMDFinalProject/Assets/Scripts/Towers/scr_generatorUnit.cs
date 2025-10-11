using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class scr_generatorUnit : scr_unit
{
    scr_productionPlant generatorData;
    int waterPerLevel = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        generatorData = cardData as scr_productionPlant;
        base.Start();
        transform.position = transform.position + new Vector3(0, 0.5f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer <= cooldown)
        {
            timer += Time.deltaTime;
        }
        else
        {
            ProduceResource(power);
            timer = 0;
        }
    }

    public void ProduceResource(float produceDelta)
    {
        if (generatorData != null && NetworkManager.Singleton.LocalClientId == gameObject.GetComponent<NetworkObject>().OwnerClientId)
        {
            scr_player.instance.ChangeWater((int)produceDelta);
        }
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.productive)
            {
                statuses.Add(status);
                power += status.statusAmnt * waterPerLevel;
            }
        }
    }
}
