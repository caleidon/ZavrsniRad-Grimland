using CommandTerminal;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject loadMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject pauseMenu;

    private void Awake()
    {
        Instance = this;
    }

    public void SetMainMenu(bool isEnabled)
    {
        mainMenu.SetActive(isEnabled);
    }

    public void SetStartMenu(bool isEnabled)
    {
        startMenu.SetActive(isEnabled);
    }

    public void SetLoadMenu(bool isEnabled)
    {
        loadMenu.SetActive(isEnabled);
    }

    public void SetOptionsMenu(bool isEnabled)
    {
        optionsMenu.SetActive(isEnabled);
    }

    public void SetPauseMenu(bool isEnabled)
    {
        pauseMenu.SetActive(isEnabled);
    }

    public void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (!Terminal.IsClosed) return;

        if (loadMenu.activeSelf)
        {
            SetLoadMenu(false);
            SetPauseMenu(true);
            return;
        }

        if (!pauseMenu.activeSelf)
        {
            TickManager.PauseGame();
            ControlsManager.DisableGameInput();

            SetPauseMenu(true);
            UIManager.Instance.SetGameUI(false);
        }
        else
        {
            SetPauseMenu(false);
            ControlsManager.EnableGameInput();
            UIManager.Instance.SetGameUI(true);
        }
    }


    public void ReturnFromLoadMenu()
    {
        if (mainMenu.activeSelf)
        {
            SetStartMenu(true);
        }
        else
        {
            SetPauseMenu(true);
        }
    }

    public void ReturnToMainMenu()
    {
        SetPauseMenu(false);
        SetMainMenu(true);
        SetStartMenu(true);
        UIManager.Instance.SetPausedImage(false);

        GameManager.DestroyWorldGrid();
    }
}