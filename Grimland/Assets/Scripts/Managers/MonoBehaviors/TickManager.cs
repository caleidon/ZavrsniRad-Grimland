using System;
using BehaviorDesigner.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

public class TickManager : MonoBehaviour
{
    private enum SpeedPresets
    {
        Paused = 0,
        Normal = 1,
        Fast = 2,
        Fastest = 5
    }

    public static ulong Tick { get; set; }
    private static float tickTimer;
    private const float TICK_INTERVAL = 0.2f;

    private static bool _isPaused;

    public static event Action OnTick;
    public static event Action OnMediumTick;
    public static event Action OnLongTick;
    public static event Action<bool> OnPause;

    private void Update()
    {
        tickTimer += Time.deltaTime;

        while (tickTimer >= TICK_INTERVAL)
        {
            BehaviorManager.instance.Tick();

            tickTimer -= TICK_INTERVAL;
            Tick++;

            // Happens every 0.2 seconds.
            OnTick?.Invoke();

            if (Tick % (5 / TICK_INTERVAL) == 0) // Happens every 5 seconds.
            {
                OnMediumTick?.Invoke();
            }

            if (Tick % (30 / TICK_INTERVAL) == 0) // Happens every 30 seconds.
            {
                OnLongTick?.Invoke();
            }
        }
    }

    public static void OnPauseOrResume(InputAction.CallbackContext context)
    {
        if (_isPaused)
        {
            ResumeGame();
            return;
        }

        PauseGame();
    }

    public static void OnSetSpeedNormal(InputAction.CallbackContext context)
    {
        SetGameSpeed((int)SpeedPresets.Normal);
    }

    public static void OnSetSpeedFast(InputAction.CallbackContext context)
    {
        SetGameSpeed((int)SpeedPresets.Fast);
    }

    public static void OnSetSpeedFastest(InputAction.CallbackContext context)
    {
        SetGameSpeed((int)SpeedPresets.Fastest);
    }

    public static void PauseGame()
    {
        SetGameSpeed((int)SpeedPresets.Paused);
    }

    public static void ResumeGame()
    {
        SetGameSpeed((int)SpeedPresets.Normal);
    }

    private static void SetGameSpeed(int gameSpeedMultiplier)
    {
        Time.timeScale = gameSpeedMultiplier;
        _isPaused = gameSpeedMultiplier <= 0;

        OnPause?.Invoke(_isPaused);
    }

    public static void ResetTickManager()
    {
        Tick = 0;
        OnTick = null;
        OnMediumTick = null;
        OnLongTick = null;
        OnPause = null;
    }
}

public class TickManagerSaver
{
    public ulong Tick { get; set; }

    public TickManagerSaver()
    {
        Tick = TickManager.Tick;
    }
}