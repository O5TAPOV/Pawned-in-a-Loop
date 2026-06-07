using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] itemPrefabs;
    [SerializeField] private Transform spawnPoint;

    // Update is called once per frame
    void Update()
    {
        if (itemPrefabs.Length == 0) return;

        if (Input.GetKeyDown(KeyCode.Space)) {
            int randomItem = UnityEngine.Random.Range(0, itemPrefabs.Length);

            Instantiate(itemPrefabs[randomItem], spawnPoint.position, spawnPoint.rotation);
        }
    }
}
