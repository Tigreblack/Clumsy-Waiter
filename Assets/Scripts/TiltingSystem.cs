using UnityEngine;
using UnityEngine.Events;

public class TiltingSystem : MonoBehaviour
{
    [SerializeField] float tiltIncreaseRate = 5f;
    [SerializeField] float maxTilt = 100f;
    [SerializeField] float dropThreshold = 95f;
    [SerializeField] float movingMultiplier = 1.5f;
    [SerializeField] float movementThreshold = 0.1f;
    
    public UnityEvent onPlateDrop;
    
    float currentTilt;
    Vector3 lastPosition;
    PlatePickup platePickup;

    public float CurrentTilt => currentTilt;
    public float TiltPercentage => currentTilt / maxTilt;
    public bool IsHoldingFood { get; private set; }

    void Start()
    {
        lastPosition = transform.position;
        platePickup = GetComponent<PlatePickup>();
    }

    void Update()
    {
        UpdateHoldingState();
        
        if (!IsHoldingFood)
        {
            currentTilt = 0f;
            return;
        }

        float speed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;

        float multiplier = speed > movementThreshold ? movingMultiplier : 1f;
        currentTilt += tiltIncreaseRate * multiplier * Time.deltaTime;
        currentTilt = Mathf.Clamp(currentTilt, 0f, maxTilt);

        if (currentTilt >= dropThreshold)
            DropPlate();
    }

    public void ReduceTilt(float amount)
    {
        currentTilt = Mathf.Max(0f, currentTilt - amount);
    }

    void UpdateHoldingState()
    {
        GameObject plate = GetPlate();
        if (plate != null)
        {
            PlateSlotManager slots = plate.GetComponent<PlateSlotManager>();
            IsHoldingFood = slots != null && slots.GetItems().Count > 0;
        }
        else
        {
            IsHoldingFood = false;
        }
    }

    GameObject GetPlate()
    {
        if (platePickup == null) return null;
            
        var field = typeof(PlatePickup).GetField("plate", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
        return field?.GetValue(platePickup) as GameObject;
    }

    void DropPlate()
    {
        GameObject plate = GetPlate();
        if (plate == null) return;

        var field = typeof(PlatePickup).GetField("plate", 
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(platePickup, null);

        plate.transform.SetParent(null);
        
        Rigidbody rb = plate.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        Destroy(plate, 0.1f);
        
        onPlateDrop?.Invoke();
        currentTilt = 0f;
        IsHoldingFood = false;
    }
}
