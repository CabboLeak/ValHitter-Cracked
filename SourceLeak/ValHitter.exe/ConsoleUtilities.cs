using System;
using System.Drawing;
using System.Threading;
using Colorful;

// Token: 0x02000007 RID: 7
internal class ConsoleUtilities
{
	// Token: 0x060000B8 RID: 184 RVA: 0x0000AD98 File Offset: 0x00008F98
	public static void Write(string message, MsgType _type)
	{
		Colorful.Console.Write("  [", Color.Silver);
		switch (_type)
		{
		case MsgType.INPUT:
			Colorful.Console.Write("INPUT", Color.Purple);
			break;
		case MsgType.OUTPUT:
			Colorful.Console.Write("OUTPUT", Color.Blue);
			break;
		case MsgType.WARNING:
			Colorful.Console.Write("WARNING", Color.Orange);
			break;
		case MsgType.ERROR:
			Colorful.Console.Write("ERROR", Color.DarkRed);
			break;
		}
		Colorful.Console.Write("] ", Color.Silver);
		Colorful.Console.Write(message, Color.White);
	}

	// Token: 0x060000B9 RID: 185 RVA: 0x0000AE28 File Offset: 0x00009028
	public static void WriteStatusLine(string label, string value, Color valueColor)
	{
		Colorful.Console.ResetColor();
		Colorful.Console.Write("                                                  [", Color.Silver);
		Colorful.Console.Write(value, valueColor);
		Colorful.Console.Write("] ", Color.Silver);
		Colorful.Console.Write(label, Color.White);
		Colorful.Console.WriteLine();
		Thread.Sleep(1);
	}
}
