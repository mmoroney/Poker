using Framework;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;

var handsOption = new Option<string>("--hands", "Comma-separated list of hands (required)") { IsRequired = true };
var boardOption = new Option<string?>("--board", () => null, "Optional board (1-5 cards)");

var root = new RootCommand("EquityCalculator") { handsOption, boardOption };

root.SetHandler((string handsValue, string? boardValue) => {
    ValidateInputs(handsValue, boardValue);
}, handsOption, boardOption);

int exitCode = await root.InvokeAsync(args);
Environment.Exit(exitCode);

static void ValidateInputs(string handsValue, string? boardValue)
{
    // split hands by comma
    string[] handStrings = handsValue.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
    if (handStrings.Length < 2)
    {
        Console.Error.WriteLine("Error: at least two hands must be supplied in --hands (comma-separated).);");
        Environment.Exit(1);
    }

    // parse each hand into Card[]
    List<Card[]> hands = new();
    int cardsPerHand = -1;
    var globalSeen = new HashSet<int>(); // use Rank.Value*13+Suit.Value to detect duplicates

    for (int h = 0; h < handStrings.Length; h++)
    {
        string hs = handStrings[h];
        if (hs.Length % 2 != 0)
        {
            Console.Error.WriteLine($"Invalid hand format for hand #{h + 1}: '{hs}'");
            Environment.Exit(1);
        }

        int count = hs.Length / 2;
        // only allow 2, 4, 5 or 6 cards per hand
        if (!(count == 2 || count == 4 || count == 5 || count == 6))
        {
            Console.Error.WriteLine($"Invalid number of cards in hand #{h + 1}: {count}. Must be 2, 4, 5 or 6.");
            Environment.Exit(1);
        }

        if (cardsPerHand == -1) cardsPerHand = count;
        else if (cardsPerHand != count)
        {
            Console.Error.WriteLine("All hands must have the same number of cards.");
            Environment.Exit(1);
        }

        var cards = new Card[count];
        var seen = new HashSet<int>();
        for (int i = 0; i < count; i++)
        {
            string token = hs.Substring(i * 2, 2);
            try
            {
                var card = Card.Parse(token);
                cards[i] = card;
                int v = card.Rank.Value * 13 + card.Suit.Value;
                if (!seen.Add(v))
                {
                    Console.Error.WriteLine($"Duplicate card '{token}' within hand #{h + 1}.");
                    Environment.Exit(1);
                }
                if (!globalSeen.Add(v))
                {
                    Console.Error.WriteLine($"Duplicate card '{token}' across hands.");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Invalid card '{token}' in hand #{h + 1}: {ex.Message}");
                Environment.Exit(1);
            }
        }

        hands.Add(cards);
    }

    // optional board validation
    if (!string.IsNullOrWhiteSpace(boardValue))
    {
        string b = boardValue.Trim();
        if (b.Length % 2 != 0)
        {
            Console.Error.WriteLine("Invalid board format.");
            Environment.Exit(1);
        }
        int boardCount = b.Length / 2;
        if (boardCount < 1 || boardCount > 5)
        {
            Console.Error.WriteLine("Board must contain between 1 and 5 cards if supplied.");
            Environment.Exit(1);
        }

        var boardSeen = new HashSet<int>();
        for (int i = 0; i < boardCount; i++)
        {
            string token = b.Substring(i * 2, 2);
            try
            {
                var card = Card.Parse(token);
                int v = card.Rank.Value * 13 + card.Suit.Value;
                if (!boardSeen.Add(v))
                {
                    Console.Error.WriteLine($"Duplicate card '{token}' within board.");
                    Environment.Exit(1);
                }
                if (!globalSeen.Add(v))
                {
                    Console.Error.WriteLine($"Duplicate card '{token}' between board and hands.");
                    Environment.Exit(1);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Invalid board card '{token}': {ex.Message}");
                Environment.Exit(1);
            }
        }
    }

    // perform equity calculation based on cards per hand
    string boardParam = boardValue ?? string.Empty;
    Rational[] equities;
    string gameType;

    if (cardsPerHand == 2)
    {
        gameType = "holdem";
        equities = Framework.Games.EquityApi.CalculateHoldem(boardParam, handStrings);
    }
    else if (cardsPerHand == 4)
    {
        gameType = "omaha high";
        equities = Framework.Games.EquityApi.CalculateOmahaHigh(boardParam, handStrings);
    }
    else
    {
        gameType = "omaha high low";
        equities = Framework.Games.EquityApi.CalculateOmahaHighLow(boardParam, handStrings, cardsPerHand);
    }

    // output results
    Console.WriteLine(gameType);
    if (!string.IsNullOrWhiteSpace(boardParam))
        Console.WriteLine($"Board: {boardParam}");

    for (int i = 0; i < handStrings.Length; i++)
    {
        double pct = equities[i].Value * 100.0;
        Console.WriteLine($"{handStrings[i]}: {pct:F2}%");
    }
}
