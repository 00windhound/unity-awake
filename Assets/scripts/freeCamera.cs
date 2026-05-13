using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float lookSpeed = 2f;

    float rotationX = 0f;
    float rotationY = 0f;

    void Update()
    {
        // Mouse look
        rotationX += Input.GetAxis("Mouse X") * lookSpeed;
        rotationY -= Input.GetAxis("Mouse Y") * lookSpeed;

        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotationY, rotationX, 0);

        // WASD movement
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        transform.position += move * moveSpeed * Time.deltaTime;

        // Up
        if (Input.GetKey(KeyCode.Space))
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }

        // Down
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }
}