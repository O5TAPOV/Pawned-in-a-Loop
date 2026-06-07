using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    public PawnshopItem CurrentSelectedItem;
    private PawnshopItem currentHoveredItem;

    void Update()
    {
        // 1. Пускаємо промінь
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // 2. ЛОГІКА НАВЕДЕННЯ (HOVER)
        if (Physics.Raycast(ray, out hit, 100f, interactableLayer))
        {
            PawnshopItem hoveredItem = hit.collider.GetComponent<PawnshopItem>();

            if (hoveredItem != null)
            {
                // 1. Логіка зміни Тултіпа (спрацьовує лише 1 раз при наведенні на НОВИЙ предмет)
                if (hoveredItem != currentHoveredItem)
                {
                    currentHoveredItem = hoveredItem;
                    EventManager.ItemHovered(currentHoveredItem);
                }

                // 2. Логіка КЛІКА (спрацьовує ЗАВЖДИ, коли ми тиснемо ЛКМ, дивлячись на предмет)
                // Зверни увагу: вона ТЕПЕР ЗА МЕЖАМИ перевірки "чи це новий предмет?"
                if (Input.GetMouseButtonDown(0))
                {
                    EventManager.ItemClicked(hoveredItem);
                }
            }
        }
        else
        {
            // Якщо промінь нікуди не влучив (дивимось у стіну або небо)
            // Перевіряємо, чи ми до цього на щось дивилися?
            if (currentHoveredItem != null)
            {
                currentHoveredItem = null; // Забуваємо предмет
                EventManager.ItemHoverExited(); // КРИЧИМО В РАДІО: "ХОВАЙ ТУЛТІП!"
            }
        }
    }
}