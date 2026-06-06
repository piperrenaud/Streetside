using TMPro;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    [SerializeField] private int coins = 0;
    [SerializeField] private TMP_Text currencyText;

    private void Start()
    {
        UpdateUI();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        currencyText.text = $"Coins: {coins}";
    }

    public int GetCoins()
    {
        return coins;
    }
}
