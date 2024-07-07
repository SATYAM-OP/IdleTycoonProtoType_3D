using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UnlockRoom : MonoBehaviour
{

    public int purchaseValue = 20;
    public GameObject parent;
    public Image fillImage;

    public  float PURCHASE_TIME=3;
    public TextMeshProUGUI purchaseText;
    private Coroutine cleaningCoroutine;

    public UnityAction RoomUnlocked;

    private void Start()
    {
        purchaseText.text= purchaseValue.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cleaningCoroutine == null && CurrencyManager.instance.CanSpend(purchaseValue))
            {
                cleaningCoroutine = StartCoroutine(StartPurchase());
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

    private IEnumerator StartPurchase()
    {
        float elapsedTime = 0f;

        while (elapsedTime < PURCHASE_TIME)
        {
            elapsedTime += Time.deltaTime;
            fillImage.fillAmount = Mathf.Clamp01(elapsedTime / PURCHASE_TIME);
            yield return null;
        }

        // Cleaning complete
        CurrencyManager.instance.SpendMoney(purchaseValue);
        RoomUnlocked?.Invoke();
        fillImage.fillAmount = 0;
        parent.SetActive(false);
        cleaningCoroutine = null;
    }
}
