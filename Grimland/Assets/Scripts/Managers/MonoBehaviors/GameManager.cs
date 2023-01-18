using Michsky.UI.ModernUIPack;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO: make wander choose any of 8 regions and only home area
    public static GameManager Instance { get; private set; }

    public CustomDropdown mapSizeDropdown;

    private void Awake()
    {
        Instance = this;
        // TickManager.PauseGame();
    }

    private void Start()
    {
        DefManager.LoadDefs();

        
    }

    public void StartNewGame()
    {
        int mapSize = int.Parse(mapSizeDropdown.selectedText.text.Split('x')[0]);

        DestroyWorldGrid();
        Map map = new Map(mapSize, mapSize);
        StartCoroutine(map.Generate());
    }

    public void LoadGame(string saveName)
    {
        DestroyWorldGrid();

        Map map = SaveManager.Load(saveName);
        StartCoroutine(map.LoadMap());
    }

    public void SaveGame()
    {
        TickManager.PauseGame();
        SaveManager.Save("testSave");
        TickManager.ResumeGame();
    }

    public static void DestroyWorldGrid()
    {
        if (Map.Instance != null)
        {
            Map.Destroy();
        }

        ControlsManager.DisableGameInput();
        ControlsManager.Controls.GameControls.Disable();
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}