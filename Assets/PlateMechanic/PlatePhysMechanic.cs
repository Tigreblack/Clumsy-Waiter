using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlatePhysMechanic : MonoBehaviour
{

    // variables public et private
    public float torqueForce = 10f; // force du plateau
    private float timer = 0f; // le timer essentielles à la mécnaique du plateau.
    public float angularDamping = 0; // Damping du plateau
    public float axisinc = 0; // Ne pas modifier cette valeur c'est le trackeur d'angle, pour modifier utilsier angleZ qui est une variable plus bas.

    // les listes
    Transform[] liste_plate;
    List<Transform> liste_emp;

    // emplacement des objets pour réaliser d'eventuelle chose pour plus tard.
    Transform emp_0 = null;
    Transform emp_1 = null;
    Transform emp_2 = null;
    Transform emp_3 = null;
    Transform emp_4 = null;

    // Object var
    GameObject plate = null; // Le plateau
    public Rigidbody rb; // RigidBody

    // Win_Defeat Initialisation et Utilisation du script.
    GameObject GameManager = null;
    private Win_Defeat win_Defeat0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb.angularDamping = angularDamping; //0.05

        string prefix1 = "emp";

        plate = GameObject.Find("Plate_V1");
        GameManager = GameObject.Find("GameManager");
        win_Defeat0 = GameManager.GetComponent<Win_Defeat>();

        // Hinge Limits Fixée

        // Setting pour l'hinge joint.

        HingeJoint hinge = plate.GetComponent<HingeJoint>();
        hinge.axis = Vector3.forward;
        JointLimits limits = hinge.limits;

        limits.min = -30f; // inclinaison max vers la droite
        limits.max = 30f;  // inclinaison max vers la gauche
        hinge.useLimits = true;
        hinge.limits = limits;

        // Initialisation

        liste_plate = plate.GetComponentsInChildren<Transform>();
        liste_emp = new List<Transform>();
        rb = GetComponent<Rigidbody>();
        
        // Boucles
        foreach (Transform transf in liste_plate)
        {

            if (transf.name.StartsWith(prefix1))
            {
                liste_emp.Add(transf);
            }
        }

        // Liste des emplacement 
        foreach (Transform t in liste_emp)
        {
            Debug.Log("Trnsf" + t.name);

            // color rouge
            t.GetComponent<Renderer>().material.color = Color.red;

            // GET les emplacements (assignation) / Switch CASE.

            for (int i = 0; i <= 4; i++)
            {
                switch (i)
                {
                    case 0:emp_0 = t; break;
                    case 1: emp_1 = t; break;
                    case 2: emp_2 = t; break;
                    case 3: emp_3 = t; break;
                    case 4: emp_4 = t; break;
                }
            }
        }

    }

    // Update is called once per frames
    void Update()
    {
        // Update la variable qui track la rotation
        float angleZ = plate.transform.localEulerAngles.z;

        // Convertir en [-180, 180]
        if (angleZ > 180f) angleZ -= 360f;

        axisinc = angleZ;


        // Zaxis_Auto_Update section. ---------- C'est ici que la mécanique du jeu prend place

        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            int rand = Random.Range(0,3);  // diificultée bascule

            // condition 1
            if (rand == 2)
            {
                rb.AddRelativeTorque(new Vector3(0f, 0f, -torqueForce));
            } 
            else if (rand == 1)
            {
                rb.AddRelativeTorque(new Vector3(0f, 0f, torqueForce));
            }

                timer = 0f; // reset timer
        }

        // Win_Defeat Condition

        if(angleZ <= -30 || angleZ >= 30) // Condition de défaite
        {

            // Pour éviter les erreurs not Found
            if (win_Defeat0 != null && win_Defeat0.Get_victory_status() != 3) 
            {
                win_Defeat0.Set_victory_status(2);
            }
        } 

        else  // Une condition supplémentaire si jamais ya quelque chose de prévu.
        {


        }      
    }


    private void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Torque avec touches pour tests à réadapter avec la mécanique de bascule.

        if (Input.GetKey(KeyCode.R))
        {
            rb.AddRelativeTorque(new Vector3(0f, 0f, -5));
        }

        if (Input.GetKey(KeyCode.E))
        {
            rb.AddRelativeTorque(new Vector3(0f, 0f, 5));
        }


        // de l'ancien code au chaos

        //rb.AddTorque(Vector3.left * horizontal * torqueForce * Time.deltaTime);
        //rb.AddTorque(Vector3.right * vertical * torqueForce * Time.deltaTime);
    }
}
