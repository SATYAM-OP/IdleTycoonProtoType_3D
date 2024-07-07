using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CleaningSpot : MonoBehaviour
{
    public GameObject parent;
    public Image fillImage;

    public bool isDirty;
    public UnityAction SpotCleaned;
    public const float CLEAN_TIME = 3f;

    private Coroutine cleaningCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cleaningCoroutine == null)
            {
                cleaningCoroutine = StartCoroutine(CleanSpot());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && cleaningCoroutine != null)
        {
            StopCoroutine(cleaningCoroutine);
            cleaningCoroutine = null;
            fillImage.fillAmount = 0;
            Debug.Log("Cleaning process interrupted.");
        }
    }

    private IEnumerator CleanSpot()
    {
        float elapsedTime = 0f;

        while (elapsedTime < CLEAN_TIME)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(elapsedTime / CLEAN_TIME);
            yield return null;
        }

        // Cleaning complete
        fillImage.fillAmount = 0;
        SpotCleaned?.Invoke();
        parent.SetActive(false);
        isDirty = true;
        cleaningCoroutine = null;
    }
}
