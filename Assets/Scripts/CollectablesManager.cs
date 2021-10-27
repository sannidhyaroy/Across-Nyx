using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace MainScript
{
    public class CollectablesManager : MonoBehaviour
    {
        [HideInInspector] public int TotalCoinsAvailable;
        [HideInInspector] public int TotalGemsAvailable;
        public TextMeshProUGUI CoinsAvailable;
        public TextMeshProUGUI GemsAvailable;
        /*public void AddCoins()
        {
            int CoinsCollected = FindObjectOfType<CoinsController>().CoinCollected;
            Debug.Log("CoinsCollected: " + CoinsCollected);
            TotalCoinsAvailable = PlayerPrefs.GetInt("TotalCoinsAvailable", 0);
            TotalCoinsAvailable += CoinsCollected;
            Debug.Log("Total Coins: " + TotalCoinsAvailable);
            CoinsAvailable.text = TotalCoinsAvailable.ToString();
            PlayerPrefs.SetInt("TotalCoinsAvailable", TotalCoinsAvailable);
            Debug.Log("Coins Saved!");
        }*/

        public void RestoreCoins(int CoinsCollected)
        {
            //TotalCoinsAvailable = PlayerPrefs.GetInt("TotalCoinsAvailable", 0);
            //CoinsAvailable.text = TotalCoinsAvailable.ToString();
            CoinsAvailable.text = CoinsCollected.ToString();
            //Debug.Log("Coins restored! Available coins = " + TotalCoinsAvailable);
            Debug.Log("Coins restored! Available coins = " + CoinsCollected);
        }
        /*public void AddGems()
        {
            int GemsCollected = FindObjectOfType<GemsController>().GemCollected;
            Debug.Log("GemsCollected: " + GemsCollected);
            TotalGemsAvailable = PlayerPrefs.GetInt("TotalGemsAvailable", 0);
            TotalGemsAvailable += GemsCollected;
            Debug.Log("Total Gems: " + TotalGemsAvailable);
            GemsAvailable.text = TotalGemsAvailable.ToString();
            PlayerPrefs.SetInt("TotalGemsAvailable", TotalGemsAvailable);
            Debug.Log("Gems Saved!");
        }*/

        public void RestoreGems(int GemsCollected)
        {
            //TotalGemsAvailable = PlayerPrefs.GetInt("TotalGemsAvailable", 0);
            //GemsAvailable.text = TotalGemsAvailable.ToString();
            GemsAvailable.text = GemsCollected.ToString();
            //Debug.Log("Gems restored! Available gems = " + TotalGemsAvailable);
            Debug.Log("Gems restored! Available gems = " + GemsCollected);
        }
        public void StoreCollectables()
        {
            SaveGameData.SaveCollectablesData(this);
        }
        public void LoadCollectables()
        {
            CollectablesData data = SaveGameData.LoadCollectablesData();
            TotalCoinsAvailable = int.Parse(Encryption.EncryptDecrypt(data.TotalCoinsAvailable,270));
            TotalGemsAvailable = int.Parse(Encryption.EncryptDecrypt(data.TotalGemsAvailable, 270));
        }
        public void UpdateCollectables(int coin, int gem)
        {
            TotalCoinsAvailable = coin;
            TotalGemsAvailable = gem;
            StoreCollectables();
        }
    }
}
