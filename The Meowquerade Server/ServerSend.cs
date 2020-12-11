using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MainGameServer
{
    class ServerSend
    {
        #region MainFunction
        private static void sendTCPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].tcp.SendData(_packet);
        }

        /*private static void sendUDPData(int _toClient, Packet _packet)
        {
            _packet.WriteLength();
            Server.clients[_toClient].udp.SendData(_packet);
        }*/
        

        private static void sendTCPDatatoAll(Packet _packet)
        {
            _packet.WriteLength();

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].tcp.SendData(_packet);
            }
        }

        private static void sendTCPDatatoAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].tcp.SendData(_packet);
                }
            }
        }

        /*private static void sendUDPDatatoAll(Packet _packet)
        {
            _packet.WriteLength();

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                Server.clients[i].udp.SendData(_packet);
            }
        }

        private static void sendUDPDatatoAll(int _exceptClient, Packet _packet)
        {
            _packet.WriteLength();

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                if (i != _exceptClient)
                {
                    Server.clients[i].udp.SendData(_packet);
                }
            }
        }*/
        #endregion


        #region Packets
        public static void Welcome(int _toClient, string _msg)
        {
            using (Packet _packet = new Packet((int)ServerPackets.welcome))
            {
                _packet.Write(_msg);
                _packet.Write(_toClient);

                sendTCPData(_toClient, _packet);

            }
        }

        public static void SpawnPlayer(int _toClientId, Player _player)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayer))
            {
                _packet.Write(_player.id);
                _packet.Write(_player.username);
                _packet.Write(_player.position[0]);
                _packet.Write(_player.position[1]);

                sendTCPData(_toClientId ,_packet);
            }
        }

        public static void SpawnPlayerInLobby(int _toClient)
        {
            using (Packet _packet = new Packet((int)ServerPackets.spawnPlayerInLobby))
            {
                _packet.Write(Server.clients.Count);
                foreach (Client _client in Server.clients.Values)
                {
                    if (_client == null || _client.player == null)
                    {
                        _packet.Write(_client.id);
                        _packet.Write("");
                    }
                    else
                    {
                        _packet.Write(_client.player.id);
                        _packet.Write(_client.player.username);
                    }

                }

                sendTCPData(_toClient, _packet);
            }
        }

        public static void SendDisconnectConfirm(int _disconnectID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.sendDisconnectComfirm))
            {
                _packet.Write(_disconnectID);

                sendTCPDatatoAll(_packet);
            }
        }

        public static void SendReadyChange(int _ReadyChangeID, bool _changeTo)
        {
            using (Packet _packet = new Packet((int)ServerPackets.ReadyChange))
            {
                _packet.Write(_ReadyChangeID);
                _packet.Write(_changeTo);
                _packet.Write(Server.isEveryoneReady());
                

                sendTCPDatatoAll(_packet);
            }
        }

        public static void SetHostCanStart(bool _state)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SetHostCanStart))
            {
                _packet.Write(_state);


                sendTCPData(1, _packet);
            }
        }

        public static void SendPlayerToGame()
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendPlayerToGame))
            {
                _packet.Write(true);
                sendTCPDatatoAll(_packet);
            }
        }

        public static void StartGame()
        {
            using (Packet _packet = new Packet((int)ServerPackets.StartGame))
            {
                _packet.Write(true);
                sendTCPDatatoAll(_packet);
                
            }
        }

        public static void SendTurnToCurrentPlayer()
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendTurnToCurrentPlayer))
            {
                _packet.Write(true);
                int _currentPlayer = Arena.TurnOrder[Arena.currentTurnOrder];
                sendTCPData(_currentPlayer, _packet);
                Server.clients[_currentPlayer].player.DrawPhase(2);
                SendPlayerCardInHand(_currentPlayer);
            }
        }

        public static void SendRoleToPlayer(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendRoleToPlayer))
            {
                _packet.Write(Server.clients[_playerId].player.roleId);

                sendTCPData(_playerId, _packet);
            }
        }

        public static void EveryoneKnowTheirRole()
        {
            using (Packet _packet = new Packet((int)ServerPackets.EveryoneKnowTheirRole))
            {
                _packet.Write(true);

                sendTCPDatatoAll(_packet);

                Thread.Sleep(5000);
                SendTurnToCurrentPlayer();

            }
        }

        public static void SendPlayerPositionToAll(int _playerIDChange)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendPlayerPositionToAll))
            {
                _packet.Write(_playerIDChange);
                _packet.Write(Server.clients[_playerIDChange].player.position[0]);
                _packet.Write(Server.clients[_playerIDChange].player.position[1]);

                sendTCPDatatoAll(_playerIDChange, _packet);
            }
        }

        public static void SendPlayerCardInHand(int _playerId)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendPlayerCardInHand))
            {
                _packet.Write(Server.clients[_playerId].player.cardInHand.Count);

                for (int i = 0; i < Server.clients[_playerId].player.cardInHand.Count; i++)
                {
                    _packet.Write(Server.clients[_playerId].player.cardInHand[i]);
                }

                sendTCPData(_playerId, _packet);
            }
        }

        public static void SendTurnCalculation(int _playerID)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendTurnCalculation))
            {
                _packet.Write(Server.clients[_playerID].player.cardEffect.Count);
                _packet.Write(Server.clients[_playerID].player.playerHealth);

                for (int i = 1; i <= Server.clients.Count; i++)
                {
                    _packet.Write(Server.clients[i].player.playerHealth);
                }

                for (int i = 0; i < Server.clients[_playerID].player.cardEffect.Count; i++)
                {
                    _packet.Write(Server.clients[_playerID].player.cardEffect[i]);
                }

                sendTCPData(_playerID, _packet);
            }
        }

        public static void SendWin(bool _isGuestWin)
        {
            using (Packet _packet = new Packet((int)ServerPackets.SendWin))
            {
                _packet.Write(_isGuestWin);

                sendTCPDatatoAll(_packet);
            }
        }

        #endregion
    }
}
