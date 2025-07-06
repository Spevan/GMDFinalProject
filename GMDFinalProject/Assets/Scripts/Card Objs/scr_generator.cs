using UnityEngine;

[CreateAssetMenu(fileName = "generator", menuName = "Scriptable Sheets/generator")]
public class scr_generator : scr_card
{
    [System.Serializable]
    public enum productionType
    {
        water,
        steel
    }
    public productionType typeGenerated;
}
