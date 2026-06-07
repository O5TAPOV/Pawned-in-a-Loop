using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class ClientAI : MonoBehaviour
{
    public enum ClientRole { Seller, Buyer }
    public ClientRole Role;
    public ItemData DesiredItem;
    [SerializeField] private TextMeshProUGUI roleText;

    public enum ClientState { WalkingIn, Waiting, WalkingOut }
    private ClientState currentState;

    private Transform counterPoint;
    private NavMeshAgent agent;

    public ItemData ItemToSell;
    private Transform itemSpawnPoint;
    private Transform exitPoint;

    void Awake()
    {
        currentState = ClientState.WalkingIn;
        agent = GetComponent<NavMeshAgent>();

        if (roleText != null)
        {
            roleText.text = "";
        }

        EventManager.NewClient(this);
    }

    void Update()
    {
        switch (currentState)
        {
            case ClientState.WalkingIn:
                if (!agent.pathPending && agent.remainingDistance <= 0.2f)
                {
                    currentState = ClientState.Waiting;
                    if (Role == ClientRole.Seller)
                    {
                        roleText.text = $"I brought {ItemToSell.ItemName}.";
                        GameObject spawnedObj = Instantiate(ItemToSell.ItemPrefab, itemSpawnPoint.position, itemSpawnPoint.rotation);
                        PawnshopItem spawnedItem = spawnedObj.GetComponent<PawnshopItem>();

                        // ÊÐÈ×ÈÌÎ Â ÐÀÄ²Î:
                        EventManager.ItemOnDesk(spawnedItem);
                    }
                    else if (Role == ClientRole.Buyer)
                    {
                        EventManager.BuyerArrived(DesiredItem);
                        roleText.text = "I wanna... " + DesiredItem.ItemName;
                    }
                }
                break;
            case ClientState.Waiting:
                break;

            case ClientState.WalkingOut:
                if(!agent.pathPending && agent.remainingDistance <= 0.2f)
                {
                    EventManager.ClientLeft();
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void GoAway()
    {
        currentState = ClientState.WalkingOut;
        agent.SetDestination(exitPoint.position);
    }

    public void SetupPoints(Transform counter, Transform itemSpawn, Transform exit)
    {
        counterPoint = counter;
        itemSpawnPoint = itemSpawn;
        exitPoint = exit;

        if (counterPoint != null)
        {
            agent.SetDestination(counterPoint.position);
        }
        else
        {
            Debug.Log("Åé, òè çàáóâ âêàçàòè òî÷êó CounterPoint äëÿ êë³ºíòà!");
        }
    }
}
