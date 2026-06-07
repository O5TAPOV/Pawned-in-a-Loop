using System.Collections;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] private float baseSpeed = 5f;
    private bool isFacingShelf = false;
    private bool isRotating = false;

    // ўоб знати, чи можна гортати стелаж (знадобитьс€ дл€ StorageManager)
    public bool IsFacingShelf => isFacingShelf;

    void Update()
    {
        if (Input.GetMouseButtonDown(1) && !isRotating)
        {
            isFacingShelf = !isFacingShelf;
            EventManager.CameraTurned(isFacingShelf);

            // якщо обертаЇмось до стелажу, кидаЇмо монетку: 180 (вправо) або -180 (вл≥во)
            float targetAngleY = isFacingShelf ? (Random.value > 0.5f ? 180f : -180f) : 0f;
            float targetAngleX = isFacingShelf ? 13f : 23.5f;

            StartCoroutine(RotateToAngle(targetAngleY, targetAngleX));
        }
    }

    private IEnumerator RotateToAngle(float targetAngleY, float targetAngleX)
    {
        isRotating = true;

        // Ѕеремо поточний кут. якщо в≥н б≥льший за 180, перетворюЇмо у в≥д'Їмний дл€ правильноњ математики
        float startAngle = transform.eulerAngles.y;
        if (startAngle > 180) startAngle -= 360f;

        float randomSpeed = baseSpeed * Random.Range(0.8f, 1.2f);
        float progress = 0f;

        float startAngleX = transform.eulerAngles.x;

        while (progress < 1f)
        {
            progress += Time.deltaTime * randomSpeed;
            // ѕлавний перех≥д м≥ж двома числами
            float currentAngle = Mathf.Lerp(startAngle, targetAngleY, progress);
            float currentAngleX = Mathf.Lerp(startAngleX, targetAngleX, progress);
            transform.rotation = Quaternion.Euler(currentAngleX, currentAngle, 0);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(targetAngleX, targetAngleY, 0);
        isRotating = false;
    }
}