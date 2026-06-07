using UnityEngine;

public class PawnshopItem : MonoBehaviour
{
    [SerializeField] private ItemData data;
    public ItemData Data => data;
    public int PurchasePrice { get; private set; }
    public int SellPrice { get; private set; }
    public bool IsOnShelf { get; set; } = false;

    [Header("Налаштування авто-розміру")]
    [Tooltip("Перетягни сюди дочірній об'єкт Visuals")]
    [SerializeField] private Transform visualsContainer;
    [Tooltip("Максимальний розмір предмета в метрах (напр. 0.5 - це півметра)")]
    [SerializeField] private float targetMaxSize = 0.5f;

    // Цей атрибут робить магічну кнопку в Інспекторі (спрацьовує навіть не в режимі гри!)
    [ContextMenu("✨ МАГІЯ: Автоматично підігнати розмір і колайдер")]
    public void AutoFitItem()
    {
        if (visualsContainer == null)
        {
            Debug.LogError("Ей, ти забув закинути об'єкт Visuals у поле visualsContainer!");
            return;
        }

        // 1. Скидаємо скейл візуалу в 1, щоб чесно порахувати оригінальні розміри
        visualsContainer.localScale = Vector3.one;

        // 2. Шукаємо всі "сітки" (Renderer) всередині Visuals
        Renderer[] renderers = visualsContainer.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0)
        {
            Debug.LogWarning("У Visuals немає жодного Renderer (модельки)!");
            return;
        }

        // 3. Рахуємо загальні межі (Bounds) всієї моделі
        Bounds bounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            bounds.Encapsulate(r.bounds); // Об'єднуємо всі шматочки в одну коробку
        }

        // 4. Знаходимо найдовшу сторону моделі (X, Y або Z)
        float maxDimension = Mathf.Max(bounds.size.x, bounds.size.y, bounds.size.z);

        // 5. Вираховуємо ідеальний масштаб і застосовуємо до Visuals
        float scaleFactor = targetMaxSize / maxDimension;
        visualsContainer.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);

        // 6. ПІДГАНЯЄМО КОЛАЙДЕР НА КОРЕНІ
        BoxCollider col = GetComponent<BoxCollider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();

        // Перераховуємо межі ПІСЛЯ зміни масштабу
        Bounds newBounds = renderers[0].bounds;
        foreach (Renderer r in renderers)
        {
            newBounds.Encapsulate(r.bounds);
        }

        // Оскільки Scale на корені у нас 1, ми просто передаємо світові розміри в колайдер
        col.center = transform.InverseTransformPoint(newBounds.center);
        col.size = newBounds.size;

#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.EditorUtility.SetDirty(col);
        UnityEditor.PrefabUtility.RecordPrefabInstancePropertyModifications(this);
#endif

        Debug.Log($"[AutoFit] {gameObject.name} успішно підігнано! Новий масштаб Visuals: {scaleFactor}");
    }

    private void Start()
    {
        if (data == null) return;

        float sellMultiplier = data.IsCursed ? Random.Range(2f, 3f) : Random.Range(0.5f, 1.5f);
        float purchaseMultipler = Random.Range(0.9f, 1.25f);
        PurchasePrice = Mathf.RoundToInt(data.Price * purchaseMultipler);
        SellPrice = Mathf.RoundToInt(PurchasePrice * sellMultiplier);
    }
}