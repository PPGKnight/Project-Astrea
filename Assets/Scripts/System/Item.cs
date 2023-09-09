using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] string Name;
    [SerializeField] string Description;
    [SerializeField] bool CanWear;
    [SerializeField] bool CanStack;
    [SerializeField] int StackSize;
    [SerializeField] int Quantity = 1;

    public string GetName { get { return Name; } }
    public int GetQuantity { get { return Quantity; } }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Kolizja z {Name} w liczbie {Quantity}");

        if (other.CompareTag("MainPlayer"))
            GameEventsManager.instance.MiscEvents.ItemPicked(this);
    }
}