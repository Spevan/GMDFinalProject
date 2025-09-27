using TMPro;
using UnityEngine;

public class scr_alertText : MonoBehaviour
{
    float timeRemaining;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = 4;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0.1f, 0);
        if(timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            this.GetComponent<TextMeshProUGUI>().CrossFadeAlpha(0, timeRemaining, true);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ChangeText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
