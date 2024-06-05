using Generator;
using Helper;
using UserInterface;

var userEmail = UI.AskForEmail();
var identicon = new Identicon(userEmail);

UI.PrintIdenticon(identicon.Grid, identicon.ForegroundColor);
bool saveImage = UI.AskToSaveImage();
if (saveImage) await ImageHelper.SaveImage(identicon.Grid, identicon.ForegroundColor, userEmail);
