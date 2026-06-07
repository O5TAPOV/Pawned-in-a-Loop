using System;

public static class EventManager
{
    public static event Action<PawnshopItem> OnItemBought;
    public static event Action OnClientLeft;
    public static event Action<ClientAI> OnNewClient;
    public static event Action<int> OnBalanceChanged;
    public static event Action OnShopClosing;
    public static event Action OnNewDayStarted;
    public static event Action<PawnshopItem> OnItemHovered;
    public static event Action OnItemHoverExited;
    public static event Action<PawnshopItem> OnItemOnDesk;
    public static event Action<bool> OnCameraTurned;
    public static event Action<ItemData> OnBuyerArrived;
    public static event Action<PawnshopItem> OnItemClicked;
    public static event Action<ItemData> OnItemRemoved;

    public static void ItemBought(PawnshopItem item){ OnItemBought?.Invoke(item); }
    public static void ClientLeft() { OnClientLeft?.Invoke(); }
    public static void NewClient(ClientAI client) { OnNewClient?.Invoke(client); }
    public static void BalanceChanged(int newBalance) { OnBalanceChanged?.Invoke(newBalance); }
    public static void ShopClosing() { OnShopClosing?.Invoke(); }
    public static void NewDayStarted() { OnNewDayStarted?.Invoke(); }
    public static void ItemHovered(PawnshopItem item) { OnItemHovered?.Invoke(item); }
    public static void ItemHoverExited() { OnItemHoverExited?.Invoke(); }
    public static void ItemOnDesk(PawnshopItem item) { OnItemOnDesk?.Invoke(item); }
    public static void CameraTurned(bool isFacingShelf) { OnCameraTurned?.Invoke(isFacingShelf); }
    public static void BuyerArrived(ItemData desiredItem) { OnBuyerArrived?.Invoke(desiredItem); }
    public static void ItemClicked(PawnshopItem item) { OnItemClicked?.Invoke(item); }
    public static void ItemRemoved(ItemData item) { OnItemRemoved?.Invoke(item); }
}
