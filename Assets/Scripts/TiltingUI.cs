using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TiltingUI : MonoBehaviour
{
    [SerializeField] GameObject tiltBarPanel;
    [SerializeField] Image tiltFillImage;
    [SerializeField] Gradient tiltColorGradient;
    [SerializeField] GameObject qtePanel;
    [SerializeField] TextMeshProUGUI qteKeyText;
    [SerializeField] Image qteTimerImage;
    [SerializeField] TiltingSystem tiltingSystem;
    [SerializeField] QTEManager qteManager;
    [SerializeField] float shakeDuration = 0.3f;
    [SerializeField] float shakeIntensity = 10f;

    Vector3 originalQTEPosition;

    void Start()
    {
        if (tiltingSystem == null)
            tiltingSystem = FindFirstObjectByType<TiltingSystem>();
            
        if (qteManager == null)
            qteManager = FindFirstObjectByType<QTEManager>();

        if (qtePanel != null)
        {
            originalQTEPosition = qtePanel.transform.localPosition;
            qtePanel.SetActive(false);
        }

        if (tiltBarPanel != null)
            tiltBarPanel.SetActive(false);

        if (qteManager != null)
        {
            qteManager.onQTEStart.AddListener(OnQTEStart);
            qteManager.onQTESuccess.AddListener(OnQTESuccess);
            qteManager.onQTEFail.AddListener(OnQTEFail);
            qteManager.onQTEEnd.AddListener(OnQTEEnd);
        }
    }

    void Update()
    {
        if (tiltingSystem != null)
        {
            bool shouldShow = tiltingSystem.IsHoldingFood;
            if (tiltBarPanel != null)
                tiltBarPanel.SetActive(shouldShow);

            if (shouldShow && tiltFillImage != null)
            {
                float percentage = tiltingSystem.TiltPercentage;
                tiltFillImage.fillAmount = percentage;
                if (tiltColorGradient != null)
                    tiltFillImage.color = tiltColorGradient.Evaluate(percentage);
            }
        }

        if (qteManager != null && qteManager.IsQTEActive && qteTimerImage != null)
            qteTimerImage.fillAmount = qteManager.QTETimePercentage;
    }

    void OnQTEStart(KeyCode key)
    {
        if (qtePanel != null)
        {
            qtePanel.SetActive(true);
            qtePanel.transform.localPosition = originalQTEPosition;
        }

        if (qteKeyText != null)
            qteKeyText.text = key.ToString();

        if (qteTimerImage != null)
            qteTimerImage.fillAmount = 1f;
    }

    void OnQTESuccess()
    {
        if (qteKeyText != null)
            qteKeyText.color = Color.green;
    }

    void OnQTEFail()
    {
        if (qteKeyText != null)
            qteKeyText.color = Color.red;
        
        if (qtePanel != null)
            StartCoroutine(ShakeQTE());
    }

    void OnQTEEnd()
    {
        if (qtePanel != null)
            qtePanel.SetActive(false);
            
        if (qteKeyText != null)
            qteKeyText.color = Color.white;
    }

    IEnumerator ShakeQTE()
    {
        float elapsed = 0f;
        while (elapsed < shakeDuration)
        {
            float x = Random.Range(-shakeIntensity, shakeIntensity);
            float y = Random.Range(-shakeIntensity, shakeIntensity);
            qtePanel.transform.localPosition = originalQTEPosition + new Vector3(x, y, 0f);
            elapsed += Time.deltaTime;
            yield return null;
        }
        qtePanel.transform.localPosition = originalQTEPosition;
    }

    void OnDestroy()
    {
        if (qteManager != null)
        {
            qteManager.onQTEStart.RemoveListener(OnQTEStart);
            qteManager.onQTESuccess.RemoveListener(OnQTESuccess);
            qteManager.onQTEFail.RemoveListener(OnQTEFail);
            qteManager.onQTEEnd.RemoveListener(OnQTEEnd);
        }
    }
}
