using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CoinUI : MonoBehaviour
{
    public int startCoinQuantity;
    public TextMeshProUGUI coinQuantityText;
    public static int current_coin_quantity;
    // Start is called before the first frame update
    void Start()
    {   
        current_coin_quantity = startCoinQuantity;
        
    }

    // Update is called once per frame
    void Update()
    {
        coinQuantityText.text = current_coin_quantity.ToString();
    }
}
