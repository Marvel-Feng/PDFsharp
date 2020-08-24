using System;
using System.Collections.Generic;
using System.Text;

using PdfSharp.Drawing;
using PdfSharp.Drawing.BarCodes;

namespace PdfSharp.Drawing.BarCodes
{
/// <summary>A Class to be able to render a Code 128 bar code</summary>
/// <remarks>For a more detailed explanation of the Code 128, please visit the following
/// web site: http://www.barcodeman.com/info/c128.php3
/// or
/// http://www.adams1.com/128code.html
/// </remarks>
public class Code128 : BarCode
{
/// <summary>A static place holder for the patterns to draw the code 128 barcode</summary>
public static Dictionary<Byte, Byte[]> Patterns = null;
private Code_128_Code_Types Code128Code = Code_128_Code_Types.CodeB;
private Byte[] Values = null;
private const int CODE128_STOPCODE = 106;

/// <summary>Constructor</summary>
/// <param name="text">String - The text to be coded</param>
/// <param name="size">XSize - The size of the bar code</param>
/// <param name="direction">CodeDirection - Indicates the direction to draw the bar code</param>
public Code128(string text, XSize size, CodeDirection direction)
: this(text, size, direction, Code_128_Code_Types.CodeB)
{
}

/// <summary>Constructor</summary>
/// <param name="text">String - The text to be coded</param>
/// <param name="size">XSize - The size of the bar code</param>
/// <param name="direction">CodeDirection - Indicates the direction to draw the bar code</param>
/// <param name="code128Code">Code_128_Code_Types - Indicates which of the codes to use when rendering the bar code.
/// The options are A, B, or buffer.</param>
public Code128(string text, XSize size, CodeDirection direction, Code_128_Code_Types code128Code)
: this((byte[])null, size, direction, code128Code)
{
this.Text = text;


// Create the array to hold the values to be rendered
if (Code128Code == Code_128_Code_Types.CodeC)
{
// Ensure that the text is an even length
if ((text.Length % 2) == 1) throw new ArgumentOutOfRangeException("Parameter text (string) must have an even length for Code 128 - Code C");
Values = new byte[text.Length / 2];
}
else
{
Values = new byte[text.Length];
}


String buffer = "";
for (int x = 0; x < text.Length; x++)
{
switch (Code128Code)
{
case Code_128_Code_Types.CodeA:
if (text[x] < 32)
{
Values[x] = (byte)(text[x] + 64);
}
else if ((text[x] >= 32) && (text[x] < 64))
{
Values[x] = (byte)(text[x] - 32);
}
else
{
Values[x] = (byte)text[x];
}
break;

case Code_128_Code_Types.CodeB:
Values[x] = (byte)(text[x] - 32);
break;

case Code_128_Code_Types.CodeC:
if ((text[x] >= '0') && (text[x] <= '9'))
{
buffer += text[x];
if (buffer.Length == 2)
{
Values[x / 2] = byte.Parse(buffer);
buffer = "";
}
}
else
{
throw new ArgumentOutOfRangeException("Parameter text (string) can only contain numeric characters for Code 128 - Code C");
}
break;
}
}
CheckValues();
}


private void CheckValues()
{
if (Values == null) throw new InvalidOperationException("Text or Values must be set");
if (Values.Length == 0) throw new InvalidOperationException("Text or Values must have content");

for (int x = 0; x < Values.Length; x++)
{
if (Values[x] > 102) throw new ArgumentOutOfRangeException(BcgSR.InvalidCode128(x));
}
}

/// <summary>Constructor</summary>
/// <param name="values">byte[] - The values to be rendered</param>
/// <param name="size">XSize - The size of the bar code</param>
/// <param name="direction">CodeDirection - Indicates the direction to draw the bar code</param>
/// <param name="code128Code">Code_128_Code_Types - Indicates which of the codes to use when rendering the bar code.
/// The options are A, B, or buffer.</param>
public Code128(byte[] values, XSize size, CodeDirection direction, Code_128_Code_Types code128Code)
: base(null, size, direction)
{
if (!Enum.IsDefined(typeof(Code_128_Code_Types), code128Code)) throw new ArgumentOutOfRangeException("Parameter code128Code (Code_128_Code_Types) is invalid");
if (Patterns == null) Load();
Code128Code = code128Code;
Values = values;
}

/// <summary>Creates a new instance of the Patterns field and populates it with the appropriate
/// pattern to draw a code 128 bar code</summary>
private void Load()
{
Patterns = new Dictionary<byte, byte[]>();
Patterns.Add(0, new Byte[] { 2, 1, 2, 2, 2, 2 });
Patterns.Add(1, new Byte[] { 2, 2, 2, 1, 2, 2 });
Patterns.Add(2, new Byte[] { 2, 2, 2, 2, 2, 1 });
Patterns.Add(3, new Byte[] { 1, 2, 1, 2, 2, 3 });
Patterns.Add(4, new Byte[] { 1, 2, 1, 3, 2, 2 });
Patterns.Add(5, new Byte[] { 1, 3, 1, 2, 2, 2 });
Patterns.Add(6, new Byte[] { 1, 2, 2, 2, 1, 3 });
Patterns.Add(7, new Byte[] { 1, 2, 2, 3, 1, 2 });
Patterns.Add(8, new Byte[] { 1, 3, 2, 2, 1, 2 });
Patterns.Add(9, new Byte[] { 2, 2, 1, 2, 1, 3 });
Patterns.Add(10, new Byte[] { 2, 2, 1, 3, 1, 2 });
Patterns.Add(11, new Byte[] { 2, 3, 1, 2, 1, 2 });
Patterns.Add(12, new Byte[] { 1, 1, 2, 2, 3, 2 });
Patterns.Add(13, new Byte[] { 1, 2, 2, 1, 3, 2 });
Patterns.Add(14, new Byte[] { 1, 2, 2, 2, 3, 1 });
Patterns.Add(15, new Byte[] { 1, 1, 3, 2, 2, 2 });
Patterns.Add(16, new Byte[] { 1, 2, 3, 1, 2, 2 });
Patterns.Add(17, new Byte[] { 1, 2, 3, 2, 2, 1 });
Patterns.Add(18, new Byte[] { 2, 2, 3, 2, 1, 1 });
Patterns.Add(19, new Byte[] { 2, 2, 1, 1, 3, 2 });
Patterns.Add(20, new Byte[] { 2, 2, 1, 2, 3, 1 });
Patterns.Add(21, new Byte[] { 2, 1, 3, 2, 1, 2 });
Patterns.Add(22, new Byte[] { 2, 2, 3, 1, 1, 2 });
Patterns.Add(23, new Byte[] { 3, 1, 2, 1, 3, 1 });
Patterns.Add(24, new Byte[] { 3, 1, 1, 2, 2, 2 });
Patterns.Add(25, new Byte[] { 3, 2, 1, 1, 2, 2 });
Patterns.Add(26, new Byte[] { 3, 2, 1, 2, 2, 1 });
Patterns.Add(27, new Byte[] { 3, 1, 2, 2, 1, 2 });
Patterns.Add(28, new Byte[] { 3, 2, 2, 1, 1, 2 });
Patterns.Add(29, new Byte[] { 3, 2, 2, 2, 1, 1 });
Patterns.Add(30, new Byte[] { 2, 1, 2, 1, 2, 3 });
Patterns.Add(31, new Byte[] { 2, 1, 2, 3, 2, 1 });
Patterns.Add(32, new Byte[] { 2, 3, 2, 1, 2, 1 });
Patterns.Add(33, new Byte[] { 1, 1, 1, 3, 2, 3 });
Patterns.Add(34, new Byte[] { 1, 3, 1, 1, 2, 3 });
Patterns.Add(35, new Byte[] { 1, 3, 1, 3, 2, 1 });
Patterns.Add(36, new Byte[] { 1, 1, 2, 3, 1, 3 });
Patterns.Add(37, new Byte[] { 1, 3, 2, 1, 1, 3 });
Patterns.Add(38, new Byte[] { 1, 3, 2, 3, 1, 1 });
Patterns.Add(39, new Byte[] { 2, 1, 1, 3, 1, 3 });
Patterns.Add(40, new Byte[] { 2, 3, 1, 1, 1, 3 });
Patterns.Add(41, new Byte[] { 2, 3, 1, 3, 1, 1 });
Patterns.Add(42, new Byte[] { 1, 1, 2, 1, 3, 3 });
Patterns.Add(43, new Byte[] { 1, 1, 2, 3, 3, 1 });
Patterns.Add(44, new Byte[] { 1, 3, 2, 1, 3, 1 });
Patterns.Add(45, new Byte[] { 1, 1, 3, 1, 2, 3 });
Patterns.Add(46, new Byte[] { 1, 1, 3, 3, 2, 1 });
Patterns.Add(47, new Byte[] { 1, 3, 3, 1, 2, 1 });
Patterns.Add(48, new Byte[] { 3, 1, 3, 1, 2, 1 });
Patterns.Add(49, new Byte[] { 2, 1, 1, 3, 3, 1 });
Patterns.Add(50, new Byte[] { 2, 3, 1, 1, 3, 1 });
Patterns.Add(51, new Byte[] { 2, 1, 3, 1, 1, 3 });
Patterns.Add(52, new Byte[] { 2, 1, 3, 3, 1, 1 });
Patterns.Add(53, new Byte[] { 2, 1, 3, 1, 3, 1 });
Patterns.Add(54, new Byte[] { 3, 1, 1, 1, 2, 3 });
Patterns.Add(55, new Byte[] { 3, 1, 1, 3, 2, 1 });
Patterns.Add(56, new Byte[] { 3, 3, 1, 1, 2, 1 });
Patterns.Add(57, new Byte[] { 3, 1, 2, 1, 1, 3 });
Patterns.Add(58, new Byte[] { 3, 1, 2, 3, 1, 1 });
Patterns.Add(59, new Byte[] { 3, 3, 2, 1, 1, 1 });
Patterns.Add(60, new Byte[] { 3, 1, 4, 1, 1, 1 });
Patterns.Add(61, new Byte[] { 2, 2, 1, 4, 1, 1 });
Patterns.Add(62, new Byte[] { 4, 3, 1, 1, 1, 1 });
Patterns.Add(63, new Byte[] { 1, 1, 1, 2, 2, 4 });
Patterns.Add(64, new Byte[] { 1, 1, 1, 4, 2, 2 });
Patterns.Add(65, new Byte[] { 1, 2, 1, 1, 2, 4 });
Patterns.Add(66, new Byte[] { 1, 2, 1, 4, 2, 1 });
Patterns.Add(67, new Byte[] { 1, 4, 1, 1, 2, 2 });
Patterns.Add(68, new Byte[] { 1, 4, 1, 2, 2, 1 });
Patterns.Add(69, new Byte[] { 1, 1, 2, 2, 1, 4 });
Patterns.Add(70, new Byte[] { 1, 1, 2, 4, 1, 2 });
Patterns.Add(71, new Byte[] { 1, 2, 2, 1, 1, 4 });
Patterns.Add(72, new Byte[] { 1, 2, 2, 4, 1, 1 });
Patterns.Add(73, new Byte[] { 1, 4, 2, 1, 1, 2 });
Patterns.Add(74, new Byte[] { 1, 4, 2, 2, 1, 1 });
Patterns.Add(75, new Byte[] { 2, 4, 1, 2, 1, 1 });
Patterns.Add(76, new Byte[] { 2, 2, 1, 1, 1, 4 });
Patterns.Add(77, new Byte[] { 4, 1, 3, 1, 1, 1 });
Patterns.Add(78, new Byte[] { 2, 4, 1, 1, 1, 2 });
Patterns.Add(79, new Byte[] { 1, 3, 4, 1, 1, 1 });
Patterns.Add(80, new Byte[] { 1, 1, 1, 2, 4, 2 });
Patterns.Add(81, new Byte[] { 1, 2, 1, 1, 4, 2 });
Patterns.Add(82, new Byte[] { 1, 2, 1, 2, 4, 1 });
Patterns.Add(83, new Byte[] { 1, 1, 4, 2, 1, 2 });
Patterns.Add(84, new Byte[] { 1, 2, 4, 1, 1, 2 });
Patterns.Add(85, new Byte[] { 1, 2, 4, 2, 1, 1 });
Patterns.Add(86, new Byte[] { 4, 1, 1, 2, 1, 2 });
Patterns.Add(87, new Byte[] { 4, 2, 1, 1, 1, 2 });
Patterns.Add(88, new Byte[] { 4, 2, 1, 2, 1, 1 });
Patterns.Add(89, new Byte[] { 2, 1, 2, 1, 4, 1 });
Patterns.Add(90, new Byte[] { 2, 1, 4, 1, 2, 1 });
Patterns.Add(91, new Byte[] { 4, 1, 2, 1, 2, 1 });
Patterns.Add(92, new Byte[] { 1, 1, 1, 1, 4, 3 });
Patterns.Add(93, new Byte[] { 1, 1, 1, 3, 4, 1 });
Patterns.Add(94, new Byte[] { 1, 3, 1, 1, 4, 1 });
Patterns.Add(95, new Byte[] { 1, 1, 4, 1, 1, 3 });
Patterns.Add(96, new Byte[] { 1, 1, 4, 3, 1, 1 });
Patterns.Add(97, new Byte[] { 4, 1, 1, 1, 1, 3 });
Patterns.Add(98, new Byte[] { 4, 1, 1, 3, 1, 1 });
Patterns.Add(99, new Byte[] { 1, 1, 3, 1, 4, 1 });
Patterns.Add(100, new Byte[] { 1, 1, 4, 1, 3, 1 });
Patterns.Add(101, new Byte[] { 3, 1, 1, 1, 4, 1 });
Patterns.Add(102, new Byte[] { 4, 1, 1, 1, 3, 1 });
Patterns.Add(103, new Byte[] { 2, 1, 1, 4, 1, 2 });
Patterns.Add(104, new Byte[] { 2, 1, 1, 2, 1, 4 });
Patterns.Add(105, new Byte[] { 2, 1, 1, 2, 3, 2 });
Patterns.Add(106, new Byte[] { 2, 3, 3, 1, 1, 1, 2 });
}

/// <summary>Validates the text string to be coded</summary>
/// <param name="text">String - The text string to be coded</param>
protected override void CheckCode(string text)
{
if (text == null) throw new ArgumentNullException("Parameter text (string) can not be null");
if (text.Length == 0) throw new ArgumentException("Parameter text (string) can not be empty");
}

/// <summary>Renders the content found in Text</summary>
/// <param name="gfx">XGraphics - Instance of the drawing surface </param>
/// <param name="brush">XBrush - Line and Color to draw the bar code</param>
/// <param name="font">XFont - Font to use to draw the text string</param>
/// <param name="position">XPoint - Location to render the bar code</param>
protected internal override void Render(XGraphics gfx, XBrush brush, XFont font, XPoint position)
{
XGraphicsState state = gfx.Save();

BarCodeRenderInfo info = new BarCodeRenderInfo(gfx, brush, font, position);
InitRendering(info);
info.CurrPosInString = 0;
info.CurrPos = position - CodeBase.CalcDistance(AnchorType.TopLeft, this.anchor, this.size);

RenderStart(info);
foreach (byte c in Values)
{
RenderValue(info, (int)c);
}
RenderStop(info);
if (TextLocation != TextLocation.None) RenderText(info);

gfx.Restore(state);
}

private void RenderStart(BarCodeRenderInfo info)
{
RenderValue(info, (int)Code128Code);
}

private void RenderStop(BarCodeRenderInfo info)
{
RenderValue(info, CalculateParity());
RenderValue(info, CODE128_STOPCODE);
}

private void RenderValue(BarCodeRenderInfo info, int chVal)
{
byte[] pattern = GetPattern(chVal);
XBrush space = XBrushes.White;
for (int idx = 0; idx < pattern.Length; idx++)
{
if ((idx % 2) == 0)
{
RenderBar(info, info.ThinBarWidth * pattern[idx]);
}
else
{
RenderBar(info, info.ThinBarWidth * pattern[idx], space);
}
}
}

private void RenderText(BarCodeRenderInfo info)
{
if (info.Font == null) info.Font = new XFont("Courier New", Size.Height / 6);
XPoint center = info.Position + CodeBase.CalcDistance(this.anchor, AnchorType.TopLeft, this.size);
if (TextLocation == TextLocation.Above)
{
info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, Size), XStringFormat.TopCenter);
}
else if (TextLocation == TextLocation.AboveEmbedded)
{
XSize textSize = info.Gfx.MeasureString(this.text, info.Font);
textSize.Width += this.Size.Width * .15;
XPoint point = info.Position;
point.X += (this.Size.Width - textSize.Width) / 2;
XRect rect = new XRect(point, textSize);
info.Gfx.DrawRectangle(XBrushes.White, rect);
info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, Size), XStringFormat.TopCenter);
}
else if (TextLocation == TextLocation.Below)
{
info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, Size), XStringFormat.BottomCenter);
}
else if (TextLocation == TextLocation.BelowEmbedded)
{
XSize textSize = info.Gfx.MeasureString(this.text, info.Font);
textSize.Width += this.Size.Width * .15;
XPoint point = info.Position;
point.X += (this.Size.Width - textSize.Width) / 2;
point.Y += Size.Height - textSize.height;
XRect rect = new XRect(point, textSize);
info.Gfx.DrawRectangle(XBrushes.White, rect);
info.Gfx.DrawString(this.text, info.Font, info.Brush, new XRect(center, Size), XStringFormat.BottomCenter);
}
}

private byte[] GetPattern(int codeValue)
{
if (codeValue < 0) throw new ArgumentOutOfRangeException("Parameter ch (int) can not be less than 32 (space).");
if (codeValue > 106) throw new ArgumentOutOfRangeException("Parameter ch (int) can not be greater than 138.");
return Patterns[(byte)codeValue];
}

private int CalculateParity()
{
long parityValue = (int)Code128Code;
for (int x = 1; x <= Values.Length; x++)
{
parityValue += ((Values[x - 1]) * x);
}
parityValue %= 103;
return (int)parityValue;
}

/// <summary>Renders a single line of the character. Each character has three lines and three spaces</summary>
/// <param name="info"></param>
/// <param name="barWidth">Indicates the thickness of the line/bar to be rendered.</param>
internal void RenderBar(BarCodeRenderInfo info, double barWidth)
{
RenderBar(info, barWidth, info.Brush);
}

/// <summary>Renders a single line of the character. Each character has three lines and three spaces</summary>
/// <param name="info"></param>
/// <param name="barWidth">Indicates the thickness of the line/bar to be rendered.</param>
/// <param name="brush">Indicates the brush to use to render the line/bar.</param>
private void RenderBar(BarCodeRenderInfo info, double barWidth, XBrush brush)
{
double height = Size.Height;
double yPos = info.CurrPos.Y;

switch (TextLocation)
{
case TextLocation.Above:
yPos = info.CurrPos.Y + (height / 5);
height *= 4.0 / 5;
break;
case TextLocation.Below:
height *= 4.0 / 5;
break;
case TextLocation.AboveEmbedded:
case TextLocation.BelowEmbedded:
case TextLocation.None:
break;
}
XRect rect = new XRect(info.CurrPos.X, yPos, barWidth, height);
info.Gfx.DrawRectangle(brush, rect);
info.CurrPos.X += barWidth;
}

internal override void InitRendering(BarCodeRenderInfo info)
{
if (Values == null) throw new InvalidOperationException(BcgSR.BarCodeNotSet);
if (Values.Length == 0) throw new InvalidOperationException(BcgSR.EmptyBarCodeSize);

int numberOfBars = Values.Length + 3; // The length of the string plus the start, stop, and parity value
numberOfBars *= 11; // Each character has 11 bars
numberOfBars += 2; // Add two more because the stop bit has two extra bars

// Calculating the width of a bar
info.ThinBarWidth = ((double)this.Size.Width / (double)numberOfBars);
}
}


/// <summary>Code types for Code 128 bar code</summary>
public enum Code_128_Code_Types
{
/// <summary>Code A</summary>
CodeA = 103,
/// <summary>Code B</summary>
CodeB = 104,
/// <summary>Code buffer</summary>
CodeC = 105,
}
}
