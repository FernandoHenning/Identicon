using Generator;
using Helper;
using Spectre.Console;


var userInput = PromptInput();
var identicon = new Identicon(userInput);

PrintIdenticon(identicon.GetGrid(), identicon.GetForegroundColor());
PromptSaveImage();


string PromptInput()
{
	var input = AnsiConsole.Ask<string>("What's your [green]email? [/]");
	return input;
}


void PromptSaveImage()
{
	if (AnsiConsole.Confirm("Save as PNG image?"))
	{
		var imageHelper = new ImageHelper();
		byte[] imageData = imageHelper.GenerateImageFromGrid(identicon.GetGrid(), identicon.GetForegroundColor());
		try
		{
			imageHelper.WriteImageToDisk(imageData, $"{userInput}-identicon.png");
		}
		catch(Exception)
		{
			AnsiConsole.WriteLine("[red]Something went wrong while saving the image!:([/]");
		}
	}
}

void PrintIdenticon(bool[][] grid, int foregroundColor)
{
	var canvas = new Canvas(6,6);

	byte red = (byte) ((foregroundColor >> 16) & 0xff);
	byte green = (byte) ((foregroundColor >> 8) & 0xff);
	byte blue = (byte) (foregroundColor & 0xff);
	Color color = new Color(red, green, blue);

	for(int row = 0; row < 6; row ++)
	{
		for(int col = 0; col < 6; col++)
		{
			if(grid[col][row])
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

