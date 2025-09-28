using UnityEngine;

public class scr_prodPlantUnit : scr_generatorUnit
{
    public override void Death()
    {
        scr_gameManager.instance.EndGameServerRpc(this.NetworkObject.OwnerClientId);
    }
}
