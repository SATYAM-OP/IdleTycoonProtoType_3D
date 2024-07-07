using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI m_TextMeshPro;

    private void Start()
    {
        CurrencyManager.OnChangeMoney += UpdateText;
        UpdateText(CurrencyManager.instance.GetMoney());
    }

    public void UpdateText(int amount)
    {
        m_TextMeshPro.text = amount.ToString();
    }
}
