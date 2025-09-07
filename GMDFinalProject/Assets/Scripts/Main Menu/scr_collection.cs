using UnityEngine;

public class scr_collection : MonoBehaviour
{
    public GameObject prefab, grid;

    public virtual void Start()
    {
        foreach (scr_card card in scr_dataPersistenceManager.instance.playerData.cards)
        {
            GameObject temp = Instantiate(prefab, grid.transform);
            temp.GetComponent<scr_cardsInMenu>().cardData = card;
            temp.GetComponent<scr_cardsInMenu>().GUI = GetComponentInParent<Canvas>().gameObject;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        grid.GetComponent<RectTransform>().localPosition = new Vector3(0, grid.GetComponent<RectTransform>().rect.yMin, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
