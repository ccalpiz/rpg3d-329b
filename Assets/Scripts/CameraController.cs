using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera cam;

    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [SerializeField] private Transform corner1;
    [SerializeField] private Transform corner2;

    [SerializeField] private float xInput;
    [SerializeField] private float zInput;
    private InputAction moveAction;
    private Vector2 moveValue;

    [Header("Zoom")]
    [SerializeField] private float zoomModifier;
    [SerializeField] private float zoomSpeed;

    private InputAction zoomAction;
    private Vector2 zoomValue;

    public static CameraController instance;

    void Awake()
    {
        instance = this;
        cam = Camera.main;
    }

    void Start()
    {
        moveSpeed = 25f;
        zoomSpeed = 0.05f;
        moveAction = InputSystem.actions.FindAction("Move");
        zoomAction = InputSystem.actions.FindAction("Zoom");
    }

    private void Update()
    {
        MoveByKB();
        Zoom();
    }

    private void MoveByKB()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        float xInput = moveValue.x;
        float zInput = moveValue.y;

        Vector3 dir = (transform.forward * zInput) + (transform.right * xInput);

        transform.position += dir * moveSpeed * Time.deltaTime;
        transform.position = Clamp(corner1.position, corner2.position);
    }

    private Vector3 Clamp(Vector3 lowerLeft, Vector3 topRight)
    {
        Vector3 pos = new Vector3(Mathf.Clamp(transform.position.x, lowerLeft.x, topRight.x), transform.position.y, Mathf.Clamp(transform.position.z, lowerLeft.z, topRight.z));

        return pos;
    }

    private void Zoom()
    {
        zoomValue = zoomAction.ReadValue<Vector2>();

        float scrollDelta = Mathf.Clamp(zoomValue.y, -1f, 1f);
        zoomModifier = scrollDelta * 5f;

        if (Keyboard.current.zKey.isPressed)
            zoomModifier = -1f;
        if (Keyboard.current.xKey.isPressed)
            zoomModifier = 1f;

        cam.orthographicSize += zoomModifier * zoomSpeed;
        cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, 4, 10);
    }


}
