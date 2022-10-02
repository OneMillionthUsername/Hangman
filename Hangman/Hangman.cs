using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace DOS
{
	public class Hangman
	{
		public Hangman()
		{
			#region INITIALISIERUNG
			Random rnd = new Random();
			Stopwatch clock = new Stopwatch();

			long score = 0, ranking1 = 0, ranking2 = 0;
			int versuche = 0, index = 0, turn = 1, durchläufe = 1, rounds = 0, runden = 1, platz = 1;
			bool repeat = false;
			string spieler1 = "", spieler2 = "", spieler = "", win = "CONGRATULATIONS!", score1 = "", score2 = "", punkte = "";
			string scoreString1 = "", scoreString2 = "";
			string path = @"C:\Users\deanm\OneDrive\VisualStudio2017\Hangman\Hangman\Rankings.txt";
			if (!File.Exists(path))
			{
				File.Create(path);
			}

			string content = File.ReadAllText(path);
			Console.Write("Name Spieler eins: ");
			spieler1 = Console.ReadLine();
			while (spieler1.Length > 12 || spieler1.Length <= 0 || !spieler1.All(char.IsLetter))
			{
				Console.WriteLine("Ungültiger Name.");
				Console.Write("Name Spieler eins: ");
				spieler1 = Console.ReadLine();
			}
			Console.Write("Name Spieler zwei: ");
			spieler2 = Console.ReadLine();
			while (spieler2.Length > 12 || spieler2.Length <= 0 || !spieler2.All(char.IsLetter))
			{
				Console.WriteLine("Ungültiger Name.");
				Console.Write("Name Spieler zwei: ");
				spieler2 = Console.ReadLine();
			}
			//Reihenfolge ausloten
			turn = rnd.Next(1, 2 + 1);
			#endregion
			#region EVALUATION
			do
			{
				Console.Clear();
				if (turn == 1)
				{
					spieler = spieler1;
					turn++;
				}
				else if (turn == 2)
				{
					spieler = spieler2;
					turn--;
				}

				Console.BackgroundColor = ConsoleColor.Blue;
				Console.ForegroundColor = ConsoleColor.White;
				if (rounds >= 6)
				{
					Console.Clear();
					Console.CursorVisible = false;
					Console.SetCursorPosition(50, 0);
					Console.WriteLine(win);
					score1 = ranking1.ToString();
					score2 = ranking2.ToString();

					while (score1.Length > score2.Length)
						score2 = "." + score2;

					while (score2.Length > score1.Length)
						score1 = "." + score1;


					if (ranking1 > ranking2)
					{
						punkte = "";
						for (int i = 0; i < 12 - spieler1.Length + score1.Length; i++)
						{
							punkte += ".";
						}

						platz = 1;
						scoreString1 = $"{platz}.{spieler1.ToUpper()}{punkte}{score1} Punkten";
						Console.SetCursorPosition(50, 2);
						Console.WriteLine(scoreString1);

						//else if (content.ToLower().Contains(spieler1.ToLower()))
						//{
						//	spielerIndex = content.IndexOf('d');
						//	content = content.Remove(spielerIndex) + platz + scoreString1 + content.Substring(scoreString1.Length + 1);
						//	File.WriteAllText(path, content);
						//}

						punkte = "";
						for (int i = 0; i < 12 - spieler2.Length + score2.Length; i++)
						{
							punkte += ".";
						}

						platz = 2;
						scoreString2 = $"{platz}.{spieler2.ToUpper()}{punkte}{score2} Punkten";
						Console.SetCursorPosition(50, 3);
						Console.WriteLine(scoreString2);


						//else if (content.ToLower().Contains(spieler2.ToLower()))
						//{
						//	spielerIndex = content.IndexOf(spieler2);
						//	content = content.Remove(spielerIndex) + platz + scoreString2 + content.Substring(scoreString2.Length + 1);
						//	File.WriteAllText(path, content);
						//}
						File.WriteAllText(path, $"{content}\n{scoreString1}\n{scoreString2}");
						break;
					}
					else if (ranking2 > ranking1)
					{
						punkte = "";
						for (int i = 0; i < 12 - spieler2.Length + score2.Length; i++)
						{
							punkte += ".";
						}

						platz = 1;
						scoreString2 = $"{platz}.{spieler2.ToUpper()}{punkte}{score2} Punkten";
						Console.SetCursorPosition(50, 2);
						Console.WriteLine(scoreString2);

						punkte = "";
						for (int i = 0; i < 12 - spieler1.Length + score1.Length; i++)
						{
							punkte += ".";
						}

						platz = 2;
						scoreString1 = $"{platz}.{spieler1.ToUpper()}{punkte}{score1} Punkten";
						Console.SetCursorPosition(50, 3);
						Console.WriteLine(scoreString1);
						File.WriteAllText(path, $"{content}\n{scoreString1}\n{scoreString2}");

						break;
					}
					else
					{
						Console.WriteLine("Sudden death!".ToUpper());
						rounds--;
					}
				}
				if (ranking1 > ranking2 || ranking1 == ranking2)
				{
					if (ranking2 == ranking1)
					{
						Console.SetCursorPosition(Console.CursorLeft + 50, 2);
						Console.WriteLine("Gleichstand!");
					}
					Console.SetCursorPosition(Console.CursorLeft + 50, 0);
					Console.WriteLine($"Spieler 1: {spieler1} mit {ranking1} Punkten.");
					Console.SetCursorPosition(Console.CursorLeft + 50, 1);
					Console.WriteLine($"Spieler 2: {spieler2} mit {ranking2} Punkten.");
				}
				else if (ranking2 > ranking1)
				{
					Console.SetCursorPosition(Console.CursorLeft + 50, 0);
					Console.WriteLine($"Spieler 1: {spieler2} mit {ranking2} Punkten.");
					Console.SetCursorPosition(Console.CursorLeft + 50, 1);
					Console.WriteLine($"Spieler 2: {spieler1} mit {ranking1} Punkten.");
				}
				Console.ResetColor();
				#endregion

				#region SPIELMECHANIK
				Console.SetCursorPosition(0, 0);
				clock.Restart();
				if (rounds > 1 && rounds % 2 == 0)
				{
					runden++;
				}
				Console.WriteLine($"ROUND {runden}.");
				Console.WriteLine($"{spieler} ist am Zug.");
				bool lösung = false;
				string geheimWort = "", eingabe = "", charSpeicher = "", leerWort = "", temp = "";

				var geheimwörter = (Geheimwörter)rnd.Next(0, 85 + 1);
				geheimWort = geheimwörter.ToString().ToLower();
				Console.WriteLine($"Das Geheimwort hat {geheimWort.Length} Buchstaben.");
				versuche = geheimWort.Length - 1;
				Console.WriteLine($"Du hast {versuche} Versuche. Viel Spass!");
				for (int i = 0; i < geheimWort.Length; i++)
				{
					leerWort += "_";
				}

				do
				{
					Console.SetCursorPosition(0, 4);
					Console.WriteLine(leerWort);
					eingabe = Console.ReadLine().ToLower();
					if (eingabe == "cheat")
					{
						eingabe = geheimWort;
					}
					Console.SetCursorPosition(0, 5);
					Console.WriteLine(new String(' ', Console.BufferWidth));
					if (!charSpeicher.Contains(eingabe.ToUpper()) && eingabe.Length == 1 && eingabe.ToUpper().All(char.IsLetter))
					{
						charSpeicher += eingabe.ToUpper();
						charSpeicher += " ";
						Console.SetCursorPosition(50, 4);
						Console.WriteLine("Verwendete Buchstaben: " + charSpeicher);
						Console.SetCursorPosition(0, 6);
					}
					else if (eingabe.Length == 1 && eingabe.All(char.IsLetter))
					{
						Console.WriteLine("Doppelte Eingabe!");
						continue;
					}
					else if (eingabe != geheimWort && eingabe.Length != geheimWort.Length)
					{
						Console.WriteLine("Ungültige Eingabe!");
						continue;
					}

					if (eingabe.ToLower() == geheimWort)
					{
						Console.SetCursorPosition(0, 5);
						Console.WriteLine("RICHTIG!");
						Console.WriteLine($"Das Wort wurde nach {geheimWort.Length - versuche} Versuchen und {clock.ElapsedMilliseconds / 1000} Sekunden gelöst.");
						score = (12500 - clock.ElapsedMilliseconds / 1000 * 110) / (geheimWort.Length - versuche);
						clock.Stop();
						Console.WriteLine($"Das Geheimwort war {geheimWort.ToUpper()}.");
						Console.WriteLine($"{spieler} bekommt " + score + " Punkte.");
						lösung = true;
					}
					//else if (eingabe != geheimWort && eingabe.Length == geheimWort.Length)
					//{
					//	Console.SetCursorPosition(0, 5);
					//	Console.WriteLine("Leider falsch geraten. Du verliest zwei Versuche.");
					//	Console.WriteLine($"Noch {versuche} Versuche.");
					//	versuche -= 2;
					//	if (versuche <= 0)
					//	{
					//		Console.WriteLine($"Keine Versuche mehr. Verloren. Das Wort war {geheimWort.ToUpper()}.");
					//		score = 0;
					//		break;
					//	}

					//	continue;
					//}
					else if (eingabe != geheimWort && eingabe.Length > 1 || eingabe.Length <= 0)
					{
						Console.WriteLine("Eingabe falsch.");
					}
					else if (eingabe.Length == 1 && geheimWort.Contains(eingabe))
					{
						temp = geheimWort;
						while (geheimWort.Contains(eingabe)) //lustiges nebenfeature = eingabe(f) = wort(ffffff)
						{
							index = geheimWort.IndexOf(eingabe);
							leerWort = leerWort.Remove(index) + geheimWort.Substring(index, 1) + leerWort.Substring(index + 1);
							geheimWort = geheimWort.Remove(index) + '_' + geheimWort.Substring(index + 1);
						}
						Console.SetCursorPosition(0, 4);
						Console.WriteLine(leerWort);
						geheimWort = temp;

						if (geheimWort == leerWort)
						{
							Console.WriteLine($"Das Wort wurde nach {geheimWort.Length - versuche} Versuchen und {clock.ElapsedMilliseconds / 1000} Sekunden gelöst.");
							score = (10000 - clock.ElapsedMilliseconds / 1000 * 110) / (geheimWort.Length - versuche);
							clock.Stop();
							Console.WriteLine($"Das Geheimwort war {geheimWort.ToUpper()}.");
							Console.WriteLine($"{spieler} bekommt " + score + " Punkte.");
							lösung = true;
						}
					}
					else
					{
						versuche--;
						if (versuche <= 0)
						{
							Console.WriteLine($"Keine Versuche mehr. Verloren! Das Wort war {geheimWort.ToUpper()}.");
							break;
						}
						Console.WriteLine($"Noch {versuche} Versuche.");
					}
					durchläufe++;
				} while (!lösung && versuche > 0);
				#endregion
				#region ENDGAME
				if (spieler == spieler1)
				{
					ranking1 += score;
					score = 0;
				}
				else if (spieler == spieler2)
				{
					ranking2 += score;
					score = 0;
				}

				Console.WriteLine("Nächster Spieler!");
				if (ConsoleKey.E == Console.ReadKey().Key)
				{
					break;
				}
				Console.Clear();
				durchläufe = 1;
				rounds++;
			} while (!repeat);
			Console.ReadKey();
			#endregion
		}
		string Scorebaord(string path, string score1, string score2)
		{
			string content = File.ReadAllText(path);
			string result = "";
			return result;
		}

		class Spieler
		{

		}
	}
	//PSSSST!!!!!
	enum Geheimwörter
	{
		Kanonenfutter,
		Quantenverschraenkung,
		Desoxyribonukleinsaeure,
		Dinosaurierei,
		Apfelstrudel,
		Hippopotamus,
		Kaiserpinguin,
		Dachgeschoss,
		Dopplereffekt,
		Sonnenfinsternis,
		Kungfu,
		Bananenrepublik,
		Ananas,
		Kajutenfenster,
		Autobahn,
		Lokomotive,
		Schokolade,
		Bienenhonig,
		Klosterneuburg,
		Zimtstern,
		Eishockey,
		Ausserirdischer,
		Schreibtisch,
		Schneeflocke,
		Atomreaktor,
		Sonnensystem,
		Elektrotechnik,
		Zirkuszelt,
		Radiergummi,
		Abrechnung,
		Bruder,
		Schwester,
		Lehrer,
		Schueler,
		Orangutan,
		Marmelade,
		Marzipan,
		Nilpferd,
		Nikotin,
		Alkohol,
		Krankenhaus,
		Fussball,
		Bueroklammer,
		Waeschekorb,
		Krokodil,
		Schwimmbad,
		Galionsfigur,
		Kernspintomographie,
		Zucchini,
		Gymnastik,
		Rhythmus,
		Metapher,
		Pankreas,
		Aluminium,
		Pandemie,
		Epidemie,
		Internet,
		Computer,
		Meerjungfrau,
		Saeugetier,
		Vulkan,
		Flugzeug,
		Uboot,
		Skateboard,
		Universum,
		Galaxie,
		Milchstrasse,
		Hangman,
		Programm,
		Nachrichten,
		Sommerzeit,
		Winternacht,
		Polarkreis,
		Aequator,
		Atlantik,
		Buddhismus,
		Zeremonie,
		Verbrecher,
		Schwertfisch,
		Hammerhai,
		Phantasie,
		Monarchie,
		Anarchie,
		Demokratie,
		Diktatur,
		Metastase
	}
}