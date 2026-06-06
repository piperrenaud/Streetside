using UnityEngine;

public abstract class PickupItemData : ScriptableObject
{
    public string itemName;
    public abstract void Use(GameObject item);
}
