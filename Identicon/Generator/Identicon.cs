using System;
using System.Text;
using System.Collections;
using System.Security.Cryptography;

namespace Generator
{
	public class Identicon
	{
		private string _input;
		private byte[] _hash;
		private int _foregroundColor;

		private readonly int GRID_SIZE = 6;
		private bool[][] _grid;


		public Identicon(string input)
		{	
			_input = input;
			_hash = GetInputHash();
			_foregroundColor = GetColorFromHash();
			_grid = GenerateGridFromHash();
		}

		public bool[][] GetGrid() => _grid;
		public int GetForegroundColor() => _foregroundColor;
		public byte[] GetHash() => _hash;

		private bool[][] GenerateGridFromHash()
		{
			if(_hash.Length <9)
				throw new ArgumentException("The hash array must contain at least 9 elements");

			var grid = new bool[GRID_SIZE][];
			
			for(int i = 3; i < 9 ; i++)
			{
				var row = new bool[GRID_SIZE];
				byte currentByte = _hash[i];
				for(int bitIndex = 0; bitIndex<3; bitIndex++)
				{
					int bitmask = 1 << (7 - bitIndex);
					row[bitIndex] = (currentByte & bitmask) != 0;
				}

				for(int bitIndex = row.Length -1 ; bitIndex >= 3; bitIndex--)
				{
					row[bitIndex] = row[(row.Length - 1) - bitIndex];
				}
				grid[i-3] = row;
			}
			return grid;
		}

		private int GetColorFromHash()
		{
			if(_hash.Length <3)
			       	throw new ArgumentException("The hash array must contain at least 3 elemnnts");

			byte red = _hash[0];
			byte green = _hash[1];
			byte blue = _hash[2];

			return (red << 16) | (green << 8) | blue;
		}


		private byte[] GetInputHash()
		{
			using(MD5 hasher = MD5.Create())
			{
				Encoding encoding = Encoding.UTF8;
				return hasher.ComputeHash(encoding.GetBytes(_input));
			}
		}
	}
}
