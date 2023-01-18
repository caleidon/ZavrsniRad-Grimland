using TMPro;
using UnityEngine;

public class LoadListButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI myText;

    public void SetText(string text)
    {
        myText.text = text;
    }

    public void OnClick()
    {
        GameManager.Instance.LoadGame(myText.text);
    }
}