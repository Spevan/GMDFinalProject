using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface scr_IDataPersistence
{
    void LoadData(scr_playerData gameData);

    void SaveData(ref scr_playerData data);
}