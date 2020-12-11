using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject startMenu;
    public GameObject SettingMenu;
    public GameObject guideMenu;
    public TMP_InputField usernameField;
    public GameObject mainPlayerSlots;

    public GameObject readyButton;
    public GameObject startButton;

    public Animator Transition;


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
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        mainPlayerSlots.gameObject.SetActive(true);
        Client.instance.ConnectToServer();
    }

    public void OpenSettingMenu()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        SettingMenu.gameObject.SetActive(true);
    }

    public void CloseSettingMenu()
    {
        SettingMenu.gameObject.SetActive(false);
        startMenu.SetActive(true);
        usernameField.interactable = true;
    }

    public void OpenGuide()
    {
        startMenu.SetActive(false);
        usernameField.interactable = false;
        guideMenu.SetActive(true);
    }

    public void CloseGuide()
    {
        guideMenu.SetActive(false);
        startMenu.SetActive(true);
        usernameField.interactable = true;
    }

    public void SendDisconnectPermission()
    {
        ClientSend.SendDisconnectRespond();
    }

    public void DisconnectFromServer()
    {
        startMenu.SetActive(true);
        readyButton.SetActive(false);
        startButton.SetActive(false);
        usernameField.interactable = true;
        mainPlayerSlots.gameObject.SetActive(false);

    }

    public void SendHug()
    {
        ClientSend.SendHug();
    }

    public void ReadyButton()
    {
        GameManager.players[Client.instance.myId].isReady = !GameManager.players[Client.instance.myId].isReady;

        ClientSend.SendReadyChangeState(GameManager.players[Client.instance.myId].isReady);
    }

    public void SetStartButtonInteract(bool _state)
    {
        startButton.GetComponent<Button>().interactable = _state;
    }

    public void StartButton()
    {
        ClientSend.PressStartButton();
    }

    public void SetHost(int _id)
    {
        if (_id == 1)
        {
            startButton.SetActive(true);
            startButton.GetComponent<Button>().interactable = false;
            
            ClientSend.SendReadyChangeState(true);
        }
        else
        {
            readyButton.SetActive(true);
        }
    }

    public void ChangeToArena()
    {
        StartCoroutine(TransitionChange());
    }

    public IEnumerator TransitionChange()
    {
        Transition.gameObject.SetActive(true);
        Transition.SetBool("isFadeOut", true);
        yield return new WaitForSeconds(1f);
        StartCoroutine(LoadAsynchronously(1));
    }

    IEnumerator LoadAsynchronously (int _index)
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

    public void CloseGame()
    {
        Application.Quit();
    }

}
