using System;

namespace AzureTableStorageMagic.Specifications.Support
{
    internal static class RandomGenerator
    {
        private readonly static Random Random = new Random();

        internal static bool Boolean()
        {
            return Random.Next(0, 2) == 0;
        }

        internal static string Character(string[] characters)
        {
            var index = Random.Next(0, characters.Length);

            return characters[index];
        }
    }
}