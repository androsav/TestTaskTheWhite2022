using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("Испорченный злоумышленниками текст:");
			WebRequest dataRequest = WebRequest.Create("https://raw.githubusercontent.com/thewhitesoft/student-2022-assignment/main/data.json");
			WebResponse dataResponse = dataRequest.GetResponse();
			List<string> jsonData;
			using (Stream stream = dataResponse.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					jsonData = JsonConvert.DeserializeObject<List<string>>(reader.ReadToEnd());

					foreach (var s in jsonData)
						Console.WriteLine(s);
				}
			}
			Console.WriteLine("================================================================================================");
			Console.WriteLine("Замены, используемые для восстановления исходного текста:");
			List<JsonData> jsonReplacement;
			WebRequest replacementRequest = WebRequest.Create("https://raw.githubusercontent.com/thewhitesoft/student-2022-assignment/main/replacement.json");
			WebResponse replacementResponse = replacementRequest.GetResponse();
			using (Stream stream = replacementResponse.GetResponseStream())
			{
				using (StreamReader reader = new StreamReader(stream))
				{
					jsonReplacement = JsonConvert.DeserializeObject<List<JsonData>>(reader.ReadToEnd());

					jsonReplacement.Sort();

					for (int i = 0; i < jsonReplacement.Count;)
					{
						if (jsonReplacement[i].Replacement == "Removed")
						{
							jsonReplacement.Remove(jsonReplacement[i]);
						}
						else
						{
							Console.WriteLine("Replacement: " + jsonReplacement[i].Replacement + " Source: " + jsonReplacement[i].Source);
							i++;
						}	
					}
				}
			}


			Console.WriteLine("================================================================================================");
			Console.WriteLine("Пошаговое восстановление исходного текста:");
			for (int i = 0; i< jsonData.Count; i++)
			{
				foreach (JsonData json in jsonReplacement)
					if (jsonData[i].Contains(json.Replacement) && (json.Source != null))
					{
						Console.WriteLine($"Замена {json.Replacement} на {json.Source}");
						Console.WriteLine(jsonData[i].Replace(json.Replacement, json.Source));
						jsonData[i] = jsonData[i].Replace(json.Replacement, json.Source);
					}
					else if (jsonData[i].Contains(json.Replacement) && (json.Source == null))
					{
						jsonData[i] = null;
						break;
					}
					else if (jsonData[i].Contains("Robert Frost poet"))
					{
						jsonData[i] = jsonData[i].Replace("Robert Frost poet", "");
					}
			}

			while (jsonData.Contains(null))
				jsonData.Remove(null);
			jsonData.Add("Robert Frost poet");

			Console.WriteLine("================================================================================================");
			Console.WriteLine("Исходный текст:");
			foreach (var s in jsonData)
				Console.WriteLine(s);

			JsonData jd = new JsonData();
			string ser = JsonConvert.SerializeObject(jsonData);

			File.WriteAllText("result.json", ser);

			Console.ReadLine();
		}
	}
}
