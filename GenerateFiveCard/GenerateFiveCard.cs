using Combinatorics;
using Framework;
using GenerateFiveCard;
using System.Xml.Serialization;
using System;
using System.IO;

// Require an output path argument
if (args.Length < 1) {
    Console.WriteLine("Warning: outputPath argument not supplied. Exiting.");
    return;
}

string outputPath = args[0];

int n = BijectiveMapping.Choose(52, 5);
ValueTuple<int, int, string>[] strengths = new ValueTuple<int, int, string>[n];

for (int i = 0; i < n; i++) {
    byte[] combo = BijectiveMapping.Decode(i, 5);
    Card[] cards = Array.ConvertAll(combo, c => new Card(c));
    FiveCards five = new(cards);
    HandStrength strength = Encoder.Encode(five);
    strengths[i] = (i, strength.Value, strength.ToString());
    if (i % 100000 == 0) {
        Console.WriteLine($"Processed {i} hands");
    }
}

Array.Sort(strengths, (a, b) => a.Item2.CompareTo(b.Item2));

ushort[] compressed = new ushort[n];
ushort currentStrength = 0;
compressed[strengths[0].Item1] = currentStrength;
Console.WriteLine(strengths[0].Item3);

for (int i = 1; i < n; i++) {
    if (strengths[i].Item2 != strengths[i - 1].Item2) {
        currentStrength++;
        Console.WriteLine(strengths[i].Item3);
    }
    compressed[strengths[i].Item1] = currentStrength;

}

byte[] byteArray = new byte[compressed.Length * sizeof(ushort)];
Buffer.BlockCopy(compressed, 0, byteArray, 0, byteArray.Length);

File.WriteAllBytes(outputPath, byteArray);

