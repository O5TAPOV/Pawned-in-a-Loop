using UnityEngine;
using TMPro;

public class TooltipUI : MonoBehaviour
{
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;

    private void Start()
    {
        tooltipPanel.SetActive(false);
    }

    private void OnEnable()
    {
        EventManager.OnItemHovered += ShowTooltip;
        EventManager.OnItemHoverExited += HideTooltip;
    }

    private void OnDisable()
    {
        EventManager.OnItemHovered -= ShowTooltip;
        EventManager.OnItemHoverExited -= HideTooltip;
    }

    private void ShowTooltip(PawnshopItem item)
    {
        tooltipPanel.SetActive(true);
        nameText.text = item.Data.ItemName;
        priceText.text = (item.IsOnShelf ? item.SellPrice : item.PurchasePrice).ToString() + "$";
    }

    private void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}