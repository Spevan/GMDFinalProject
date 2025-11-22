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
        //scr_gameManager.instance.SpawnGeneratorClientRpc(this.gameObject, 1, OwnerClientId);
        foreach(GameObject player in scr_gameManager.instance.players)
        {
            if(player.GetComponent<NetworkObject>().OwnerClientId == OwnerClientId)
            {
                GetTarget(player);
            }
        }
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
            target.GetComponent<scr_player>().ChangeWater((int)produceDelta);

            foreach (scr_condition condition in conditions)
            {
                if (condition.conditionType == scr_condition.conditionTypes.leaking)
                {
                    condition.conditionOrigin.GetComponent<scr_unit>().GetLeaking(condition.conditionAmnt);
                }
            }
        }
    }

    public override void SetStatuses()
    {
        base.SetStatuses();
        foreach (scr_status status in cardData.statuses)
        {
            if (status.statusType == scr_status.statusTypes.Productive)
            {
                statuses.Add(status);
                power += status.statusAmnt * waterPerLevel;
            }
        }

        foreach (scr_condition condition in conditions)
        {
            if (condition.conditionType == scr_condition.conditionTypes.leaking)
            {
                power -= condition.conditionAmnt * waterPerLevel;
            }
        }
    }

    public override void Death()
    {
        scr_gameManager.instance.EndGameServerRpc(this.NetworkObject.OwnerClientId);
        base.Death();
    }
}
