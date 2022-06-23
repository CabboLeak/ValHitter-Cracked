using System;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Colorful;

// Token: 0x02000005 RID: 5
internal static class Utils
{
	// Token: 0x060000AB RID: 171 RVA: 0x0000A8E8 File Offset: 0x00008AE8
	public static string LRParse(string data, string lS, string rS, bool recursive = false, bool useRegexLR = false)
	{
		string input = data;
		bool flag = lS == "" && rS == "";
		string str;
		if (flag)
		{
			str = data;
		}
		else
		{
			bool flag2 = (!input.Contains(lS) && lS != "") || (!input.Contains(rS) && rS != "");
			if (flag2)
			{
				str = "";
			}
			else
			{
				string str2 = null;
				if (recursive)
				{
					if (useRegexLR)
					{
						try
						{
							string pattern = Utils.BuildLRPattern(lS, rS);
							foreach (object obj in Regex.Matches(input, pattern))
							{
								Capture match = (Capture)obj;
								str2 = match.Value;
							}
							return str2;
						}
						catch
						{
							return str2;
						}
					}
					try
					{
						while ((input.Contains(lS) || lS == "") && (input.Contains(rS) || rS == ""))
						{
							int startIndex = ((lS == "") ? 0 : (input.IndexOf(lS) + lS.Length));
							string str3 = input.Substring(startIndex);
							int length = ((rS == "") ? (str3.Length - 1) : str3.IndexOf(rS));
							str2 = str3.Substring(0, length);
							input = str3.Substring(str2.Length + rS.Length);
						}
						return str2;
					}
					catch
					{
						return str2;
					}
				}
				if (useRegexLR)
				{
					string pattern2 = Utils.BuildLRPattern(lS, rS);
					MatchCollection matchCollection = Regex.Matches(input, pattern2);
					bool flag3 = matchCollection.Count > 0;
					if (flag3)
					{
						str2 = matchCollection[0].Value;
					}
				}
				else
				{
					try
					{
						int startIndex2 = ((lS == "") ? 0 : (input.IndexOf(lS) + lS.Length));
						string str4 = input.Substring(startIndex2);
						int length2 = ((rS == "") ? str4.Length : str4.IndexOf(rS));
						str2 = str4.Substring(0, length2);
					}
					catch
					{
					}
				}
				str = str2;
			}
		}
		return str;
	}

	// Token: 0x060000AC RID: 172 RVA: 0x0000AB80 File Offset: 0x00008D80
	public static string BuildLRPattern(string ls, string rs)
	{
		return string.Concat(new string[]
		{
			"(?<=",
			string.IsNullOrEmpty(ls) ? "^" : Regex.Escape(ls),
			").+?(?=",
			string.IsNullOrEmpty(rs) ? "$" : Regex.Escape(rs),
			")"
		});
	}

	// Token: 0x060000AD RID: 173 RVA: 0x0000ABE0 File Offset: 0x00008DE0
	public static void centerText(string text)
	{
		System.Console.WriteLine(string.Format("{0," + (System.Console.WindowWidth / 2 + text.Length / 2).ToString() + "}", text));
	}

	// Token: 0x060000AE RID: 174 RVA: 0x0000AC20 File Offset: 0x00008E20
	public static void center2(string text)
	{
		System.Console.Write(string.Format("{0," + (System.Console.WindowWidth / 7 + text.Length / 1).ToString() + "}", text));
	}

	// Token: 0x060000AF RID: 175 RVA: 0x000028C6 File Offset: 0x00000AC6
	public static string Base64Encode(string plainText)
	{
		return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
	}

	// Token: 0x060000B0 RID: 176 RVA: 0x0000AC60 File Offset: 0x00008E60
	public static int Pourcentage(int current, int maximum)
	{
		int num = 0;
		bool flag = maximum > 0;
		if (flag)
		{
			num = (int)(current / maximum * 100m);
		}
		return num;
	}

	// Token: 0x060000B1 RID: 177 RVA: 0x0000AC98 File Offset: 0x00008E98
	public static DateTime smethod_0(long long_0)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)long_0).ToLocalTime();
	}

	// Token: 0x060000B2 RID: 178 RVA: 0x0000ACC8 File Offset: 0x00008EC8
	public static void centerText(string text, Color color)
	{
		Colorful.Console.WriteLine(string.Format("{0," + (System.Console.WindowWidth / 2 + text.Length / 2).ToString() + "}", text), color);
	}

	// Token: 0x060000B3 RID: 179 RVA: 0x0000AD0C File Offset: 0x00008F0C
	public static void Equal(string text)
	{
		Colorful.Console.Write(string.Format("{0," + (System.Console.WindowWidth / 2 + text.Length / 2).ToString() + "}", text));
	}

	// Token: 0x060000B4 RID: 180 RVA: 0x0000AC98 File Offset: 0x00008E98
	public static DateTime method_0(long long_0)
	{
		return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double)long_0).ToLocalTime();
	}

	// Token: 0x060000B5 RID: 181 RVA: 0x0000AD4C File Offset: 0x00008F4C
	public static string randomString()
	{
		Random random = new Random();
		return new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 12)
			select s[random.Next(s.Length)]).ToArray<char>());
	}
}
