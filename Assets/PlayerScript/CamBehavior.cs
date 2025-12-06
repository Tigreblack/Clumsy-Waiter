using UnityEngine;

public class CamBehavior : MonoBehaviour
{
    public Transform target;      // Le joueur à suivre
    public Vector3 offset = new Vector3(0, 5, -7); // Position relative à garder
    public float smoothSpeed = 5f; // Vitesse de lissage

    public float mouseSensitivity = 100f;
    private float yaw = 0f;
    private float pitch = 0f;


    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -30f, 60f);

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }

    // Caméra automatique 

    ///void LateUpdate()
    //{
    // Position désirée
    //Vector3 desiredPosition = target.position + offset;

    // Lissage
    //transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

    // garder la caméra orientée vers le joueur
    //transform.rotation = Quaternion.LookRotation(target.position - transform.position);


    ///}


}
