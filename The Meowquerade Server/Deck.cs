using System;
using System.Collections.Generic;
using System.Text;

namespace MainGameServer
{
    class Deck
    {
        public static Deck instance;
        public List<int> deckList = new List<int>();

        public Deck()
        {
            if (instance != null) return;
            else
            {
                instance = this;
                RefillDeck();
            }

        }

        public void RefillDeck()
        {
            for (int i = 0; i < 20; i++)
            {
                deckList.Add(0);
            }

            for (int i = 0; i < 20; i++)
            {
                deckList.Add(1);
            }

            for (int i = 0; i < 10; i++)
            {
                deckList.Add(2);
            }
            DeckShuffle();
        }

        public void DeckShuffle()
        {
            Random rnd = new Random();
            List<int> _tempDeck = new List<int>();

            while (deckList.Count != 0)
            {
                int num = rnd.Next(0, deckList.Count);
                _tempDeck.Add(deckList[num]);
                deckList.RemoveAt(num);
            }
            deckList.AddRange(_tempDeck);

        }

        public int Draw()
        {
            if (deckList.Count == 0)
            {
                RefillDeck();
            }

            int cardIDDraw = deckList[0];
            deckList.RemoveAt(0);
            //Console.WriteLine($"Card Drawn : {cardIDDraw}");
            return cardIDDraw;
        }
    }
}
