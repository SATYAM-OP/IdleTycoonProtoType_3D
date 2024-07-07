using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CurrencyManager : MonoBehaviour
{
    // The static variable to hold the amount of money
    public int money;
    public static UnityAction<int> OnChangeMoney;
    public static CurrencyManager instance;
    public const string MONEY_KEY = "MoneyKey";

    public void Awake()
    {
        if (instance == null)
        {
         instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        money = PlayerPrefs.GetInt(MONEY_KEY, 0);
    }

    // Public static method to get the current amount of money
    public  int GetMoney()
    {
        return money;
    }

    // Public static method to add money
    public  void AddMoney(int amount)
    {
        money += amount;
        OnChangeMoney?.Invoke(money);
    }

    // Public static method to subtract money
    public  void SpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            OnChangeMoney?.Invoke(money);
        }
    }

    public  bool CanSpend(int amount)
    {
        return money >= amount;
    }

    public void OnDestroy()
    {
        PlayerPrefs.SetInt(MONEY_KEY, money);
        PlayerPrefs.Save();
    }

    // Optionally, you can add a method to set the money value (e.g., for initializing or resetting)
    public  void SetMoney(int amount)
    {
        money = amount;
        OnChangeMoney?.Invoke(money);
        Debug.Log("Money set to: " + money);
    }

}
