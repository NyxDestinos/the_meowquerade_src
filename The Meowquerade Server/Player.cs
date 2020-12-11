using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MainGameServer
{
    class Player
    {
        public int id;
        public int roleId;
        public string username;


        public int[] position = new int[] { 0, 0 };
        public int playerHealth = 5;
        public List<int> cardEffect = new List<int>();


        public List<int> cardInHand = new List<int>();
        public bool isReady = false;
        public bool isLoadComplete = false;
        public bool isRoleReveal;

        public Player(int _id, string _username)
        {
            id = _id;
            username = _username;

        }

        public void Update()
        {
        }

        public void DrawPhase(int _cardAmount)
        {
            for (int i = 0; i < _cardAmount; i++)
            {
                cardInHand.Add(Arena.deck.Draw());
            }
            
        }

        public void damageCalculation()
        {
            int _damage = 0;
            int _heal = 0;
            for (int i =0; i < cardEffect.Count; i++)
            {
                switch (cardEffect[i])
                {
                    case 0:
                        _damage += 1;
                        break;
                    case 1:
                        _damage -= 1;
                        break;
                    case 2:
                        _heal += 1;
                        break;
                }
            }
            if (_damage < 0) _damage = 0;

            playerHealth -= (_damage - _heal);
            if (playerHealth <= 0)
            {
                Arena.TurnOrder.Remove(id);
            }
                
        }
    }
}
