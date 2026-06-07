using UnityEngine;

public class ClientSpawner : MonoBehaviour
{
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private Transform counterPoint;
    [SerializeField] private Transform itemSpawnPoint;
    [SerializeField] private Transform exitPoint;

    [SerializeField] private ItemData[] allPossibleItems;

    private bool canSpawn = true;

    private void OnEnable()
    {
        EventManager.OnClientLeft += SpawnNextClient;
        EventManager.OnShopClosing += StopSpawning;
        EventManager.OnNewDayStarted += ResetSpawning;
    }

    private void OnDisable()
    {
        EventManager.OnClientLeft -= SpawnNextClient;
        EventManager.OnShopClosing -= StopSpawning;
        EventManager.OnNewDayStarted -= ResetSpawning;
    }

    private void SpawnNextClient()
    {
        if (!canSpawn) return;
        GameObject newClient = Instantiate(clientPrefab, transform.position, transform.rotation);
        ClientAI clientAI = newClient.GetComponent<ClientAI>();
        float randomRole = Random.Range(0f, 1f);
        if (randomRole <= 0.7f)
        {
            clientAI.Role = ClientAI.ClientRole.Seller;

            int rndIndex = Random.Range(0, allPossibleItems.Length);
            clientAI.ItemToSell = allPossibleItems[rndIndex];
        }
        else
        {
            clientAI.Role = ClientAI.ClientRole.Buyer;
            int rndIndex = Random.Range(0, allPossibleItems.Length);
            clientAI.DesiredItem = allPossibleItems[rndIndex];
        }

        clientAI.SetupPoints(counterPoint, itemSpawnPoint, exitPoint);
    }

    private void StopSpawning()
    {
        canSpawn = false;
    }

    private void ResetSpawning()
    {
        canSpawn = true;
        SpawnNextClient();
    }
}