using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class scr_buttonToggle : MonoBehaviour
{
    public List<GameObject> objOff, objOn;
    public void Toggle()
    {
        foreach (GameObject obj in objOn)
        {
            if (obj != null)
            {
                obj.SetActive(true);
            }
        }

        foreach (GameObject obj in objOff)
        {
            if (obj != null)
            {
                obj.SetActive(false);
            }
        }
    }
}
