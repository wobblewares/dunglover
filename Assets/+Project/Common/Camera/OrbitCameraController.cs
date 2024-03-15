using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class OrbitCameraController : MonoBehaviour
{
    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float distanceMin = .5f;
    public float distanceMax = 15f;
    public float zoomSpeed = 5.0f;
    private new Rigidbody rigidbody;

    public bool allowGamepad = false;
    public bool useOrthographicZoom = false;

    public float positionSpeed = 10.0f;
    public float rotationSpeed = 10.0f;

    float x = 0.0f;
    float y = 0.0f;

    private Vector2 rotationInput = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

        rigidbody = GetComponent<Rigidbody>();

        // Make the rigid body not change rotation
        if (rigidbody != null)
        {
            rigidbody.freezeRotation = true;
        }
    }

    public void OnRotate(InputValue input)
    {
        rotationInput = input.Get<Vector2>();
    }

    void FixedUpdate()
    {
        if (target)
        {
            float scrollAxis = 0.0f;//Input.GetAxis("Mouse ScrollWheel");

            x += rotationInput.x * xSpeed * distance * 0.02f;
            y -= rotationInput.y * ySpeed * 0.02f;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);

            distance = Mathf.Clamp(distance - scrollAxis * zoomSpeed * Time.deltaTime, distanceMin, distanceMax);

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = rotation * negDistance + target.position;

            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            transform.position =  Vector3.Lerp( transform.position, position, Time.deltaTime * positionSpeed);

        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}