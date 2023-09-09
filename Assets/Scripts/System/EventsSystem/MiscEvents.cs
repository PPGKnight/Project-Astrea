using System;
public class MiscEvents
{
    public event Action<Item> onItemPicked;
    public void ItemPicked(Item t) => onItemPicked?.Invoke(t);
}
