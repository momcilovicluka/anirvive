using UnityEngine;

public class LookPlayer : MonoBehaviour
{
    [SerializeField]
    private Transform character;

    public float sensitivity = 2;
    public float smoothing = 1.5f;

    private Vector2 velocity;
    private Vector2 frameVelocity;

    private void Reset()
    {
        character = GetComponentInParent<PlayerMovementController>().transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (InventorySystem.Instance.isOpen || CraftingSystem.Instance.isOpen)
        {
            character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
            return;
        }

        // Get smooth velocity.
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}