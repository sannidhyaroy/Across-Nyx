using System;
using UnityEngine;
using TMPro;

namespace MainScript
{
    public class InvitesUIList : MonoBehaviour
    {
        [SerializeField] private string _friendName;
        [SerializeField] private string _roomName;
        [SerializeField] private TMP_Text _friendNameText;

        public static Action<string> OnRoomInviteAccept = delegate { };
        public static Action<InvitesUIList> OnInviteAccept = delegate { };
        public static Action<InvitesUIList> OnInviteDecline = delegate { };

        public void Initialize(string friendName, string roomName)
        {
            _friendName = friendName;
            _roomName = roomName;

            _friendNameText.SetText(_friendName);
        }

        public void AccecptInvite()
        {
            OnInviteAccept?.Invoke(this);
            OnRoomInviteAccept?.Invoke(_roomName);
        }

        public void DeclineInvite()
        {
            OnInviteDecline?.Invoke(this);
        }
    }
}