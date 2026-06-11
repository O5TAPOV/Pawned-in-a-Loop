using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<ItemData> boughtItemsToday = new List<ItemData>();
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;

    [SerializeField] private float dayDuration = 20f;
    private float currentTime;
    private bool isDayActive = false;
    private bool isClosingPhase = false;

    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject closingTextObject;

    [SerializeField] private TextMeshProUGUI dayText;

    private void OnEnable()
    {
        EventManager.OnItemBought += HandleItemBought;
        EventManager.OnClientLeft += LastClientLeft;
        EventManager.OnItemRemoved += HandleItemRemoved;
    }

    private void OnDisable()
    {
        EventManager.OnItemBought -= HandleItemBought;
        EventManager.OnClientLeft -= LastClientLeft;
        EventManager.OnItemRemoved -= HandleItemRemoved;
    }

    private void HandleItemBought(PawnshopItem boughtItem)
    {
        boughtItemsToday.Add(boughtItem.Data);
    }

    private void HandleItemRemoved(ItemData removedItem)
    {
        // Видаляємо предмет зі списку куплених за сьогодні!
        boughtItemsToday.Remove(removedItem);
    }
    public void EndDay()
    {
        for (int i = 0; i < boughtItemsToday.Count; i++)
        {
            if (boughtItemsToday[i].IsCursed)
            {
                Debug.Log("Ти купив прокляту річ! Ломбард згорів, ти вмер.");
                losePanel.SetActive(true);
                return;
            }
        }

        GlobalState.CurrentDay++;
        PlayerWallet wallet = FindFirstObjectByType<PlayerWallet>();
        if (wallet != null)
        {
            GlobalState.MoneySnapshot = wallet.GetCurrentMoney();
        }

        winPanel.SetActive(true);
        GlobalState.SaveGame();
    }

    public void StartNextDay()
    {
        winPanel.SetActive(false);
        losePanel.SetActive(false);
        closingTextObject.SetActive(false);
        


        currentTime = dayDuration;
        isDayActive = true;
        isClosingPhase = false;
        boughtItemsToday.Clear();
        dayText.text = $"DAY {GlobalState.CurrentDay}";
        EventManager.NewDayStarted();
    }

    public void LastClientLeft()
    {
        if (isClosingPhase)
        {
            EndDay();
            isDayActive = false;
        }
    }

    public void RestartTimeline()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    private void Start()
    {
        StartNextDay();
    }

    private void Update()
    {
        if (!isDayActive) return;

        currentTime = Mathf.Max(0, currentTime - Time.deltaTime);

        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

        if (!isClosingPhase && currentTime <= 0)
        {
            currentTime = 0;
            isClosingPhase = true;
            closingTextObject.SetActive(true);
            EventManager.ShopClosing();
        }
    }
}