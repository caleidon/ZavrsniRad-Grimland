using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Random = UnityEngine.Random;
#if UNITY_EDITOR
using UnityEditor;
#endif

public static class Utilities
{
    public static Vector2 MousePosition()
    {
        return Mouse.current.position.ReadValue();
    }

    public static Vector2 MouseWorldPosition()
    {
        return CameraManager.ActualCamera.ScreenToWorldPoint(MousePosition());
    }

    public static void ShowGizmoText(string text, Vector3 node, Color color, int fontSize = 15, bool bold = true)
    {
#if UNITY_EDITOR
        Handles.Label(node + Vector3Int.up, text,
            new GUIStyle { fontSize = fontSize, fontStyle = bold ? FontStyle.Bold : FontStyle.Normal, normal = new GUIStyleState { textColor = color } });
#endif
    }

    public static Color RandomColor()
    {
        Color color = new Color(
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            Random.Range(0f, 1f),
            1f
        );

        return color;
    }

    public static Color ColorFromHash(int hash)
    {
        var bytes = BitConverter.GetBytes(hash);
        Color32 color = new Color32(
            bytes[0],
            bytes[1],
            bytes[2],
            255
        );

        return RemapValue(color, 0.9f, 0.02f);
    }

    public static Color RemapValue(Color color, float saturation, float brightness)
    {
        Color.RGBToHSV(color, out var h, out var s, out var v);
        return Color.HSVToRGB(h, Mathf.Lerp(saturation, 1, s), Mathf.Lerp(brightness, 0.9f, v));
    }


    public static T[] PopulateArray<T>(this T[] arr, T value)
    {
        for (int i = 0; i < arr.Length; i++)
        {
            arr[i] = value;
        }

        return arr;
    }
}