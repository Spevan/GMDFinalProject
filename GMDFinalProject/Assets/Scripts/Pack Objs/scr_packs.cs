using UnityEngine;

[CreateAssetMenu(fileName = "packs", menuName = "Scriptable Sheets/packs")]
public class scr_packs : ScriptableObject
{
    public scr_card[] commonsPossible, uncommonsPossible, raresPossible, legendariesPossible;
    public int cardsNum, packPrice;
}
