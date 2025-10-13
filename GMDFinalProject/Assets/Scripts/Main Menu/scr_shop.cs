using TMPro;
using UnityEngine;

public class scr_shop : MonoBehaviour
{
    public TextMeshProUGUI currencyTXT;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        DisplayCurrency();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayCurrency()
    {
        currencyTXT.text = scr_dataPersistenceManager.instance.playerData.currency.ToString();
    }
}
