using UnityEngine;
using UnityEngine.UI;

public class scr_collection : MonoBehaviour
{
    public GameObject prefab, grid, deckList;
    public float timeRemain = 0.5f; public int count = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        scr_dataPersistenceManager.instance.LoadGame();
        grid.GetComponent<RectTransform>().localPosition = new Vector3(0, grid.GetComponent<RectTransform>().rect.yMin, 0);
        count = 0;
    }

    public virtual void Update()
    {
        if (timeRemain > 0)
        {
            timeRemain -= Time.deltaTime;
        }
        else if (count < scr_dataPersistenceManager.instance.playerData.cardsCollected.Count)
        {
            CreateCard();
            count++;
            Debug.Log(count);
            timeRemain = 0.25f;
        }
    }

    public void CreateCard()
    {
        GameObject temp = Instantiate(prefab, grid.transform);
        scr_cardsInMenu tempCard = temp.GetComponent<scr_cardsInMenu>();
        tempCard.LoadData(scr_dataPersistenceManager.instance.playerData);
        tempCard.GUI = GetComponentInParent<Canvas>().gameObject;
        tempCard.deckList = deckList;
    }

    public void OnDisable()
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
