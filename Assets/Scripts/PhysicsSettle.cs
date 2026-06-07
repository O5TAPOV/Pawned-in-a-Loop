using UnityEngine;

public class PhysicsSettle : MonoBehaviour
{
    private Rigidbody rb;
    private Transform targetSlot;

    public void Initialize(Transform slot)
    {
        targetSlot = slot;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Не реагуємо на інші предмети
        if (collision.gameObject.CompareTag("Item")) return;

        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Прикріплюємо до слота
        transform.SetParent(targetSlot);

        // КЛЮЧОВИЙ МОМЕНТ: Викликаємо прилипання
        SnapToFloor();

        Destroy(this);
    }

    private void SnapToFloor()
    {
        // Стріляємо променем вниз з центру предмета
        // 2f - це довжина променя (має вистачити, щоб дістати до полиці)
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, Vector3.down, out RaycastHit hit, 2f))
        {
            // Беремо відстань від центру об'єкта до його дна (bounds.min)
            Collider col = GetComponentInChildren<Collider>();
            if (col != null)
            {
                float objectHalfHeight = transform.position.y - col.bounds.min.y;

                // Ставимо предмет рівно: Точка удару (поверхня полиці) + висота дна предмета
                transform.position = new Vector3(
                    transform.position.x,
                    hit.point.y + objectHalfHeight,
                    transform.position.z
                );
            }
        }
    }
}