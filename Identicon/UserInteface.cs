using System.Text.RegularExpressions;
using Spectre.Console;

namespace UserInterface
{
    public partial class UI
    {
        [GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
        private static partial Regex EmailRegex();

        public static bool AskToSaveImage() => AnsiConsole.Confirm("Save as PNG image?");

        public static void PromptErrorWhileSavingImage() => AnsiConsole.MarkupLine("[red]Something went wrong while saving the image!:([/]");

        public static void PrintIdenticon(bool[][] grid, int foregroundColor)
        {
            var canvas = new Canvas(6, 6);

            byte red = (byte)((foregroundColor >> 16) & 0xff);
            byte green = (byte)((foregroundColor >> 8) & 0xff);
            byte blue = (byte)(foregroundColor & 0xff);
            Color color = new Color(red, green, blue);

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 6; col++)
                {
                    if (grid[col][row])
                    {
                        canvas.SetPixel(row, col, color);
                    }
                    else
                    {
                        canvas.SetPixel(row, col, Color.White);
                    }
                }
            }
            AnsiConsole.Write(canvas);
        }

        public static string AskForEmail() => AnsiConsole.Prompt(
                new TextPrompt<string>("Please enter your email:")
                    .Validate(email =>
                        EmailRegex().IsMatch(email)
                        ? ValidationResult.Success()
                        : ValidationResult.Error("[red]That's not a valid email address[/]")));


    }
}
