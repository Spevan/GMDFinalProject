using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_cards : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject GUI, details, temp;
    public scr_card cardData;
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        temp = Instantiate(details, GUI.transform);
        temp.GetComponent<scr_cardDetails>().cardData = cardData;
        if (this.transform.localPosition.x < (GUI.GetComponentInParent<Camera>().scaledPixelWidth / 2))
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(100, 0, 5);
        }
        else
        {
            temp.transform.localPosition = this.transform.localPosition + new Vector3(-100, 0, 5);
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (temp != null)
        {
            Destroy(temp);
        }
    }
}
