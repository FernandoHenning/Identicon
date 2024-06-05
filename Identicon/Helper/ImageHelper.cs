using SkiaSharp;
using UserInterface;
namespace Helper
{
    public class ImageHelper
    {
        private const int ImageWidth = 600;
        private const int ImageHeight = 600;
        private const int GridSize = 6;
        private const int CellSize = 100;

        /// <summary>
        /// Generates an image from a boolean grid and foreground color.
        /// </summary>
        /// <param name="grid">The boolean grid representing the image pattern.</param>
        /// <param name="foregroundColor">The color of the foreground elements in the image.</param>
        /// <returns>A byte array containing the PNG image data.</returns>
        public static byte[] GenerateImageFromGrid(bool[][] grid, int foregroundColor)
        {
            if (grid == null)
                throw new ArgumentNullException(nameof(grid));

            if (grid.Length != GridSize || grid.Any(row => row.Length != GridSize))
                throw new ArgumentException("Grid must be 6x6 in size.", nameof(grid));

            using (var surface = CreateSurface())
            {
                var canvas = surface.Canvas;
                using (var foregroundPaint = CreatePaintWithColor(foregroundColor))
                {
                    for (int row = 0; row < GridSize; row++)
                    {
                        for (int col = 0; col < GridSize; col++)
                        {
                            if (grid[row][col])
                                canvas.DrawRect(col * CellSize, row * CellSize, CellSize, CellSize, foregroundPaint);
                        }
                    }
                }

                using (var image = surface.Snapshot())
                using (var skData = image.Encode(SKEncodedImageFormat.Png, 100))
                {
                    return skData.ToArray();
                }
            }
        }

        /// <summary>
        /// Writes image data to disk asynchronously.
        /// </summary>
        /// <param name="imageData">The byte array containing image data.</param>
        /// <param name="filename">The name of the file to save.</param>
        public static async Task WriteImageToDiskAsync(byte[] imageData, string filename)
        {
            if (imageData == null)
                throw new ArgumentNullException(nameof(imageData));

            if (string.IsNullOrWhiteSpace(filename))
                throw new ArgumentException("Filename cannot be null or whitespace.", nameof(filename));

            string myPicturesPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            string identiconsFolderPath = Path.Combine(myPicturesPath, "Identicons");
            Directory.CreateDirectory(identiconsFolderPath);

            string newIdenticonPath = Path.Combine(identiconsFolderPath, filename);

            await File.WriteAllBytesAsync(newIdenticonPath, imageData);
        }

        /// <summary>
        /// Creates an SKPaint object with the specified color.
        /// </summary>
        /// <param name="colorInt">The color as an integer.</param>
        /// <returns>An SKPaint object with the specified color.</returns>
        private static SKPaint CreatePaintWithColor(int colorInt)
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

        /// <summary>
        /// Creates an SKSurface object with the specified image dimensions.
        /// </summary>
        /// <returns>An SKSurface object initialized with the image dimensions.</returns>
        private static SKSurface CreateSurface()
        {
            var imageInfo = new SKImageInfo(ImageWidth, ImageHeight);
            var surface = SKSurface.Create(imageInfo);
            surface.Canvas.Clear(SKColors.White);
            return surface;
        }

        /// <summary>
        /// Save the Identicon as a .PNG images into the <c>~/Images/Identicon</c> folder.
        /// </summary>
        /// <param name="grid">The identicon grid.</param>
        /// <param name="foregroundColor">The color for  foreground cells.</param>
        /// <param name="email">The email entered by the user.</param>
        public static async Task SaveImage(bool[][] grid, int foregroundColor, string email)
        {
            byte[] imageData = GenerateImageFromGrid(grid, foregroundColor);
            try
            {
                await WriteImageToDiskAsync(imageData, $"{email}-identicon.png");
            }
            catch (Exception)
            {
                UI.PromptErrorWhileSavingImage();
            }
        }
    }
}
