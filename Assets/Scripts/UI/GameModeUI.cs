using System;
using UnityEngine;

namespace MainScript
{
    public class GameModeUI : MonoBehaviour
    {
        [SerializeField] private GameMode _gameMode;
        public static Action<GameMode> OnGameModeSelected = delegate { };

        public void SelectGameMode()
        {
            if (_gameMode == null) return;

            OnGameModeSelected?.Invoke(_gameMode);
        }
    }
}