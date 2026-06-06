using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public PickupItemData itemData;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemData.Use(other.gameObject);
            Destroy(gameObject);
        }
    }
}
