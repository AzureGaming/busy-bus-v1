using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Coin", menuName = "ScriptableObjects/CoinScriptableObject")]
public class CoinScriptableObject : ScriptableObject {
    public float value;
    public Sprite sprite;
}
