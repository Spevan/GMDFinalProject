using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class scr_editText : MonoBehaviour
{
    [SerializeField] GameObject textInput;
    public void editText()
    {
        if (textInput.GetComponent<TMP_InputField>().enabled)
        {
            textInput.GetComponent<TMP_InputField>().enabled = false;
        }
        else
        {
            textInput.GetComponent<TMP_InputField>().enabled = true;
        }
    }
}
