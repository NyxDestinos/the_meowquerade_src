using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server : {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        UIManager.instance.SetHost(_myId);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        int _x = _packet.ReadInt();
        int _y = _packet.ReadInt();

        GameManager.instance.SpawnPlayer(_id, _username, _x, _y);
    }

    public static void SpawnPlayerInLobby(Packet _packet)
    {
        int _playerLength = _packet.ReadInt();
        for (int i = 0; i < _playerLength; i++)
        {
            int _id = _packet.ReadInt();
            string _username = _packet.ReadString();
            Debug.Log(_id + " : " + _username);
            GameManager.instance.SpawnPlayerInLobby(_id, _username);
        }
    }

    public static void SendDisconnectConfirm(Packet _packet)
    {
        int _disconnectPlayer = _packet.ReadInt();

        if (_disconnectPlayer == Client.instance.myId)
        {
            Debug.Log("Disconnect Confirm!");
            Client.instance.Disconnect();
            GameManager.instance.Disconnect();
            UIManager.instance.DisconnectFromServer();
        }
        else
        {
            Debug.Log("Get Disconnect Sign from player : " + _disconnectPlayer);
            Debug.Log("You are : " + Client.instance.myId);
            GameManager.instance.RemovePlayer(_disconnectPlayer);
            GameManager.instance.ClearPlayerInLobby(_disconnectPlayer);
        }
    }

    public static void SendReadyChange(Packet _packet)
    {
        int _ReadyChangeID = _packet.ReadInt();
        bool _changeTo = _packet.ReadBool();

        GameManager.players[_ReadyChangeID].isReady = _changeTo;
        GameManager.instance.setReadyState(_ReadyChangeID, _changeTo);
    }

    public static void SetHostCanStart(Packet _packet)
    {
        bool _setState = _packet.ReadBool();

        UIManager.instance.SetStartButtonInteract(_setState);
    }

    public static void SendPlayerToGame(Packet _packet)
    {
        UIManager.instance.ChangeToArena();
    }

    public static void StartGame(Packet _packet)
    {
        ArenaUIManager.instance.LoadComplete();
    }

    public static void SendTurnToCurrentPlayer(Packet _packet)
    {
        bool _isMyTurn = _packet.ReadBool();

        GameManager.isCurrentTurn = true;
        ArenaUIManager.instance.SetTurnBanner(_isMyTurn);
        ArenaUIManager.instance.endTurnButton.gameObject.SetActive(true);
        ArenaUIManager.instance.SetAvailableMove(1);
    }

    public static void SendRoleToPlayer(Packet _packet)
    {
        int _roleID = _packet.ReadInt();
        GameManager.myRoleID = _roleID;
    }

    public static void EveryoneKnowTheirRole(Packet _packet)
    {
        bool _canStart = _packet.ReadBool();

        ArenaUIManager.instance.GameStart();
    }

    public static void SendPlayerPositionToAll(Packet _packet)
    {
        int _playerChangeID = _packet.ReadInt();
        int _playerX = _packet.ReadInt();
        int _playerY = _packet.ReadInt();

        foreach (HexTile tile in ArenaUIManager.instance.hexTileList)
        {
            if (tile.x == _playerX && tile.y == _playerY)
            {
                GameManager.players[_playerChangeID].getNextPoint(_playerX, _playerY, tile.transform.position);
                break;
            }
        }
        
    }

    public static void SendPlayerCardInHand(Packet _packet)
    {
        int _cardInHandCount = _packet.ReadInt();

        for (int i = 0; i < _cardInHandCount; i++)
        {
            int _cardId = _packet.ReadInt();
            if (i >= GameManager.cardInHand.Count)
            {
                Card _card = Instantiate(GameManager.instance.cardPrefab, new Vector3(0f, 0f, 0f), Quaternion.identity);
                Debug.Log("Card Drawn : " + _cardId);
                _card.SetCard(_cardId);
                ArenaUIManager.instance.SetCardPosition();
                GameManager.cardInHand.Add(_card);
            }

        }
    }

    public static void SendTurnCalculation(Packet _packet)
    {
        int _cardCount = _packet.ReadInt();
        int _playerHealth = _packet.ReadInt();
        List<int> _allCard = new List<int>();

        for (int i = 1; i <= GameManager.players.Count; i++)
        {
            int _remainHealth = _packet.ReadInt();
            if (_remainHealth <= 0)
            {
                Destroy(GameManager.players[i].gameObject);
            }
            for (int j = 0; j < GameManager.players.Count; j++)
            {
                PlayerStatusBar _temp = ArenaUIManager.instance.turnOrderBar.transform.GetChild(j).gameObject.GetComponent<PlayerStatusBar>();
                if (_temp.id == i) _temp.heartDisplay(_remainHealth);
            }
        }

        for (int i = 0; i < _cardCount; i++)
        {
            int _cardID = _packet.ReadInt();
            _allCard.Add(_cardID);
        }
        ArenaUIManager.instance.SetDamageBanner(_allCard);
    }

    public static void SendWin(Packet _packet)
    {
        bool _isGuestWin = _packet.ReadBool();

        GameManager.isGuestWin = _isGuestWin;
        ArenaUIManager.instance.ChangeToResult();
    }
}
