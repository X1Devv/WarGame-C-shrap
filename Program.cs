using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        StartGame();
    }

    static (List<string>, List<string>) PoolListCards()
    {
        List<string> PoolCards = new List<string>
        {
            "Jack", "Queen", "King", "Ace",
            "2", "3", "4", "5", "6", "7", "8", "9", "10"
        };

        List<string> deck = new List<string>();
        foreach (var card in PoolCards)
        {
            deck.Add(card);
            deck.Add(card);
        }

        List<string> shuffledDeck = ShuffleDeck(deck);

        List<string> CardPoolPlayer1 = new List<string>();
        List<string> CardPoolPlayer2 = new List<string>();

        for (int i = 0; i < shuffledDeck.Count; i++)
        {
            if (i % 2 == 0)
                CardPoolPlayer1.Add(shuffledDeck[i]);
            else
                CardPoolPlayer2.Add(shuffledDeck[i]);
        }

        return (CardPoolPlayer1, CardPoolPlayer2);
    }

    static List<string> ShuffleDeck(List<string> deck)
    {
        Random rng = new Random();
        int n = deck.Count;
        while (n > 1)
        {
            int k = rng.Next(n--);
            var temp = deck[n];
            deck[n] = deck[k];
            deck[k] = temp;
        }
        return deck;
    }

    static (string, string, int, int) DisplayCard(List<string> CardPoolPlayer1, List<string> CardPoolPlayer2)
    {
        return (CardPoolPlayer1.Count > 0 ? CardPoolPlayer1[0] : "No cards",
                CardPoolPlayer2.Count > 0 ? CardPoolPlayer2[0] : "No cards",
                CardPoolPlayer1.Count,
                CardPoolPlayer2.Count);
    }

    static string RoundEvent(int winner)
    {
        if (winner == 1) return "Player 1 wins this round!";
        if (winner == 2) return "Player 2 wins this round!";
        if (winner == 3) return "War!";
        return "";
    }

    static int GetCardValue(string card)
    {
        Dictionary<string, int> cardValues = new Dictionary<string, int>
        {
            { "2", 2 },
            { "3", 3 },
            { "4", 4 },
            { "5", 5 },
            { "6", 6 },
            { "7", 7 },
            { "8", 8 },
            { "9", 9 },
            { "10", 10 },
            { "Jack", 11 },
            { "Queen", 12 },
            { "King", 13 },
            { "Ace", 14 }
        };

        return cardValues[card];
    }

    static int LogicProgram(List<string> CardPoolPlayer1, List<string> CardPoolPlayer2)
    {
        if (CardPoolPlayer1.Count == 0 && CardPoolPlayer2.Count == 0)
            return 0;
        if (CardPoolPlayer1.Count == 0)
            return 2;
        if (CardPoolPlayer2.Count == 0)
            return 1;

        string cardPlayer1 = CardPoolPlayer1[0];
        string cardPlayer2 = CardPoolPlayer2[0];

        int valuePlayer1 = GetCardValue(cardPlayer1);
        int valuePlayer2 = GetCardValue(cardPlayer2);

        if (valuePlayer1 > valuePlayer2)
            return 1;
        else if (valuePlayer2 > valuePlayer1)
            return 2;
        else
            return 3;
    }

    static void WarEvent(List<string> CardPoolPlayer1, List<string> CardPoolPlayer2)
    {
        if (CardPoolPlayer1.Count < 4)
        {
            Console.WriteLine("Player 1 does not have enough cards for war.");
            Environment.Exit(0);
        }
        else if (CardPoolPlayer2.Count < 4)
        {
            Console.WriteLine("Player 2 does not have enough cards for war.");
            Environment.Exit(0);
        }

        List<string> warCards = new List<string>();
        for (int i = 0; i < 3; i++)
        {
            warCards.Add(CardPoolPlayer1[0]);
            CardPoolPlayer1.RemoveAt(0);
            warCards.Add(CardPoolPlayer2[0]);
            CardPoolPlayer2.RemoveAt(0);
        }

        int warWinner = LogicProgram(CardPoolPlayer1, CardPoolPlayer2);
        if (warWinner == 1)
        {
            CardPoolPlayer1.AddRange(warCards);
            CardPoolPlayer1.Add(CardPoolPlayer2[0]);
            CardPoolPlayer2.RemoveAt(0);
        }
        else if (warWinner == 2)
        {
            CardPoolPlayer2.AddRange(warCards);
            CardPoolPlayer2.Add(CardPoolPlayer1[0]);
            CardPoolPlayer1.RemoveAt(0);
        }
        else
        {
            Console.WriteLine("War continues!");
            WarEvent(CardPoolPlayer1, CardPoolPlayer2);
        }
    }

    static void StartGame()
    {
        var (CardPoolPlayer1, CardPoolPlayer2) = PoolListCards();

        while (true)
        {
            if (CardPoolPlayer1.Count == 0 || CardPoolPlayer2.Count == 0)
            {
                Console.WriteLine("Game end!");
                break;
            }

            var (cardPlayer1, cardPlayer2, CountCards1, CountCards2) = DisplayCard(CardPoolPlayer1, CardPoolPlayer2);
            int winner = LogicProgram(CardPoolPlayer1, CardPoolPlayer2);

            Console.WriteLine("##############################################################################");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine($"                                    [Player 2]");
            Console.WriteLine($"        Card>|>{cardPlayer2}<|                                     ");
            Console.WriteLine($"        Cards left|>{CountCards2}<|                                ");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine($"                                   {RoundEvent(winner)}            ");
            Console.WriteLine("");
            Console.WriteLine("                                                                    ");
            Console.WriteLine($"        Cards left|>{CountCards1}<|                                 ");
            Console.WriteLine($"        Card>|>{cardPlayer1}<|                                      ");
            Console.WriteLine("                                    [Player 1]");
            Console.WriteLine("------------------------------------------------------------------------------");
            Console.WriteLine("##############################################################################");

            Console.WriteLine("Press enter");
            Console.ReadLine();

            if (winner == 1)
            {
                CardPoolPlayer1.Add(CardPoolPlayer1[0]);
                CardPoolPlayer1.Add(CardPoolPlayer2[0]);
            }
            else if (winner == 2)
            {
                CardPoolPlayer2.Add(CardPoolPlayer2[0]);
                CardPoolPlayer2.Add(CardPoolPlayer1[0]);
            }
            else if (winner == 3)
            {
                Console.WriteLine("War! All players place 3 cards.");
                WarEvent(CardPoolPlayer1, CardPoolPlayer2);
            }

            CardPoolPlayer1.RemoveAt(0);
            CardPoolPlayer2.RemoveAt(0);
            Console.Clear();
        }
    }
}
