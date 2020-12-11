using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ArenaUIManager : MonoBehaviour
{
    public static ArenaUIManager instance;
    public List<HexTile> hexTileList = new List<HexTile>();

    [Header("Pre-Game Attributes")]
    public Animator Transition;
    public bool isClickRoleCard = false;
    public Animator RoleCard;
    public Animator RolePrefab;
    public Sprite[] roleCardFaceList;

    [Header("User InterFace")]
    public Image yourTurnBanner;
    public Animator TurnBanner;
    [Space]
    public Image damageBanner;
    public Animator damageBannerAnim;
    [Space]
    public GameObject handPrefab;
    public TextMeshProUGUI targetText;
    public bool isFirstTurn = true;

    [Space]
    public GameObject turnOrderBar;
    public Button endTurnButton;
    public int lastedCardCount;


    public void Awake()
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
    }

    private void Start()
    {
        Transition.gameObject.SetActive(true);
        for (int i =0; i < GameManager.players.Count; i++)
        {
            GameObject _statusBar = turnOrderBar.transform.GetChild(GameManager.players.Count - (i + 1)).gameObject;
            PlayerManager _player = GameManager.players[i + 1];
            _statusBar.SetActive(true);
            _statusBar.GetComponent<PlayerStatusBar>().playerName.text = _player.username;
            _statusBar.GetComponent<PlayerStatusBar>().id = _player.id;
        }
    }

    public void Update()
    {
        if (lastedCardCount != handPrefab.transform.childCount)
        {
            SetCardPosition();
            lastedCardCount = handPrefab.transform.childCount;
        }
        
    }

    public void LoadComplete()
    {
        Debug.Log("Load Arena");

        RoleCard.SetBool("isReveal", true);
        
        //Transition.SetBool("isFadeIn", true);

        /*for (int i = 1; i <= GameManager.players.Count; i++)
        {
            Instantiate(GameManager.players[i].gameObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
            
        }*/
    }

    public void roleCardReveal()
    {
        if (!isClickRoleCard)
        {
            RoleCard.SetBool("isClick", true);
            ClientSend.SendRoleIDRequest();
            Invoke("ChangeRoleCardFromBackToFace", 0.25f);
        }
        isClickRoleCard = true;

    }

    public void GameStart()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(5f);
        Transition.SetBool("isFadeIn", true);
        RolePrefab.SetBool("isSwift", true);
        StartCoroutine(destroyRolePrefab());
    }

    IEnumerator destroyRolePrefab()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(RolePrefab);
    }

    public void ChangeRoleCardFromBackToFace()
    {
        RoleCard.gameObject.GetComponent<Image>().sprite = roleCardFaceList[GameManager.myRoleID];
    }

    public void SetTurnBanner(bool _isMyTurn)
    {
        yourTurnBanner.gameObject.SetActive(_isMyTurn);
        TurnBanner.SetBool("isFade", true);
        StartCoroutine(FadeTurnBanner());
    }

    IEnumerator FadeTurnBanner()
    {
        yield return new WaitForSeconds(2.8f);
        TurnBanner.SetBool("isFade", false);
        yourTurnBanner.gameObject.SetActive(false);
    }

    public void SetDamageBanner(List<int> _allCard)
    {
        damageBanner.gameObject.SetActive(true);
        string text = "";

        if (_allCard.Count == 0)
        {
            text = "You are not under target by any player.";
        }
        else
        {
            text = "You have been target by ";
            if (CountInList(_allCard, 0) != 0) text += $"{CountInList(_allCard, 0)} Attack ";
            if (CountInList(_allCard, 1) != 0) text += $"{CountInList(_allCard, 1)} Defense ";
            if (CountInList(_allCard, 2) != 0) text += $"{CountInList(_allCard, 2)} Heal ";
        }
        damageBanner.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = text;
        damageBannerAnim.SetBool("isFade", true);
        StartCoroutine(FadeDamageBanner());
    }

    public int CountInList(List<int> _list, int _num)
    {
        int _count = 0;
        for (int i = 0; i < _list.Count; i++)
        {
            if (_list[i] == _num) _count++;
        }
        return _count;
    }
    IEnumerator FadeDamageBanner()
    {
        yield return new WaitForSeconds(2.8f);
        damageBannerAnim.SetBool("isFade", false);
        damageBanner.gameObject.SetActive(false);
    }

    public void SetAvailableMove(int _range)
    {
        int _x = GameManager.players[Client.instance.myId].position[0];
        int _y = GameManager.players[Client.instance.myId].position[1];
        for (int y = -_range; y <= 1; y++)
        {
            int startx;
            int endx;
            if (y > 0)
            {
                startx = -1;
                endx = 1 - y;
            }
            else
            {
                startx = -1 - y;
                endx = 1;
            }


            for (int x = startx; x <= endx; x++)
            {
                foreach (HexTile tile in hexTileList)
                {
                    if (tile.x == x + _x && tile.y == y + _y)
                    {
                        tile.hexPrefab.setMoveableColor();
                        tile.isTurn = true;
                        break;
                    }
                }
            }

        }
    }

    public void SetTileEndTurn()
    {
        foreach (HexTile tile in hexTileList)
        {
            tile.hexPrefab.setStartColor();
            tile.isTurn = false;
        }
    }

    public void EndTurn()
    {
        ArenaUIManager.instance.endTurnButton.gameObject.SetActive(false);
        GameManager.isCurrentTurn = false;
        GameManager.isMove = false;
        ClientSend.EndTurn();
    }

    
    public void SetCardPosition()
    {
        int cardCount = handPrefab.transform.childCount;
        float _cardInHandAllWidth = (cardCount * 200f) - ((cardCount - 1) * 50f);
        float _xPerCard = _cardInHandAllWidth / cardCount;
        for (int i = 0; i < cardCount; i++)
        {
            float _targetX = (i * _xPerCard) - (_cardInHandAllWidth / 2);
            handPrefab.transform.GetChild(i).GetComponent<Card>().targetPosition = new Vector3(_targetX, -450f, 0f);
        }
    }

    public void GetPlayerInRange(int _range, bool _isCastSelf)
    {
        int _x = GameManager.players[Client.instance.myId].position[0];
        int _y = GameManager.players[Client.instance.myId].position[1];
        for (int y = -1; y <= 1; y++)
        {
            int startx;
            int endx;
            if (y > 0)
            {
                startx = -1;
                endx = 1 - y;
            }
            else
            {
                startx = -1 - y;
                endx = 1;
            }


            for (int x = startx; x <= endx; x++)
            {
                for (int i = 1; i <= GameManager.players.Count; i++)
                {
                    PlayerManager _player = GameManager.players[i];
                    if (_player.position[0] == x + _x && _player.position[1] == y + _y && (i != Client.instance.myId || _isCastSelf))
                    {
                        _player.isTargetable = true;
                        _player.playerPrefab.setTargetable();
                        break;
                    }
                }
            }

        }
    }

    public void SelfCastRange()
    {
        PlayerManager _player = GameManager.players[Client.instance.myId];
        _player.isTargetable = true;
        _player.playerPrefab.setTargetable();
    }

    public void DamageCalculate(int _health)
    {
        GameManager.players[Client.instance.myId].GetComponent<PlayerManager>().playerHealth = _health;
    }


    public void ChangeToResult()
    {
        StartCoroutine(TransitionChange());
    }

    public IEnumerator TransitionChange()
    {
        Transition.gameObject.SetActive(true);
        Transition.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(LoadAsynchronously(2));
    }

    IEnumerator LoadAsynchronously(int _index)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(_index);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log(progress);

            ClientSend.LoadArenaComplete();

            yield return null;
        }
    }

}
