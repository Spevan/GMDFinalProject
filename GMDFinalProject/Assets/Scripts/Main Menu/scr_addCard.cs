using UnityEngine;

public class scr_addCard : MonoBehaviour
{
    public scr_card card;

    public void AddCard()
    {
        scr_dataPersistenceManager.instance.AddCardToCollection(card);
        scr_dataPersistenceManager.instance.SaveGame();
    }
}
