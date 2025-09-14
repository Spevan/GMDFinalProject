using UnityEngine;
using UnityEngine.UI;

public class scr_collection : MonoBehaviour
{
    public GameObject prefab, grid, deckList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        grid.GetComponent<RectTransform>().localPosition = new Vector3(0, grid.GetComponent<RectTransform>().rect.yMin, 0);

        if (scr_dataPersistenceManager.instance.playerData.cards != null)
        {
            foreach (scr_card card in scr_dataPersistenceManager.instance.playerData.cards)
            {
                GameObject temp = Instantiate(prefab, grid.transform);
                scr_cardsInMenu tempCard = temp.GetComponent<scr_cardsInMenu>();
                tempCard.cardData = card;
                tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
                tempCard.deckList = deckList;
            }
        }
    }

    private void OnDisable()
    {
        foreach(Transform child in grid.transform)
        {
            if (!child.tag.Equals("PleaseDontDestroy"))
            {
                Destroy(child.gameObject);
            }
        }
    }
}
