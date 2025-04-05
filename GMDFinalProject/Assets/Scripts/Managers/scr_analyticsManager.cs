using UnityEngine;

public class scr_analyticsManager : MonoBehaviour
{
    int cardsPlayed, waterUsed, minsElapsed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        minsElapsed = (int)Time.fixedTime;
    }

    public int GetTimeElapsed()
    {
        return minsElapsed;
    }

    public int GetWaterUsed(int waterSpent)
    {
        waterUsed += waterSpent;
        return waterUsed;
    }

    public int GetCardsPlayed()
    {
        cardsPlayed++;
        return cardsPlayed;
    }
}
