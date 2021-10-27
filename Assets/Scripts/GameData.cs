using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    [System.Serializable]
    public class GameData
    {
        public string EmailID;
        public string Password;
        public GameData(PlayFabManager data)
        {
            EmailID = Encryption.EncryptDecrypt(data.emailid.text, 500);
            Password = Encryption.EncryptDecrypt(data.password.text, 2000);
        }
    }
}

namespace MainScript
{
    [System.Serializable]
    public class CollectablesData
    {
        public string TotalCoinsAvailable;
        public string TotalGemsAvailable;
        public CollectablesData(CollectablesManager data)
        {
            TotalCoinsAvailable = Encryption.EncryptDecrypt((data.TotalCoinsAvailable.ToString()), 270);
            TotalGemsAvailable = Encryption.EncryptDecrypt((data.TotalGemsAvailable.ToString()), 270);
        }
    }
}
