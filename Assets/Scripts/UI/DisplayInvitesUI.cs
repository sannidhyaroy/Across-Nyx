using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScript
{
    public class DisplayInvitesUI : MonoBehaviour
    {
        [SerializeField] Transform inviteContainer;
        [SerializeField] InvitesUIList invitesUIPrefab;
        [SerializeField] RectTransform contentRect;
        [SerializeField] Vector2 originalSize;
        [SerializeField] Vector2 increaseSize;
        private List<InvitesUIList> invites;
        private void Awake()
        {
            invites = new();
            contentRect = inviteContainer.GetComponent<RectTransform>();
            originalSize = contentRect.sizeDelta;
            increaseSize = new Vector2(0, invitesUIPrefab.GetComponent<RectTransform>().sizeDelta.y);
            PhotonChatController.OnRoomInvite += HandleRoomInvite;
            InvitesUIList.OnInviteAccept += HandleInviteAccept;
            InvitesUIList.OnInviteDecline += HandleInviteDecline;
        }
        private void OnDestroy()
        {
            PhotonChatController.OnRoomInvite -= HandleRoomInvite;
            InvitesUIList.OnInviteAccept -= HandleInviteAccept;
            InvitesUIList.OnInviteDecline -= HandleInviteDecline;
        }

        private void HandleInviteAccept(InvitesUIList invite)
        {
            if (invites.Contains(invite))
            {
                invites.Remove(invite);
                Destroy(invite.gameObject);
            }
        }

        private void HandleInviteDecline(InvitesUIList invite)
        {
            if (invites.Contains(invite))
            {
                invites.Remove(invite);
                Destroy(invite.gameObject);
            }
        }

        private void HandleRoomInvite(string friend, string room)
        {
            Debug.Log("Room invite from " + friend + " to join Room " + room);
            InvitesUIList invitesUI = Instantiate(invitesUIPrefab, inviteContainer);
            invitesUI.Initialize(friend, room);
            contentRect.sizeDelta += increaseSize;
            invites.Add(invitesUI);
        }
    }
}