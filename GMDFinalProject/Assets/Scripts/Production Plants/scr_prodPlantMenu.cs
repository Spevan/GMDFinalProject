using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.EventSystems;

public class scr_prodPlantMenu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, scr_IDataPersistence
{
    public scr_productionPlant plantData;
    public GameObject GUI, details, temp;

    public void LoadData(scr_playerData playerData)
    {
        foreach (scr_productionPlant plant in playerData.productionPlants)
        {
            for(int i = 0; i < playerData.productionPlants.Count; i++)
            {
                if(plant.plant_id == i && !plant.loaded)
                {
                    plant.loaded = true;
                    plantData = plant;
                    return;
                }
            }
        }
    }

    public void SaveData(ref scr_playerData data)
    {
        plantData.loaded = false;
        plantData.plant_id = data.productionPlants.Count;
        data.productionPlants.Add(plantData);
        scr_dataPersistenceManager.instance.SaveGame();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        temp = Instantiate(details, GUI.transform);
        temp.GetComponent<scr_prodPlantDetails>().plantData = plantData;
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

    private void OnDestroy()
    {
        plantData.loaded = false;
    }

    private void OnApplicationQuit()
    {
        OnDestroy();
    }
}
