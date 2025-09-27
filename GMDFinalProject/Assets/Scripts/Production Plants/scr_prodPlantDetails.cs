using TMPro;
using UnityEngine;

public class scr_prodPlantDetails : MonoBehaviour
{
    public scr_productionPlant plantData;
    TextMeshProUGUI health, cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health.text = health.ToString();
        cooldown.text = cooldown.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
