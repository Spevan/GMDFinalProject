using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface scr_IDataPersistence
{
    public void LoadData(scr_playerData gameData);

    public void SaveData(ref scr_playerData data);
}