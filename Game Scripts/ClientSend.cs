using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /*private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }*/

    #region Packets
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void SendHug()
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendHug))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendDisconnectRespond()
    {
        using (Packet _packet = new Packet((int)ClientPackets.sendDisconnectRespond))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendReadyChangeState(bool _isReady)
    {
        using (Packet _packet = new Packet((int)ClientPackets.changeReadyState))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_isReady);

            SendTCPData(_packet);
        }
    }

    public static void PressStartButton()
    {
        using (Packet _packet = new Packet((int)ClientPackets.PressStartButton))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void LoadArenaComplete()
    {
        using (Packet _packet = new Packet((int)ClientPackets.LoadArenaComplete))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendRoleIDRequest()
    {
        using (Packet _packet = new Packet((int)ClientPackets.SendRoleIDRequest))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendPlayerPosition(int _x, int _y)
    {
        using (Packet _packet = new Packet((int)ClientPackets.SendPlayerPosition))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_x);
            _packet.Write(_y);

            SendTCPData(_packet);
        }
    }

    public static void EndTurn()
    {
        using (Packet _packet = new Packet((int)ClientPackets.EndTurn))
        {
            _packet.Write(Client.instance.myId);

            SendTCPData(_packet);
        }
    }

    public static void SendCardEffectToTarget(int _targetPlayerID, int _cardID)
    {
        using (Packet _packet = new Packet((int)ClientPackets.SendCardEffectToTarget))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(_targetPlayerID);
            _packet.Write(_cardID);

            SendTCPData(_packet);
        }
    }

    #endregion
}
