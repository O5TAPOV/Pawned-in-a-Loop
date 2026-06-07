using TMPro;
using UnityEngine;

public class PawnshopUI : MonoBehaviour
{
    [SerializeField] private PlayerWallet wallet;
    [SerializeField] private PlayerInteraction interaction;

    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private GameObject actionButtonsPanel;

    [SerializeField] private ClientAI currentClient;
    private PawnshopItem currentDeskItem;
    private ItemData currentDesiredItem;

    public void BuyItem()
    {
        if (currentDeskItem == null) return;
        int price = currentDeskItem.PurchasePrice;

        if (wallet.TrySpendMoney(price))
        {
            currentDeskItem.IsOnShelf = true;
            // 1. Кричимо в радіо, передаємо ВЕСЬ об'єкт
            EventManager.ItemBought(currentDeskItem);

            // 3. Забуваємо предмет, бо ми його вже купили
            currentDeskItem = null;

            if (currentClient != null)
            {
                currentClient.GoAway();
                currentClient = null; // Одразу забуваємо його, щоб не натиснути кнопку двічі!
            }
        }
    }

    public void RefuseClient()
    {
        if (currentClient == null) return; // Нема клієнта - нікого виганяти

        // Якщо це був ПРОДАВЕЦЬ (на столі щось лежить) - знищуємо цей предмет
        if (currentDeskItem != null)
        {
            Destroy(currentDeskItem.gameObject);
            currentDeskItem = null;
        }

        // Якщо це був ПОКУПЕЦЬ, currentDeskItem і так null, ми просто пропустили верхній if

        // Відпускаємо клієнта (спрацює для обох!)
        currentClient.GoAway();
        currentClient = null;
    }

    private void OnEnable()
    {
        EventManager.OnNewClient += NewClient;
        EventManager.OnBalanceChanged += UpdateBalanceUI;
        EventManager.OnItemOnDesk += HandleItemOnDesk;
        EventManager.OnCameraTurned += HandleCameraTurn;
        EventManager.OnBuyerArrived += HandleBuyerArrived;
        EventManager.OnItemClicked += HandleItemClicked;
    }

    private void OnDisable()
    {
        EventManager.OnNewClient -= NewClient;
        EventManager.OnBalanceChanged -= UpdateBalanceUI;
        EventManager.OnItemOnDesk -= HandleItemOnDesk;
        EventManager.OnCameraTurned -= HandleCameraTurn;
        EventManager.OnBuyerArrived -= HandleBuyerArrived;
        EventManager.OnItemClicked -= HandleItemClicked;
    }

    private void NewClient(ClientAI newClient)
    {
        currentClient = newClient;
    }

    private void UpdateBalanceUI(int newBalance)
    {
        balanceText.text = newBalance.ToString() + " $";
    }

    private void HandleItemOnDesk(PawnshopItem newItem)
    {
        currentDeskItem = newItem;

        Debug.Log($"UI побачило, що клієнт поклав: {newItem.Data.ItemName}");
    }

    private void HandleCameraTurn(bool isFacingShelf)
    {
        actionButtonsPanel.SetActive(!isFacingShelf);
    }

    private void HandleBuyerArrived(ItemData desiredItem)
    {
        currentDesiredItem = desiredItem;
    }

    private void HandleItemClicked(PawnshopItem clickedItem)
    {
        if (currentClient == null) return; // Нема клієнта - нічого не продаємо

        if (currentClient.Role != ClientAI.ClientRole.Buyer) return; // Якщо це продавець - ігноруємо кліки по шафі

        // Якщо те, на що ми клікнули, це те, що хоче покупець
        if (clickedItem.Data.ItemName == currentDesiredItem.ItemName)
        {
            // 1. Додаємо гроші в гаманець (сподіваюсь, ти вже додав метод AddMoney в PlayerWallet!)
            wallet.AddMoney(clickedItem.SellPrice);

            EventManager.ItemRemoved(clickedItem.Data);
            // 2. Знищуємо предмет (він проданий)
            Destroy(clickedItem.gameObject);
            EventManager.ItemHoverExited();

            // 3. Забуваємо бажання
            currentDesiredItem = null;

            // 4. Покупець задоволений йде геть
            currentClient.GoAway();
            currentClient = null;

            Debug.Log("ПРЕДМЕТ УСПІШНО ПРОДАНО!");
        }
        else
        {
            Debug.Log("Це не той предмет! Покупець хоче щось інше.");
        }
    }
}
