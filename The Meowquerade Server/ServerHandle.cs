using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MainGameServer
{
    class ServerHandle
    {
        public static void WelcomeReceived(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();
            string _username = _packet.ReadString();

            Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} | {_username} connected successfully and is now player {_fromClient}");
            if (_fromClient != _clientIdCheck)
            {
                Console.WriteLine($"Player \"{_username}\" (ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})");
            }
            Server.clients[_fromClient].SendIntoGame(_username);

            int _length = Server.clients.Count;

            for (int i = 1; i <= _length; i++)
            {
                if (Server.clients[i] != null)
                {
                    ServerSend.SpawnPlayerInLobby(i);
                }
            }

        }

        public static void SendHug(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient == _clientIdCheck)
            {
                Console.WriteLine($"{Server.clients[_fromClient].player.username}(Player {_fromClient}) send HUG to server <3");
            }
            else
            {
                Console.WriteLine($"Player \"{Server.clients[_fromClient].player.username}\" (ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})");
            }
        }

        public static void SendDisconnectRespond(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient == _clientIdCheck)
            {
                Console.WriteLine($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} has try to disconnect from Server");
                ServerSend.SendDisconnectConfirm(_fromClient);
            }
            else
            {
                Console.WriteLine($"Player \"{Server.clients[_fromClient].player.username}\" (ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})");
            }
        }

        public static void SendReadyChangeState(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient == _clientIdCheck)
            {
                bool _changeTo = _packet.ReadBool();
                Server.clients[_fromClient].player.isReady = _changeTo;
                Console.WriteLine($"{Server.clients[_fromClient].player.username} is changeState to {_changeTo}");
                ServerSend.SendReadyChange(_fromClient, _changeTo);

                if (Server.isEveryoneReady())
                {
                    ServerSend.SetHostCanStart(true);
                }
                else
                {
                    ServerSend.SetHostCanStart(false);
                }
            }
            else
            {
                Console.WriteLine($"Player \"{Server.clients[_fromClient].player.username}\" (ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})");
            }
        }

        public static void PressStartButton(int _fromClient, Packet _packet)
        {
            int _clientIDCheck = _packet.ReadInt();

            if (_fromClient == _clientIDCheck)
            {
                Server.arena = new Arena();
                ServerSend.SendPlayerToGame();
                Server.arena.RandomTurnOrder();
                Arena.GiveRoleToPlayer();
            }
        }

        public static void LoadArenaComplete(int _fromClient, Packet _packet)
        {
            int _clientIdCheck = _packet.ReadInt();

            if (_fromClient == _clientIdCheck)
            {
                Server.clients[_fromClient].player.isLoadComplete = true;

                if (Server.isEveryoneLoadComplete())
                {
                    ServerSend.StartGame();
                }
            }
            else
            {
                Console.WriteLine($"Player \"{Server.clients[_fromClient].player.username}\" (ID : {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})");
            }
        }

        public static void SendRoleIDRequest(int _fromClient, Packet _packet)
        {
            int _clientIDCheck = _packet.ReadInt();

            if (_fromClient == _clientIDCheck)
            {
                ServerSend.SendRoleToPlayer(_fromClient);
                Server.clients[_fromClient].player.isRoleReveal = true;

                if (Server.isEveryoneRevealTheirRole())
                {
                    ServerSend.EveryoneKnowTheirRole();

                    for (int i = 1; i <= Server.MaxPlayers; i++)
                    {
                        ServerSend.SendPlayerCardInHand(i);
                    }
                    
                }
            }
        }

        public static void SendPlayerPosition(int _fromClient, Packet _packet)
        {
            int _clientIDCheck = _packet.ReadInt();

            if (_fromClient == _clientIDCheck)
            {
                Server.clients[_fromClient].player.position[0] = _packet.ReadInt();
                Server.clients[_fromClient].player.position[1] = _packet.ReadInt();
                ServerSend.SendPlayerPositionToAll(_fromClient);
            }
        }

        public static void EndTurn(int _fromClient, Packet _packet)
        {
            int _clientIDCheck = _packet.ReadInt();

            if (_fromClient == _clientIDCheck)
            {
                Server.arena.NextTurn();
                ServerSend.SendTurnToCurrentPlayer();
            }
        }

        public static void SendCardEffectToTarget(int _fromClient, Packet _packet)
        {
            int _clientIDCheck = _packet.ReadInt();
            int _toPlayer = _packet.ReadInt();
            int _cardID = _packet.ReadInt();

            if (_fromClient == _clientIDCheck)
            {
                Server.clients[_fromClient].player.cardInHand.Remove(_cardID);
                Server.clients[_toPlayer].player.cardEffect.Add(_cardID);
            }
        }
        #region Lobby

        #endregion

    }
}
