using TMPro;
using UnityEngine;

public class scr_cardDetails : MonoBehaviour
{
    public scr_card cardData;
    [SerializeField] TextMeshProUGUI[] cost;
    [SerializeField] TextMeshProUGUI health;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < cardData.cost.Length; i++)
        {
            cost[i].text = cardData.cost[i].ToString();
        }
        health.text = cardData.health.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
