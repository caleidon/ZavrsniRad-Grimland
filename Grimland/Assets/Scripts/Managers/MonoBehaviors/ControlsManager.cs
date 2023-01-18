using UnityEngine;

public class ControlsManager : MonoBehaviour
{
    private CameraManager cameraManager;
    private MenuManager menuManager;

    public static Controls Controls { get; private set; }

    private void Awake()
    {
        Controls = new Controls();

        cameraManager = FindObjectOfType<CameraManager>();
        menuManager = FindObjectOfType<MenuManager>();

        // Camera controls
        Controls.CameraControls.Camera_Zoom.performed += cameraManager.OnZoom;
        Controls.CameraControls.MousePoint.performed += cameraManager.OnPoint;

        // Time controls
        Controls.TimeControls.PauseOrResume.performed += TickManager.OnPauseOrResume;
        Controls.TimeControls.SetSpeedNormal.performed += TickManager.OnSetSpeedNormal;
        Controls.TimeControls.SetSpeedFast.performed += TickManager.OnSetSpeedFast;
        Controls.TimeControls.SetSpeedFastest.performed += TickManager.OnSetSpeedFastest;

        // Game controls
        Controls.GameControls.TogglePauseMenu.performed += menuManager.TogglePauseMenu;
        Controls.GameControls.LeftClick.performed += InputManager.SpawnWater;
        Controls.GameControls.RightClick.performed += InputManager.SpawnSoil;
        Controls.GameControls.Wood.performed += InputManager.SpawnWood;
        Controls.GameControls.Blueprint.performed += InputManager.SpawnBlueprint;
        Controls.GameControls.Human.performed += InputManager.SpawnHuman;
    }

    public static void EnableGameInput()
    {
        Controls.CameraControls.Enable();
        Controls.TimeControls.Enable();
    }

    public static void DisableGameInput()
    {
        Controls.CameraControls.Disable();
        Controls.TimeControls.Disable();
    }

    private void OnDisable()
    {
        Controls.CameraControls.Disable();
        Controls.TimeControls.Disable();
        Controls.GameControls.Disable();
    }
}