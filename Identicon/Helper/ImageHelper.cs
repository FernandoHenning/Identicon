using System;
using System.Runtime.CompilerServices;
using SkiaSharp;

namespace Helper
{
	public class ImageHelper
	{
		private const int IMAGE_WIDTH = 600;
		private const int IMAGE_HEIGHT = 600;


		public byte[] GenerateImageFromGrid(bool[][] grid, int foregroundColor)
		{
			using (var surface = CreateSurface())
			{
				var canvas = surface.Canvas;
				var foregroundPaint = CreatePaintWithColor(foregroundColor);

				for (int row = 0; row < 6; row++)
				{
					for (int col = 0; col < 6; col++)
					{
						if (grid[row][col])
							canvas.DrawRect(col * 100 , row * 100, 100, 100, foregroundPaint);			
					}
				}

				using (var image = surface.Snapshot())
				using (var skData = image.Encode(SKEncodedImageFormat.Png, 100))
				{
					return skData.ToArray();
				}
			}
		}

		public void WriteImageToDisk(byte[] imageData, string filename)
		{
			string myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
			string identiconsFolderPath = Path.Combine(myPicturesPath, "Identicons");
			Directory.CreateDirectory(identiconsFolderPath);

			string newIdenticonPath = Path.Combine(identiconsFolderPath, filename);

			File.WriteAllBytes(newIdenticonPath, imageData);
		}

		private SKPaint CreatePaintWithColor(int colorInt)
		{
			byte red = (byte)((colorInt >> 16) & 0xff);
			byte green = (byte)((colorInt >> 8) & 0xff);
			byte blue = (byte)(colorInt & 0xff);
			var color = new SKColor(red, green, blue);

			return new SKPaint
			{
				Style = SKPaintStyle.Fill,
				Color = color
			};
		}


		private SKSurface CreateSurface()
		{
			var imageInfo = new SKImageInfo(IMAGE_WIDTH, IMAGE_HEIGHT);
			var surface = SKSurface.Create(imageInfo);
			surface.Canvas.Clear(SKColors.White);
			return surface;
		}
	}
}
