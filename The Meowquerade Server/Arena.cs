using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MainGameServer
{
    class Arena
    {
        public static List<int> TurnOrder;
        public static List<int> RoleDeck;
        public static Deck deck = new Deck();
        public static int currentTurnOrder = 0;
        public static int[,] startPosition = new int[6,2] { { -2, 2 }, { -2, 0 }, { 0, -2 }, { 2, -2 }, { 2, 0 }, { 0, 2 } };
        public static List<int[]> startPositionList = new List<int[]>();

        public Arena()
        {
            TurnOrder = new List<int>();
            RoleDeck = new List<int>();
            startPositionList = new List<int[]>();

            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                startPositionList.Add(new int[] { startPosition[i,0], startPosition[i,1]});
            }

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                TurnOrder.Add(i);
            }

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    int _index = deck.Draw();
                    Server.clients[i].player.cardInHand.Add(_index);
                }
            }

        }

        public void RandomTurnOrder()
        {
            List<int> _temp = new List<int>();
            Random rnd = new Random();
           
            for (int i = 0; i < Server.MaxPlayers; i++)
            {
                int _num = rnd.Next(0, TurnOrder.Count);
                _temp.Add(TurnOrder[_num]);
                TurnOrder.RemoveAt(_num);
            }
            TurnOrder = _temp;
            for (int i = 0; i < TurnOrder.Count; i++)
            {
                Console.WriteLine(TurnOrder[i]);
            }
            
        }

        public static void GiveRoleToPlayer()
        {
            Random rnd = new Random();
            for (int i = 0; i < Server.MaxPlayers - 1; i++)
            {
                RoleDeck.Add(0);
            }

            RoleDeck.Add(1);

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                int num = rnd.Next(0, RoleDeck.Count);
                Server.clients[i].player.roleId = RoleDeck[num];
                RoleDeck.RemoveAt(num);
            }
        }

        public static void GiveStartPositionToPlayer()
        {
            Random rnd = new Random();

            for (int i = 1; i <= Server.MaxPlayers; i++)
            {
                int num = rnd.Next(0, startPositionList.Count);
                Server.clients[i].player.position = startPositionList[num];
                startPositionList.RemoveAt(num);
            }
        }

        public int GetCurrentPlayerId()
        {
            return TurnOrder[currentTurnOrder];
        }

        public void NextTurn()
        {
            if (TurnOrder.Count - 1 > currentTurnOrder)
            {
                currentTurnOrder++;
                Console.WriteLine(TurnOrder[currentTurnOrder]);
            }
            else
            {
                currentTurnOrder = 0;
                for (int i = 1; i <= Server.clients.Count; i++)
                {
                    Server.clients[i].player.damageCalculation();
                }

                int _GuestNum = 0;
                int _BetrayalNum = 0;
                for (int i = 0; i < TurnOrder.Count; i++)
                {
                    if (Server.clients[TurnOrder[i]].player.roleId == 0) _GuestNum++;
                    else _BetrayalNum++;
                }

                if (_BetrayalNum >= _GuestNum)
                {
                    Console.WriteLine("Betrayal Win");
                    ServerSend.SendWin(false);
                }
                    
                if (_BetrayalNum == 0)
                {
                    Console.WriteLine("Guest Win");
                    ServerSend.SendWin(true);
                }



                for (int i = 1; i <= Server.clients.Count; i++)
                {
                    ServerSend.SendTurnCalculation(i);
                    Server.clients[i].player.cardEffect.Clear();
                }
                Thread.Sleep(3000);
                
            }
        }
    }
}
