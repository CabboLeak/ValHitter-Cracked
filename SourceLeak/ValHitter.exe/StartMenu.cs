using System;
using Colorful;

// Token: 0x02000009 RID: 9
internal class StartMenu
{
	// Token: 0x060000BB RID: 187
	public static void Title()
	{
		Colorful.Console.Clear();
	}

	// Token: 0x04000049 RID: 73
	private static Random rdm = new Random();

	// Token: 0x0400004A RID: 74
	public static int red = StartMenu.rdm.Next(0, 255);

	// Token: 0x0400004B RID: 75
	public static int blue = StartMenu.rdm.Next(0, 255);
}
