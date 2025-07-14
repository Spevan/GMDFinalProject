using TMPro;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class scr_deckEdit : MonoBehaviour
{
    public scr_deck deckData;
    public GameObject nameField;
    string path = "Assets/Scriptable Objects/Decks/";

    private void Start()
    {
        if(nameField != null)
        {
            nameField.GetComponent<TMP_InputField>().onEndEdit.AddListener(delegate { EditDeckName(nameField.GetComponent<TMP_InputField>().text); });
        }
    }

    public void EditDeckName(string name)
    {
        AssetDatabase.RenameAsset(path + "temp.asset", name + ".asset");
        AssetDatabase.Refresh();
        deckData.name = name;
    }
}
