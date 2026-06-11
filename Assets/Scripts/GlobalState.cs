using UnityEngine;

public static class GlobalState
{
    public static int CurrentDay = 1;
    public static int MoneySnapshot = 200;

    public static void SaveGame()
    {
        PlayerPrefs.SetInt("SavedDay", CurrentDay);
        PlayerPrefs.SetInt("SavedMoney", MoneySnapshot);
        PlayerPrefs.Save();
        Debug.Log($"[SaveSystem] Збережено: День {CurrentDay}, Гроші {MoneySnapshot}$");
    }

    public static void LoadGame()
    {
        CurrentDay = PlayerPrefs.GetInt("SavedDay", 1);
        MoneySnapshot = PlayerPrefs.GetInt("SavedMoney", 200);
        Debug.Log($"[SaveSystem] Завантажено: День {CurrentDay}, Гроші {MoneySnapshot}$");
    }

    public static bool HasSave() => PlayerPrefs.HasKey("SavedDay");

    public static void ResetGame()
    {
        CurrentDay = 1;
        MoneySnapshot = 200;
        PlayerPrefs.DeleteKey("SavedDay");
        PlayerPrefs.DeleteKey("SavedMoney");
        Debug.Log("[SaveSystem] Прогрес скинуто! Починаємо з чистого аркуша.");
    }
}
