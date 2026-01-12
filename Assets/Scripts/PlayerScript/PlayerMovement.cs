using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerControl : MonoBehaviour
{
    public float speed = 5f;
    public float rotationSpeed = 200f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        Vector3 move = transform.forward * v * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + move);

        Quaternion rot = Quaternion.Euler(0f, h * rotationSpeed * Time.fixedDeltaTime, 0f);
        rb.MoveRotation(rb.rotation * rot);
    }
}
