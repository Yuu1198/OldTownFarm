using System;
using UnityEngine;

public class PlayerMoneyManager : MonoBehaviour
{
    public static PlayerMoneyManager instance;

    [SerializeField] public Currency playerMoney;

    public event Action OnMoneyChanged;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        // Startmoney
        playerMoney = new Currency(0, 0, 0);
    }

    public void AddMoney(Currency amount)
    {
        playerMoney.Add(amount);
        OnMoneyChanged?.Invoke();
    }

    public void SubstractMoney(Currency amount)
    {
        playerMoney.Substract(amount);
        OnMoneyChanged?.Invoke();
    }

    public string GetMoneyString()
    {
        return playerMoney.GetMoneyString();
    }

    public int GetTotalCopper()
    {
        return playerMoney.ToCopper();
    }
}
