using System.Text;
using System.Security.Cryptography;

namespace Generator
{
    public class Identicon
    {
        private readonly string _input;
        private readonly byte[] _hash;
        private readonly int _foregroundColor;
        private readonly bool[][] _grid;

        public Identicon(string input)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _hash = ComputeHash(_input);
            _foregroundColor = GetColorFromHash(_hash);
            _grid = GenerateGridFromHash(_hash);
        }

        public bool[][] Grid => _grid;
        public int ForegroundColor => _foregroundColor;
        public byte[] Hash => _hash;

        /// <summary>
        /// Generates a grid of boolean values representing the identicon.
        /// </summary>
        /// <param name="hash">The hash value to generate the grid from.</param>
        /// <returns>A bidimensional array representing the grid.</returns>
        /// <exception cref="ArgumentException">Thrown when the hash array is not valid.</exception>
        /// <exception cref="ArgumentnullException">Thrown when the hash array is null.</exception>
        private static bool[][] GenerateGridFromHash(byte[] hash)
        {
            const int RequiredHashLength = 9;
            const int GridSize = 6; // Number of rows/columns in the grid
            const int BitsToExtract = 3;

            if (hash == null)
                throw new ArgumentNullException(nameof(hash), "The hash array cannot be null.");

            if (hash.Length < RequiredHashLength)
                throw new ArgumentException($"The hash array must contain at least {RequiredHashLength} elements", nameof(hash));

            var grid = new bool[GridSize][];

            for (int i = 0; i < GridSize; i++)
            {
                var row = new bool[GridSize];
                byte currentByte = hash[i + 3];

                for (int bitIndex = 0; bitIndex < BitsToExtract; bitIndex++)
                {
                    int bitmask = 1 << (7 - bitIndex);
                    row[bitIndex] = (currentByte & bitmask) != 0;
                }

                for (int bitIndex = BitsToExtract; bitIndex < GridSize; bitIndex++)
                {
                    row[bitIndex] = row[GridSize - 1 - bitIndex];
                }

                grid[i] = row;
            }

            return grid;
        }

        /// <summary>
        /// Returns a color, represented as an integter, from the first 3 digits of the hash value.
        /// </summary>
        /// <param name="hash">The hash value to generate the grid from.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>An integer representing a color.</returns>
        private static int GetColorFromHash(byte[] hash)
        {
            if (hash.Length < 3)
                throw new ArgumentException("The hash array must contain at least 3 elements", nameof(hash));

            byte red = hash[0];
            byte green = hash[1];
            byte blue = hash[2];

            return (red << 16) | (green << 8) | blue;
        }
        /// <summary>
        /// Compute the hash from the input. Uses the MD5 hash function.
        /// </summary>
        /// <param name="input">The string input to hash.</param>
        /// <returns>A byte array with the hash value.</returns>
        private static byte[] ComputeHash(string input)
        {
            return MD5.HashData(Encoding.UTF8.GetBytes(input));
        }
    }
}
