using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using EnumDescription;

public class scr_statusInMenu : MonoBehaviour
{
    public scr_status statusData;
    [SerializeField] TextMeshProUGUI status, description;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        status.text = statusData.statusType.ToString() + " " + statusData.statusAmnt.ToString();
        description.text = statusData.statusType.GetDescription();
    }

    public void SetStatus(scr_status status)
    {
        statusData = status;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
