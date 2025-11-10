using TMPro;
using UnityEngine;

public class scr_ending : MonoBehaviour
{
    public TextMeshProUGUI alertTxt, waterGathered, targetsDestroyed, winBonus, waterEarned;
    const int winBonusAmnt = 50;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Ending(string msg, int water, int destroyed, bool win)
    {
        alertTxt.text = msg;

        waterGathered.text = water.ToString();
        targetsDestroyed.text = destroyed.ToString();

        if(win)
        {
            winBonus.text = winBonusAmnt.ToString();
            int earned = water + destroyed + winBonusAmnt;
            waterEarned.text = earned.ToString();
            if (scr_dataPersistenceManager.instance != null)
            {
                scr_dataPersistenceManager.instance.playerData.currency += earned;
            }
        }
        else
        {
            winBonus.text = (-winBonusAmnt).ToString();
            int earned = water + destroyed - winBonusAmnt;
            waterEarned.text = earned.ToString();
            if(scr_dataPersistenceManager.instance != null)
            {
                scr_dataPersistenceManager.instance.playerData.currency += earned;
            }
        }
    }
}
