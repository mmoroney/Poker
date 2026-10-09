using Combinatorics;
using Framework;
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
ushort[] encodedArray = new ushort[n];

for (int i = 0; i < n; i++) {
    byte[] combo = BijectiveMapping.Decode(i, 5);
    Card[] cards = Array.ConvertAll(combo, c => new Card(c));
    FiveCards five = new(cards);
    ushort encoded = Encoder.Encode(five);
    encodedArray[i] = encoded;
    if (i % 100000 == 0) {
        Console.WriteLine($"Processed {i} hands");
    }
}

byte[] byteArray = new byte[encodedArray.Length * sizeof(ushort)];
Buffer.BlockCopy(encodedArray, 0, byteArray, 0, byteArray.Length);

File.WriteAllBytes(outputPath, byteArray);

