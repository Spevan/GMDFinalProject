using UnityEngine;

public class scr_destroy : MonoBehaviour
{
    public void OnDestroy()
    {
        Destroy(gameObject);
    }
}
