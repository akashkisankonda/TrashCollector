using UnityEngine;
using System;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

[Serializable]
public class MainGameData
{
    public int trucks = 0;
    public int speed = 5;
    public int Money = 100;

    public List<Debree> debrees = new();
}

[Serializable]
public class Debree 
{
    public int id;
    public Vector3 sccale;
}


public class GameStore : MonoBehaviour
{
    public static GameStore Instance { get; private set; }

    public MainGameData mainGameData;

    public TextMeshProUGUI Money;

    public TextMeshProUGUI truckCost;
    public TextMeshProUGUI speedCost;

    public int moneyRequiredForTruck = 100;
    public int moneyRequiredForSpeed = 100;

    public int speedBumpOnUpgrade = 1;

    public int moneyIncrementOnDebree = 25;

    public Vector3 debreeDecrement = new Vector3(0.9f, 0.9f, 0.9f);

    public Transform allDebrees;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializeMainGameData();

        if(mainGameData.debrees.Count > 0)
        {
            foreach (var item in mainGameData.debrees)
            {
                Transform debree = allDebrees.GetChild(item.id);
                debree.localScale = item.sccale;
            }
        }
        else
        {
            for (int i = 0; i < allDebrees.transform.childCount; i++)
            {
                Debree debree = new();
                debree.id = i;
                debree.sccale = allDebrees.transform.GetChild(i).transform.localScale;

                mainGameData.debrees.Add(debree);
            }
        }

        SaveGameData();

        truckCost.text = moneyRequiredForTruck.ToString() + "$";
        speedCost.text = moneyRequiredForSpeed.ToString() + "$";
        UpdateMoney(0);
    }

    public void UpdateDebreeScaling()
    {
        foreach (var item in mainGameData.debrees)
        {
            Transform debree = allDebrees.GetChild(item.id);
            item.sccale = debree.localScale;
        }

        SaveGameData();
    }

    public void UpdateMoney(int value)
    {
        mainGameData.Money += value;
        Money.text = mainGameData.Money.ToString() + "$";
    }
    public void UpdateSpeed()
    {
        if (mainGameData.Money < moneyRequiredForSpeed) return;
        mainGameData.speed += speedBumpOnUpgrade;
        UpdateMoney(-moneyRequiredForSpeed);
    }
    public void UpdateTrucks()
    {
        if (mainGameData.Money < moneyRequiredForTruck) return;
        if (mainGameData.trucks > 5) return;
        mainGameData.trucks += 1;
        UpdateMoney(-moneyRequiredForTruck);
    }

    private void InitializeMainGameData()
    {
        if (PlayerPrefs.HasKey("Data"))
        {
            string jsonData = PlayerPrefs.GetString("Data");
            mainGameData = JsonUtility.FromJson<MainGameData>(jsonData);
            Debug.Log("Game data successfully loaded from PlayerPrefs.");
        }
        else
        {
            mainGameData = new();
            Debug.Log("New game data created and saved.");
        }
    }

    public void SaveGameData()
    {
        string jsonData = JsonUtility.ToJson(mainGameData);

        PlayerPrefs.SetString("Data", jsonData);

        PlayerPrefs.Save();
        Debug.Log("Game data saved to PlayerPrefs.");
    }

    public void ClearGameData()
    {
        PlayerPrefs.DeleteKey("Data");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
