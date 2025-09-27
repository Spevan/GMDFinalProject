using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class scr_generatorUnit : scr_unit
{
    scr_productionPlant generatorData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public override void Start()
    {
        base.Start();
        generatorData = cardData as scr_productionPlant;
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
}
