using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Colorful;
using Leaf.xNet;

// Token: 0x02000004 RID: 4
internal class Program
{
	// Token: 0x060000A4 RID: 164 RVA: 0x000028B3 File Offset: 0x00000AB3
	[STAThread]
	private static void Main()
	{
		Program.Menu();
	}

	// Token: 0x060000A5 RID: 165 RVA: 0x0000A088 File Offset: 0x00008288
	public static void Menu()
	{
		File.WriteAllText(Environment.CurrentDirectory + "\\CrackedByCabbo.txt", Program.x);
		System.Console.Title = Program.x;
		StartMenu.Title();
		System.Console.ForegroundColor = ConsoleColor.Cyan;
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine(Program.x);
		System.Console.WriteLine("->Welcome To ValHitter!");
		System.Console.WriteLine("->Where Do You Want To Go?");
		System.Console.WriteLine();
		System.Console.ForegroundColor = ConsoleColor.White;
		System.Console.WriteLine("->[1] Valorant Checker");
		System.Console.WriteLine("->[2] License And Info");
		System.Console.WriteLine("->[3] Discord Server");
		System.Console.WriteLine("->[4] ProxyScrape");
		System.Console.WriteLine("->[5] Quit");
		System.Console.WriteLine();
		System.Console.Write("-> ");
		string text = System.Console.ReadLine();
		if (text == "2")
		{
			System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.WriteLine(Program.x);
		}
		else if (text == "1")
		{
			System.Console.ForegroundColor = ConsoleColor.Green;
			System.Console.WriteLine("Successfully logged in!");
			System.Console.ForegroundColor = ConsoleColor.White;
			Thread.Sleep(200);
			System.Console.Clear();
			System.Console.Title = "Valhiter | Threads";
			StartMenu.Title();
			int num;
			for (;;)
			{
				try
				{
					StartMenu.Title();
					System.Console.WriteLine("Threads: ", MsgType.INPUT);
					num = int.Parse(System.Console.ReadLine());
					if (num > 750)
					{
						num = 750;
					}
				}
				catch
				{
					continue;
				}
				break;
			}
			Checker.Start(Program.LoadCombos(), Program.LoadProxies(false), num);
		}
		else if (text == "5")
		{
			System.Console.ForegroundColor = ConsoleColor.Cyan;
			System.Console.WriteLine("Closing in 3 seconds...");
			Thread.Sleep(3000);
			Process.GetCurrentProcess().Kill();
		}
		else if (text == "4")
		{
			Program.proxyscrape();
		}
		else if (text == "3")
		{
			Process.Start("https://discord.gg/valhitters");
			Thread.Sleep(500);
			Colorful.Console.Clear();
			Program.Menu();
		}
		else
		{
			System.Console.ForegroundColor = ConsoleColor.Red;
			System.Console.WriteLine("Please choose a valid option, loader closing in 3 seconds...");
			Thread.Sleep(3000);
			Process.GetCurrentProcess().Kill();
		}
		System.Console.ReadLine();
	}

	// Token: 0x060000A6 RID: 166 RVA: 0x0000A2D8 File Offset: 0x000084D8
	private static string[] LoadCombos()
	{
		string[] strArray = null;
		using (OpenFileDialog openFileDialog = new OpenFileDialog())
		{
			try
			{
				StartMenu.Title();
				System.Console.Title = "ValHitter | Load Combos";
				ConsoleUtilities.Write(":Load Your Combos:", MsgType.INPUT);
				openFileDialog.Multiselect = false;
				openFileDialog.CheckFileExists = true;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
				openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
				openFileDialog.Title = "ValHitter | Load Combos";
				bool flag = openFileDialog.ShowDialog() == DialogResult.OK;
				if (flag)
				{
					strArray = File.ReadAllLines(openFileDialog.FileName);
				}
			}
			catch (Exception ex)
			{
				ConsoleUtilities.Write("An unexpected error occurred!\n", MsgType.ERROR);
				int num = (int)MessageBox.Show(ex.ToString(), "ValHitter - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				Thread.Sleep(-1);
			}
		}
		return strArray;
	}

	// Token: 0x060000A7 RID: 167 RVA: 0x0000A3D0 File Offset: 0x000085D0
	private static List<string> LoadProxies(bool proxyless = false)
	{
		List<string> stringList = null;
		for (;;)
		{
			try
			{
				StartMenu.Title();
				System.Console.Title = "ValHitter | Load Proxies";
				ConsoleUtilities.Write("Proxy type [HTTP, SOCKS4, SOCKS5]: ", MsgType.INPUT);
				Program.ProxyType = System.Console.ReadLine().ToUpper();
				bool flag = Program.ProxyType == "HTTP" || Program.ProxyType == "SOCKS4" || Program.ProxyType == "SOCKS5" || Program.ProxyType == "NONE";
				if (flag)
				{
					StartMenu.Title();
					System.Console.Title = "ValHitter | Load Proxies";
					ConsoleUtilities.Write("URL or FILE: ", MsgType.INPUT);
					Program.isUrl = System.Console.ReadLine().ToUpper();
					bool flag2 = Program.isUrl == "FILE";
					if (flag2)
					{
						ConsoleUtilities.Write("Load Your Proxies:\n", MsgType.INPUT);
						using (OpenFileDialog openFileDialog = new OpenFileDialog())
						{
							try
							{
								StartMenu.Title();
								openFileDialog.Multiselect = false;
								openFileDialog.CheckFileExists = true;
								openFileDialog.RestoreDirectory = true;
								openFileDialog.InitialDirectory = Environment.SpecialFolder.Desktop.ToString();
								openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
								openFileDialog.Title = "ValHitter | Load Proxies.";
								bool flag3 = openFileDialog.ShowDialog() == DialogResult.OK;
								if (flag3)
								{
									stringList = new List<string>(File.ReadAllLines(openFileDialog.FileName));
									break;
								}
								break;
							}
							catch (Exception ex)
							{
								ConsoleUtilities.Write("An unexpected error occurred!\n", MsgType.ERROR);
								int num = (int)MessageBox.Show(ex.ToString(), "ValHitter - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
								Thread.Sleep(-1);
								break;
							}
						}
					}
					bool flag4 = Program.isUrl == "URL";
					if (flag4)
					{
						ConsoleUtilities.Write("URL: ", MsgType.INPUT);
						Program.url = System.Console.ReadLine();
						try
						{
							using (HttpRequest httpRequest = new HttpRequest())
							{
								httpRequest.UserAgent = Http.ChromeUserAgent();
								stringList = new List<string>(httpRequest.Get(Program.url, null).ToString().Split(Array.Empty<char>()));
								break;
							}
						}
						catch (Exception ex2)
						{
							ConsoleUtilities.Write("An unexpected error occurred!\n", MsgType.ERROR);
							int num2 = (int)MessageBox.Show(ex2.ToString(), "ValHitter - Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
							Thread.Sleep(-1);
							break;
						}
					}
				}
			}
			catch
			{
			}
		}
		return stringList;
	}

	// Token: 0x060000A8 RID: 168 RVA: 0x0000A6A4 File Offset: 0x000088A4
	private static void proxyscrape()
	{
		StartMenu.Title();
		System.Console.Title = "ValHitter | ProxyScrape";
		System.Console.ForegroundColor = ConsoleColor.DarkGreen;
		Colorful.Console.WriteLine();
		Colorful.Console.Write("");
		Colorful.Console.Write("-> What Proxie type would you like to scrape?");
		Colorful.Console.Write(" [HTTP/SOCKS4/SOCKS5]");
		Colorful.Console.WriteLine();
		Colorful.Console.WriteLine();
		Colorful.Console.Write("->");
		string str = Colorful.Console.ReadLine();
		Colorful.Console.WriteLine();
		Colorful.Console.Write("");
		Colorful.Console.Write("-> How Many Timeouts? - ");
		Colorful.Console.Write("this must be between 50 - 10000");
		Colorful.Console.Write("-> ");
		int int32 = Convert.ToInt32(Colorful.Console.ReadLine());
		Colorful.Console.WriteLine();
		Colorful.Console.WriteLine();
		Colorful.Console.Write("-> Scraping... ");
		WebClient webClient = new WebClient();
		DateTime.Now.ToString().Replace(":", "-");
		string str2 = webClient.DownloadString(string.Format("https://api.proxyscrape.com/?request=displayproxies&proxytype={0}&timeout={1}", str, int32));
		Thread.Sleep(500);
		using (StreamWriter streamWriter = File.AppendText(string.Format("{1} Proxies .txt", int32.ToString(), str)))
		{
			streamWriter.WriteLine(str2);
		}
		Colorful.Console.WriteLine();
		Colorful.Console.WriteLine();
		Colorful.Console.Write("-> Finished scraping!");
		Colorful.Console.WriteLine();
		Colorful.Console.Write("-> Select - ");
		Colorful.Console.Write("-> [M] to Menu | [X] to exit | [P] to scrape a different proxies ");
		Colorful.Console.WriteLine();
		Colorful.Console.Write("-> ");
		string upper = Colorful.Console.ReadLine().ToUpper();
		bool flag = upper == "X";
		if (flag)
		{
			Colorful.Console.WriteLine();
			Colorful.Console.WriteLine();
			Colorful.Console.Write("Closing the program - Thanks for using");
			Colorful.Console.WriteLine();
			Thread.Sleep(1000);
			Environment.Exit(0);
		}
		else
		{
			bool flag2 = upper == "P";
			if (flag2)
			{
				Colorful.Console.Clear();
				Program.proxyscrape();
			}
			else
			{
				bool flag3 = !(upper == "M");
				if (!flag3)
				{
					System.Console.WriteLine();
					System.Console.WriteLine();
					System.Console.WriteLine("Closing ProxyScrape - Returning To Menu");
					System.Console.WriteLine();
					Thread.Sleep(1000);
					Program.Menu();
				}
			}
		}
	}

	// Token: 0x0400003F RID: 63
	public static string ProxyType;

	// Token: 0x04000040 RID: 64
	private static string isUrl;

	// Token: 0x04000041 RID: 65
	private static string url;

	// Token: 0x04000042 RID: 66
	public static string x = "Cracked by Cabbo :D | Discord: FreeCabbo10#6558 | Telegram: @cabboshiba | GitHub: https://github.com/CabboShiba - https://github.com/CabboLeak";
}
