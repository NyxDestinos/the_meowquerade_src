using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();

    [Header("Player Prefab")]
    public GameObject localPlayerPrefab;
    public GameObject PlayerPrefab;
    public GameObject[] spriteSet;
    public GameObject mainPlayerLobbySlots;

    [Header("Card")]
    public int currentCardHand;
    public int currentCardIndex;
    public Card cardPrefab;
    public Sprite[] cardFaceList;
    public static List<Card> cardInHand = new List<Card>();

    [Header("Client Player Status Setting")]
    public static bool isCurrentTurn = false;
    public static bool isMove = false;
    public static int myRoleID;
    public static bool isGuestWin;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);

        }
        DontDestroyOnLoad(this);
    }
    
    public void SpawnPlayer(int _id, string _username, int _playerX, int playerY)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, new Vector3(0, 0, -2.5f), new Quaternion());
        }
        else
        {
            _player = Instantiate(PlayerPrefab, new Vector3(0, 0, -2.5f), new Quaternion());
        }

        _player.name = "Player " + _id;

        _player.GetComponent<PlayerManager>().id = _id;
        _player.GetComponent<PlayerManager>().username = _username;


        GameObject _playerPrefab = Instantiate(spriteSet[1], new Vector3(0, 0, 0), new Quaternion());
        _playerPrefab.transform.SetParent(_player.transform);
        _player.GetComponent<PlayerManager>().playerPrefab = _playerPrefab.GetComponent<PlayerSprite>();
        players.Add(_id, _player.GetComponent<PlayerManager>());


    }

    #region Lobby
    public void Disconnect()
    {
        for (int i = 1; i <= GameManager.players.Count; i++)
        {
            if (GameManager.players[i].gameObject != null) Destroy(GameManager.players[i].gameObject);
        }
        players = new Dictionary<int, PlayerManager>();
    }

    public void RemovePlayer(int _id)
    {
        players.Remove(_id);
    }

    public void SpawnPlayerInLobby(int _id, string _username)
    {
        mainPlayerLobbySlots.transform.GetChild(_id - 1).GetComponent<LobbyPlayerSlot>().setSlotData(_id, _username);
    }

    public void ClearPlayerInLobby(int _id)
    {
        mainPlayerLobbySlots.transform.GetChild(_id - 1).GetComponent<LobbyPlayerSlot>().clearSlot();
    }

    public void UpdateLobby()
    {
        for (int i = 0; i < players.Count; i++)
        {
            int _id = players.ElementAt(i).Key;

            SpawnPlayerInLobby(players[_id].id , players[_id].username );
            Debug.Log(players[_id]);
        }
        
    }

    public void setReadyState(int _id, bool _isActive)
    {
        mainPlayerLobbySlots.transform.GetChild(_id - 1).GetComponent<LobbyPlayerSlot>().setIsReadyText(_isActive);
    }
    #endregion

    public void UseCard(int _toPlayer)
    {
        ClientSend.SendCardEffectToTarget(_toPlayer, currentCardIndex);
        for (int i = 1; i <= GameManager.players.Count; i++)
        {
            PlayerManager _player = GameManager.players[i].GetComponent<PlayerManager>();
            _player.isTargetable = false;
        }
        //Destroy(GameManager.cardInHand[currentCardHand].gameObject);
        Destroy(ArenaUIManager.instance.handPrefab.transform.GetChild(currentCardHand).gameObject);
        GameManager.cardInHand.RemoveAt(currentCardHand);
        ArenaUIManager.instance.SetCardPosition();
        
    }

    public void UseCard()
    {
        ClientSend.SendCardEffectToTarget(Client.instance.myId, currentCardIndex);
        for (int i = 1; i <= GameManager.players.Count; i++)
        {
            PlayerManager _player = GameManager.players[i].GetComponent<PlayerManager>();
            _player.isTargetable = false;
        }
        //Destroy(GameManager.cardInHand[currentCardHand].gameObject);
        Destroy(ArenaUIManager.instance.handPrefab.transform.GetChild(currentCardHand).gameObject);
        GameManager.cardInHand.RemoveAt(currentCardHand);
        ArenaUIManager.instance.SetCardPosition();

    }

}
