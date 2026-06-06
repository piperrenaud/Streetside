using UnityEngine;
using UnityEngine.Diagnostics;

[CreateAssetMenu(fileName = "CoinItem", menuName = "Items/Coin Item")]
public class CoinItem : PickupItemData
{
    public int coinValue = 1;

    public override void Use(GameObject item)
    {
        PlayerCurrency currency = item.GetComponentInParent<PlayerCurrency>();

        if (currency != null )
        {
            currency.AddCoins(coinValue);
        }
    }
}
