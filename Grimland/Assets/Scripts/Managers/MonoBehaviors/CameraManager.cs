using CommandTerminal;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    // Zoom options
    [SerializeField] private float startingZoom = 15f;
    [SerializeField] private float maximumZoom = 5f;
    [SerializeField] private float minimumZoom = 100f;
    [SerializeField] private float zoomIncrement = 2.5f;
    [SerializeField] private float zoomSmoothness = 0.8f;

    // Camera movement options
    [SerializeField] private float cameraMoveIncrement = 10f;
    [SerializeField] private float cameraSmoothness = 0.15f;
    [SerializeField] private float zoomToMovementRatio = 3f;

    //Camera specific variables
    public static Camera ActualCamera { get; private set; }
    private Vector3 dragOrigin;

    //Movement variables
    private Vector3 moveTarget;
    private float currentZoom;

    private void Start()
    {
        ActualCamera = GetComponentInChildren<Camera>();
        ActualCamera.transform.position = Vector3.back;
        currentZoom = startingZoom;
    }

    public void OnZoom(InputAction.CallbackContext context)
    {
        if (!Terminal.IsClosed)
        {
            return;
        }

        // Adjust the current zoom value based on the direction of the scroll - this is clamped to our zoom min/max.
        currentZoom = Mathf.Clamp(currentZoom - context.ReadValue<Vector2>().y * zoomIncrement, maximumZoom,
            minimumZoom);
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        // Obtain current mouse position from context and convert it to world coords
        Vector2 screenPoint = context.ReadValue<Vector2>();
        Vector3 worldPoint = ActualCamera.ScreenToWorldPoint(screenPoint);

        // Check if middle mouse button is being held.
        if (Mouse.current.middleButton.isPressed)
        {
            // Calculate difference between current and previous mouse location
            Vector3 delta = dragOrigin - worldPoint;

            // Use that difference to set new camera position
            var cameraRigTransform = transform;
            var position = cameraRigTransform.position;
            position += delta;
            cameraRigTransform.position = position;
            moveTarget = position;

            // Adjust the worldPoint accordingly, because after the scroll it's now somewhere else
            worldPoint += delta;
        }

        // Update lastPoint for the next frame
        dragOrigin = worldPoint;
    }

    private void Update()
    {
        // Detect movement from movement keys (WASD and arrows)
        // If the console is open, ignore movement keys input
        if (!Terminal.IsClosed)
        {
            return;
        }

        // Check if input value from movement keys has changed and update the new move target
        Vector2 moveValue = ControlsManager.Controls.CameraControls.Camera_Move.ReadValue<Vector2>();
        moveTarget += (Vector3.right * moveValue.x + Vector3.up * moveValue.y) * (Time.unscaledDeltaTime * cameraMoveIncrement * (currentZoom / zoomToMovementRatio));

        // Calculate the lerpBlend for smooth movement regardless of framerate
        float lerpBlend = 1f - Mathf.Pow(1f - cameraSmoothness, Time.unscaledDeltaTime * 60);

        // Lerp the camera to a new move target position
        transform.position = Vector3.Lerp(transform.position, moveTarget, lerpBlend);

        // Lerp the zoom amount towards the new one
        ActualCamera.orthographicSize = Mathf.Lerp(ActualCamera.orthographicSize, currentZoom, lerpBlend * zoomSmoothness);
    }

    // OnZoomChange?.Invoke(this, new OnZoomChangeArgs {ZoomLevel = cam.orthographicSize});
    // public static event EventHandler<OnZoomChangeArgs> OnZoomChange;
    //
    //
    // public class OnZoomChangeArgs : EventArgs
    // {
    //     public float ZoomLevel;
    // }
}