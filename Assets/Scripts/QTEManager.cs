using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class QTEManager : MonoBehaviour
{
    [SerializeField] KeyCode[] possibleKeys = { KeyCode.K, KeyCode.H, KeyCode.T, KeyCode.J, KeyCode.L };
    [SerializeField] float qteInterval = 3f;
    [SerializeField] float qteTimeWindow = 2f;
    [SerializeField] float tiltReductionOnSuccess = 30f;
    [SerializeField] float tiltIncreaseOnFail = 15f;
    [SerializeField] TiltingSystem tiltingSystem;
    
    public UnityEvent<KeyCode> onQTEStart;
    public UnityEvent onQTESuccess;
    public UnityEvent onQTEFail;
    public UnityEvent onQTEEnd;

    KeyCode currentKey;
    bool qteActive;
    float qteTimer;
    Coroutine qteCoroutine;

    public KeyCode CurrentKey => currentKey;
    public bool IsQTEActive => qteActive;
    public float QTETimeRemaining => qteTimer;
    public float QTETimePercentage => qteActive ? qteTimer / qteTimeWindow : 0f;

    void Start()
    {
        if (tiltingSystem == null)
            tiltingSystem = GetComponent<TiltingSystem>();
    }

    void Update()
    {
        if (tiltingSystem != null && tiltingSystem.IsHoldingFood && qteCoroutine == null)
        {
            qteCoroutine = StartCoroutine(QTELoop());
        }
        else if ((tiltingSystem == null || !tiltingSystem.IsHoldingFood) && qteCoroutine != null)
        {
            StopCoroutine(qteCoroutine);
            qteCoroutine = null;
            EndQTE();
        }

        if (qteActive)
        {
            qteTimer -= Time.deltaTime;

            if (Input.GetKeyDown(currentKey))
                QTESuccess();
            else if (qteTimer <= 0f)
                QTEFail();
        }
    }

    IEnumerator QTELoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(qteInterval);
            
            if (tiltingSystem != null && tiltingSystem.IsHoldingFood)
                StartQTE();
        }
    }

    void StartQTE()
    {
        currentKey = possibleKeys[Random.Range(0, possibleKeys.Length)];
        qteActive = true;
        qteTimer = qteTimeWindow;
        onQTEStart?.Invoke(currentKey);
    }

    void QTESuccess()
    {
        tiltingSystem?.ReduceTilt(tiltReductionOnSuccess);
        onQTESuccess?.Invoke();
        EndQTE();
    }

    void QTEFail()
    {
        tiltingSystem?.ReduceTilt(-tiltIncreaseOnFail);
        onQTEFail?.Invoke();
        EndQTE();
    }

    void EndQTE()
    {
        qteActive = false;
        qteTimer = 0f;
        onQTEEnd?.Invoke();
    }
}
