using TMPro;
using UnityEngine;

public class scr_currency : MonoBehaviour
{
    public TextMeshProUGUI currencyTXT;

    // Update is called once per frame
    void Update()
    {
        currencyTXT.text = scr_dataPersistenceManager.instance.playerData.currency.ToString();
    }
}
