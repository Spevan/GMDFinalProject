using UnityEngine;

public class scr_prodPlantUnit : scr_generatorUnit
{
    public override void Death()
    {
        scr_player.instance.LoseGame();
        base.Death();
    }
}
