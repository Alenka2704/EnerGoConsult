using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnerGoConsult.SourceCode
{
	class FileHandler
	{
		const string header = "[HLAVICKA]",
					 quality = "[KVALITA]",
					 events = "[UDALOSTI]",
					 recorder = "[RECORDER]"; //names of sections

		List<string[]> headerArray = new List<string[]>(); //headers list
		List<List<string>> qualityArray=new List<List<string>>(); // qualiies list
		string text;

		delegate void TextParser(string sectionText); //delegate type for parsing methods

		/// <summary>
		/// Trims unnecessary starting and trailing symbols
		/// </summary>
		/// <param name="text">string to be handled</param>
		/// <returns>trimmed string</returns>
		string TrimText(string text)
		{
			return text.Trim('\r', '\n', '\t', ' ');
		}

		/// <summary>
		/// Getter for HLAVICKA container
		/// </summary>
		/// <returns>key-value pairs from HLAVICKA section</returns>
		public List<string[]> GetHeaderArray()
		{
			return headerArray;
		}

		/// <summary>
		/// Getter for KVALITA section
		/// </summary>
		/// <returns>dictionary with column name as KEY and list of values as VALUE</returns>
		public List<List<string>> GetQualityArray()
		{
			return qualityArray;
		}

		/// <summary>
		/// Parse data from HLAVICKA part to appropriate container
		/// </summary>
		/// <param name="sectionText">HLAVICKA part</param>
		void SetHeader(string sectionText)
		{
			string[] pairs = sectionText.Split('\n');
			foreach (string item in pairs)
			{
				string[] values = TrimText(item).Split(';');
				headerArray.Add(values);
			}
		}

		/// <summary>
		/// Parse data from KVALITA part to appropriate container
		/// </summary>
		/// <param name="sectionText">KVALITA part</param>
		void SetQuality(string sectionText)
		{
			string[] temp = sectionText.Split('\n');
			string[][] temp1 = new string[temp.Length][];
			for (int i = 0; i < temp.Length; i++)
			{
				temp1[i] = temp[i].Split(';');
				if (temp1[i].Length>1)
				{
					qualityArray.Add(new List<string>());
				}
				else
				{
					break;
				}
			}

			for (int j = 0; j < temp1[0].Length; j++)
			{
				if (TrimText(temp1[0][j]).StartsWith("I") || TrimText(temp1[0][j]).StartsWith("U"))
				{
					qualityArray[0].Add(TrimText(temp1[0][j]));
					for (int i = 1; i < qualityArray.Count; i++)
					{
						qualityArray[i].Add(TrimText(temp1[i][j]));
					}
				}
			}
		}

		/// <summary>
		/// Parse data from specific section
		/// </summary>
		void ParseSection(int initPos, int endPos, TextParser SpecificParser)
		{
			string tempText = String.Copy(text.Substring(initPos, endPos - initPos));
			tempText = TrimText(tempText);
			SpecificParser(TrimText(tempText));

		}

		/// <summary>
		/// Separate text from file to containers
		/// </summary>
		public void ParseFile(string fileText)
		{
			text = fileText;
			int initPos = text.IndexOf(header) + header.Length;
			int endPos = text.IndexOf(quality);
			ParseSection(initPos, endPos, SetHeader);

			initPos = endPos + quality.Length;
			endPos = text.IndexOf(events);
			ParseSection(initPos, endPos, SetQuality);

		}
	}
}
