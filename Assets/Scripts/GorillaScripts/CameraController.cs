using UnityEngine;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/

/// <summary>
/// Camera movement script for third person games.
/// This Script should not be applied to the camera! It is attached to an empty object and inside
/// it (as a child object) should be your game's MainCamera.
/// </summary>
/// 
/// // EDITED BY LOGAN FOR ORBITAL CAMERA MOVEMENT
public class CameraController : MonoBehaviour
{

    [Tooltip("Enable to move the camera by holding the right mouse button. Does not work with joysticks.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel. Does not work with joysticks.")]
    public bool canZoom = true;
    [Space]
    [Tooltip("The higher it is, the faster the camera moves. It is recommended to increase this value for games that uses joystick.")]
    public float sensitivity = 5f;

    [Tooltip("Camera Y rotation limits. The X axis is the maximum it can go up and the Y axis is the maximum it can go down.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);

    float mouseX;
    float mouseY;

    [SerializeField]
    float offsetDistanceY;
    [SerializeField]
    int radius = 10;

    Transform player;

    void Start()
    {

        player = GameObject.FindWithTag("Player").transform;
        // offsetDistanceY = transform.position.y; dumb line in og script

        // Lock and hide cursor with option isn't checked
        if ( ! clickToMoveCamera )
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

    }


    void Update()
    {
        // Zoom (optional)
        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;

        // Only update rotation if clickToMoveCamera is off, or if right mouse button is held
        bool shouldRotate = !clickToMoveCamera || Input.GetAxisRaw("Fire2") != 0;

        if (shouldRotate)
        {
            mouseX += Input.GetAxis("Mouse X") * sensitivity;
            mouseY -= Input.GetAxis("Mouse Y") * sensitivity; // Invert Y for typical orbit
            mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);
        }

        // Calculate offset in spherical coordinates
        float yRadians = Mathf.Deg2Rad * mouseY;
        float xRadians = Mathf.Deg2Rad * mouseX;

        Vector3 offset = new Vector3(
            radius * Mathf.Cos(yRadians) * Mathf.Sin(xRadians),
            radius * Mathf.Sin(yRadians),
            radius * Mathf.Cos(yRadians) * Mathf.Cos(xRadians)
        );

        // Always follow player position
        transform.position = player.position + offset;

        // Maintain vertical offset
        Vector3 pos = transform.position;
        pos.y += offsetDistanceY;
        transform.position = pos;

        transform.LookAt(player.position);
    }
}