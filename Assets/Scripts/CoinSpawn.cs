using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawn : MonoBehaviour {
    public delegate void GetCoinsAmount(int toonies, int loonies, int quarters, int dimes, int nickels);
    public static GetCoinsAmount OnGetCoinsAmount;
    public delegate void ClearSpawn();
    public static ClearSpawn OnClearSpawn;

    public GameObject coinPrefab;
    public CoinScriptableObject toonieData;
    public CoinScriptableObject loonieData;
    public CoinScriptableObject quarterData;
    public CoinScriptableObject dimeData;
    public CoinScriptableObject nickelData;

    private void OnEnable() {
        OnGetCoinsAmount += SpawnCoins;
        OnClearSpawn += DeleteCoins;
    }

    private void OnDisable() {
        OnGetCoinsAmount -= SpawnCoins;
        OnClearSpawn -= DeleteCoins;
    }

    void SpawnCoins(int toonies, int loonies, int quarters, int dimes, int nickels) {
        DeleteCoins();
        SpawnToonies(toonies);
        SpawnLoonies(loonies);
        SpawnQuarters(quarters);
        SpawnDimes(dimes);
        SpawnNickels(nickels);
    }

    void DeleteCoins() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }
    }

    void SpawnToonies(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject toonie = Instantiate(coinPrefab, transform);
            toonie.GetComponent<Coin>().LoadData(toonieData);
        }
    }

    void SpawnLoonies(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject loonie = Instantiate(coinPrefab, transform);
            loonie.GetComponent<Coin>().LoadData(loonieData);
        }
    }

    void SpawnQuarters(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject quarter = Instantiate(coinPrefab, transform);
            quarter.GetComponent<Coin>().LoadData(quarterData);
        }
    }

    void SpawnDimes(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject dime = Instantiate(coinPrefab, transform);
            dime.GetComponent<Coin>().LoadData(dimeData);
        }
    }

    void SpawnNickels(int amount) {
        for (int i = 0; i < amount; i++) {
            GameObject nickel = Instantiate(coinPrefab, transform);
            nickel.GetComponent<Coin>().LoadData(nickelData);
        }
    }
}
