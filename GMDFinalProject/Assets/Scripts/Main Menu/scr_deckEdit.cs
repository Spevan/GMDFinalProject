using UnityEngine;

public class scr_deckEdit : MonoBehaviour
{
    bool isVisible = true;
    Vector3 moveDistance = new Vector3(160, 0, 0);

    public void ToggleViewList()
    {
        if(isVisible)
        {
            transform.Translate(moveDistance);
            isVisible = false;
        }
        else
        {
            transform.Translate(-moveDistance);
            isVisible = true;
        }
    }
}
