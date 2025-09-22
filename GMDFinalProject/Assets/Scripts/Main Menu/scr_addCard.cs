using UnityEngine;

public class scr_addCard : MonoBehaviour
{
    public scr_cards card;

    public void AddCard()
    {
        card.SaveData(ref scr_dataPersistenceManager.instance.playerData);
    }
}
