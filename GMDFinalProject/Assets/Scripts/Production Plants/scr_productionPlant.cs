using UnityEngine;

[CreateAssetMenu(fileName = "productionPlant", menuName = "Scriptable Sheets/productionPlant")]
[System.Serializable]
public class scr_productionPlant : ScriptableObject
{
    public int plant_id;
    public bool loaded;
    public GameObject unit;
    public int health;
    public string description;
    public float maxCooldown;
}
