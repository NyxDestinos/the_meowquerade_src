using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    public int id;
    public string username;
    public int roleID;
    public int playerHealth;
    public int[] position = new int[] { 0, 0 };
    public bool isReady = false;
    public bool isTargetable = false;

    public PlayerSprite playerPrefab;

    public Vector3 targetPoint;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        targetPoint = new Vector3(0f,0f,0f);
    }

    public void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPoint + new Vector3(0f,0f,-2.5f) , Time.deltaTime * 2);
    }

    public void getNextPoint(int _x, int _y, Vector3 _hexLocalPosition)
    {
        position[0] = _x;
        position[1] = _y;
        targetPoint = _hexLocalPosition;
    }
}
