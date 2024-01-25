using System;
public class MiscEvents
{
    public event Action<Item> onItemPicked;
    public void ItemPicked(Item t) => onItemPicked?.Invoke(t);

    public event Action onOptionKeyPressed;
    public void OptionKeyPressed() => onOptionKeyPressed?.Invoke();

    public event Action onDeathScreen;
    public void DeathScreen() => onDeathScreen?.Invoke();
}
