using UnityEngine;

public class StorageManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private Transform shelfContainer;
    [SerializeField] private Transform[] shelfStartPoints;

    [Header("Scroll Settings")]
    [Tooltip("Має дорівнювати Item Spacing! ТІЛЬКИ ПЛЮСОВЕ ЧИСЛО! (напр. 0.5)")]
    [SerializeField] private float scrollStep = 0.5f;
    [Tooltip("Швидкість згладжування (спробуй 7 або 5)")]
    [SerializeField] private float smoothTime = 7f;
    [SerializeField] private float minX = 0f;
    [SerializeField] private float maxX = 0f;

    [Header("Shelf Grid")]
    [Tooltip("Відстань між предметами. ТІЛЬКИ ПЛЮСОВЕ ЧИСЛО! (напр. 0.5)")]
    [SerializeField] private float itemSpacing = 0.5f;

    [Tooltip("УВІМКНИ, якщо полиця має заповнюватись ВЛІВО (в мінус по X)")]
    [SerializeField] private bool growToLeft = false;

    private int itemsCount = 0;
    private float targetX = 0f;

    private void OnEnable()
    {
        EventManager.OnItemBought += PutItemOnShelf;
        EventManager.OnNewDayStarted += ClearShelf;
    }

    private void OnDisable()
    {
        EventManager.OnItemBought -= PutItemOnShelf;
        EventManager.OnNewDayStarted -= ClearShelf;
    }

    private void Update()
    {
        if (playerCamera.IsFacingShelf)
        {
            HandleScrolling();
        }
    }

    private void HandleScrolling()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0)
        {
            float normalizedScroll = Mathf.Clamp(scroll, -1f, 1f);
            int maxColumns = Mathf.Max(0, (itemsCount - 1) / shelfStartPoints.Length);

            // МАТЕМАТИКА ЗАЛЕЖНО ВІД НАПРЯМКУ РОСТУ ПОЛИЦІ
            if (growToLeft)
            {
                // Якщо полиця росте вліво, контейнер має їхати ВПРАВО (+), щоб показати нові речі
                targetX += normalizedScroll * scrollStep;
                float dynamicMaxX = maxX + (maxColumns * itemSpacing);
                targetX = Mathf.Clamp(targetX, minX, dynamicMaxX);
            }
            else
            {
                // Стандартно: полиця росте вправо, контейнер їде ВЛІВО (-)
                targetX -= normalizedScroll * scrollStep;
                float dynamicMinX = minX - (maxColumns * itemSpacing);
                targetX = Mathf.Clamp(targetX, dynamicMinX, maxX);
            }
        }

        Vector3 currentPos = shelfContainer.localPosition;
        currentPos.x = Mathf.Lerp(currentPos.x, targetX, Time.deltaTime * smoothTime);
        shelfContainer.localPosition = currentPos;
    }

    private void PutItemOnShelf(PawnshopItem item)
    {
        Transform newSlot = CreateFreeSlot();
        AlignItemToSlot(item.gameObject, newSlot);
    }

    private Transform CreateFreeSlot()
    {
        int currentShelfIndex = itemsCount % shelfStartPoints.Length;
        int currentColumn = itemsCount / shelfStartPoints.Length;

        Transform targetShelfStart = shelfStartPoints[currentShelfIndex];
        GameObject newSlot = new GameObject($"DynamicSlot_{itemsCount}");
        newSlot.transform.SetParent(targetShelfStart);

        // Враховуємо напрямок росту (якщо growToLeft, то множимо на -1)
        float direction = growToLeft ? -1f : 1f;
        newSlot.transform.localPosition = new Vector3(currentColumn * itemSpacing * direction, 0, 0);
        newSlot.transform.localRotation = Quaternion.identity;

        itemsCount++;
        return newSlot.transform;
    }

    private void AlignItemToSlot(GameObject itemObj, Transform slot)
    {
        if (itemObj.TryGetComponent(out Rigidbody rb))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        if (itemObj.TryGetComponent(out BoxCollider box))
        {
            float localBottomY = box.center.y - (box.size.y / 2f);
            itemObj.transform.position = slot.position - new Vector3(0, localBottomY, 0);
        }
        else
        {
            itemObj.transform.position = slot.position;
        }

        itemObj.transform.rotation = Quaternion.identity;
        itemObj.transform.SetParent(slot);
    }

    public void ClearShelf()
    {
        foreach (Transform startPoint in shelfStartPoints)
        {
            foreach (Transform slot in startPoint)
            {
                Destroy(slot.gameObject);
            }
        }

        itemsCount = 0;
        targetX = 0f;
        shelfContainer.localPosition = Vector3.zero;
    }
}