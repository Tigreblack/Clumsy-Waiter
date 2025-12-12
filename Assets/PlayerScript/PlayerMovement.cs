using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public float speed = 5f; // vitesse de déplacement
    public float rotationSpeed = 200f; // vitesse de rotation
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // bloque la rotation
    }
    void FixedUpdate()
    {
        // Récupère les inputs clavier
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Déplacement
        Vector3 move = transform.forward * vertical * speed * Time.deltaTime;
        rb.MovePosition(rb.position + move);

        // Rotation vers la direction du mouvement
        float rotation = horizontal * rotationSpeed * Time.deltaTime;
        Quaternion q_rotation = Quaternion.Euler(0f, rotation, 0f);
        rb.MoveRotation(rb.rotation * q_rotation);

    }


}
