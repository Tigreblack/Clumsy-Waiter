using UnityEngine;

public class CamBehavior : MonoBehaviour
{
    public Transform target; // Le joueur à suivre

    public Vector3 distance = new Vector3(0, 5, -7); // Position relative à garder
    
    // ancien
    // public float smoothSpeed = 5f; // Vitesse de lissage

    [SerializeField] float minRotY = 0;
    [SerializeField] float maxRotY = 0;

    public float mouseSensitivity = 100f;
    private float camAngleX = 0f;
    private float pitch = 0f;


    void LateUpdate()
    {
        camAngleX += Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, minRotY, maxRotY);

        Quaternion rotation = Quaternion.Euler(pitch, camAngleX, 0);
        Vector3 desiredPosition = target.position + rotation * distance;

        transform.position = desiredPosition;
        transform.LookAt(target.position);
    }

    // Caméra automatique {anciennement}

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
