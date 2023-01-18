using System.Collections.Generic;
using UnityEngine;

public class ButtonListControl : MonoBehaviour
{
    [SerializeField] private GameObject loadButtonTemplate;

    private List<int> intList;

    private void Start()
    {
        // DirectoryInfo dir = new DirectoryInfo(Application.persistentDataPath + "/Saves");
        //
        // FileInfo[] saveArray = dir.GetFiles("*.*");
        // foreach (FileInfo save in saveArray)
        // {
        //     GameObject button = Instantiate(loadButtonTemplate, loadButtonTemplate.transform.parent, false);
        //     button.SetActive(true);
        //
        //     button.GetComponent<LoadListButton>().SetText(save.Name.Remove(save.Name.Length - save.Extension.Length));
        // }
    }
}