using TMPro;
using UnityEngine;

public class scr_cardDetails : MonoBehaviour
{
    public scr_card cardData;
    [SerializeField] TextMeshProUGUI cost;
    [SerializeField] TextMeshProUGUI health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cost.text = cardData.cost.ToString();
        health.text = cardData.health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
