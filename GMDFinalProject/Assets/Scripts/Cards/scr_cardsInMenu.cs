using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scr_cardsInMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GUI, details, temp;
    public scr_card cardData;

    public void OnPointerEnter(PointerEventData eventData)
    {
        temp = Instantiate(details, GUI.transform);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
        if(this.transform.localPosition.x < (GUI.GetComponentInChildren<Camera>().scaledPixelWidth / 2))
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(160, 0, 5);
        }
        else
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(-160, 0, 5);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(temp != null)
        {
            Destroy(temp);
        }
    }
}
