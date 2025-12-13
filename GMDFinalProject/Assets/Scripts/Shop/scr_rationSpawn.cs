using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr_rationSpawn : MonoBehaviour
{
    public bool spawned, redeemed, corner;
    public GameObject ration;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnRation(GameObject ration, scr_card card)
    {
        this.ration = ration;
        spawned = true;
        this.ration.GetComponentInChildren<Button>().onClick.AddListener(RedeemCard); 
        this.ration.GetComponentInChildren<TextMeshProUGUI>().text = card.name; 
    }

    void RedeemCard()
    {

    }
}
