using System;
using TMPro;
using UnityEngine;

public class LoadingManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private TextMeshProUGUI loadingText;
    private static LoadingManager Instance { get; set; }

    public enum LoadingType
    {
        Generation,
        Loading,
        Defs,
        None
    }

    private void Awake()
    {
        Instance = this;
    }

    public static void SetLoading(bool isLoading, LoadingType loadingType = LoadingType.None)
    {
        switch (loadingType)
        {
            case LoadingType.Generation:
                Instance.loadingText.SetText("Generating map...");
                break;
            case LoadingType.Loading:
                Instance.loadingText.SetText("Loading map...");
                break;
            case LoadingType.Defs:
                Instance.loadingText.SetText("Initializing defs...");
                break;
            case LoadingType.None:
                Instance.loadingText.SetText("Loading screen error!");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(loadingType), loadingType, null);
        }

        Instance.loadingScreen.gameObject.SetActive(isLoading);
    }
}