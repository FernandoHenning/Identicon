using Generator;

namespace IdenticonTests
{
    public class IdenticonTests
    {
        [Fact]
        public void GetGrid_ProducesSymmetricalGrid()
        {
            var identicon = new Identicon("test");
            bool[][] grid = identicon.Grid;

            for (int i = 0; i < grid.Length; i++)
            {
                for (int j = 0; j < grid[i].Length / 2; j++)
                {
                    Assert.Equal(grid[i][j], grid[i][grid[i].Length - 1 - j]);
                }
            }
        }

	[Fact]
	public void GetForegroundColor_ReturnsExpectedColor()
	{
		var identicon = new Identicon("test");
		int color = identicon.ForegroundColor;

		byte[] hash = identicon.Hash;

		int expectedColor = (hash[0] << 16) | (hash[1] << 8) | hash[2];

		Assert.Equal(expectedColor, color);
	}
    }
}
