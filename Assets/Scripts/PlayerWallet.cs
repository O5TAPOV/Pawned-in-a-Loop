using UnityEngine;

public class PlayerWallet : MonoBehaviour
{
    private int money;
    public bool TrySpendMoney(int amount)
    {
        if (money >= amount)
        {
            money -= amount;
            EventManager.BalanceChanged(money);
            Debug.Log($"Куплено! Залишок грошей: {money}");
            return true;
        }
        else
        {
            Debug.Log($"Недостатньо грошей!");
            return false;
        }
    }

    public void AddMoney(int amount)
    {
        if (amount < 0) return;
        money += amount;
        EventManager.BalanceChanged(money);
        Debug.Log($"\"Продано! Заробили: {amount}");
    }

    public int GetCurrentMoney() => money;

    private void Start()
    {
        money = GlobalState.MoneySnapshot;
        EventManager.BalanceChanged(money);
    }
}
