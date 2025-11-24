using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KaratExercises
{
    public enum Suit
    {
        Clubs,
        Diamonds, 
        Hearts,
        Spades
    }

    public enum Rank
    {
        Two = 2, Three, Four, Five, Six, Seven, Eight,
        Nine, Ten, Jack = 11, Queen, King, Ace
    }

    public class Card
    {
        public Suit Suit { get; private set; }
        public Rank Rank { get; private set; }

        public Card(Suit Suit, Rank Rank)
        {
            this.Suit = Suit;
            this.Rank = Rank;
        }

        public override string ToString()
        {
            return $"{Rank} of {Suit}";
        }
    }

    public class Deck
    {
        protected List<Card> cards;
        private Random random = new Random();

        public Deck() {
            cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit))){
                foreach(Rank rank in Enum.GetValues(typeof(Rank)))
                {
                    cards.Add(new Card(suit, rank));
                }
            }
        }

        public void Shuffle() { 
            for(int i = cards.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);

                // Swap cards[i] with the element at random index
                var temp = cards[i];
                cards[i] = cards[j];
                cards[j] = temp;
            }
        }

        public Card DealCard() {
            if (cards.Count == 0) return null;

            Card topCard = cards[0];
            cards.RemoveAt(0);
            return topCard;
        }
    }

    public class BlackjackCard : Card {
        public BlackjackCard(Suit suit, Rank rank) : base(suit, rank) { }

        public int Value
        {
            get {
                if (Rank >= Rank.Jack && Rank <= Rank.King)
                    return 10;
                else if(Rank == Rank.Ace)
                    return 11; // Ace can also be 1, but we'll handle that in the game logic
                else
                    return (int)Rank;
            }
        }

        public bool IsAce => Rank == Rank.Ace;
    }

    public class BlackjackDeck : Deck { 
        public BlackjackDeck()
        {
            cards = new List<Card>();

            foreach (Suit suit in Enum.GetValues(typeof(Suit))) {
                foreach (Rank rank in Enum.GetValues(typeof(Rank))) {
                    cards.Add(new BlackjackCard(suit, rank));
                }
            }
        }

        public new BlackjackCard DealCard() { 
            return (BlackjackCard)base.DealCard();
        }
    }

    public class  BlackjackHand
    {
        private List<BlackjackCard> cards = new List<BlackjackCard>();

        public void AddCard(BlackjackCard card) { 
            cards.Add(card);
        }

        public int Score
        {
            get { 
                int total = 0;
                int aceCount = 0;

                foreach (var card in cards) {
                    total += card.Value;

                    if(card.IsAce) aceCount++;
                }

                // Adjust for Aces (subtract 10 if needed to prevent bust)
                while (total > 21 && aceCount > 0) {
                    total -= 10;
                    aceCount--;
                }

                return total;
            }
        }

        public bool IsBusted => Score > 21;

        public override string ToString()
        {
            return $"Hand: {string.Join(", ", cards)} | Score:{Score}";
        }
    }
}
