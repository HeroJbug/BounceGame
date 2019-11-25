using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueKeywords
{
	[System.Serializable]
	public class KeywordDictionary : UDictionary<string, string>
	{

	}
	
	/// <summary>
	/// Responsible for holding and retrieving the keyword data
	/// </summary>
	[System.Serializable]
	public struct KeywordData
	{
		/// <summary>
		/// The Keywords used in the dialogue
		/// </summary>
		public KeywordDictionary keywords;
		private int lastKeywordLength;

		/// <summary>
		/// Places keyword into string
		/// </summary>
		/// <param name="i">The index in the string</param>
		/// <param name="str">The string to be changed</param>
		/// <returns>The altered string if it successfully replaces the keyword, null if otherwise</returns>
		public string PlaceKeyword(int i, string str, string[] speakers)
		{
			foreach (string kw in keywords.Keys)
			{
				if (i + (kw.Length - 1) < str.Length && str.Substring(i, kw.Length) == kw)
				{
					lastKeywordLength = keywords[kw].Length;
					return str.Substring(0, i) + keywords[kw] + str.Substring(i + kw.Length, str.Length - (i + kw.Length));
				}
			}

			if (i + 4 < str.Length && str.Substring(i, 5) == "spkr<")
			{
				int length = 6;
				while (str[i + length - 1] != '>')
				{
					length++;
				}

				int.TryParse(str.Substring(i + 5, length - 6), out int idx);
				lastKeywordLength = speakers[idx].Length;
				return str.Substring(0, i) + speakers[idx] + str.Substring(i + length, str.Length - (i + length));
			}

			return null;
		}

		/// <summary>
		/// Gets the length of the last keyword
		/// </summary>
		/// <returns>The length of the last keyword</returns>
		public int GetLastKeywordLength()
		{
			int length = lastKeywordLength;
			lastKeywordLength = 0;
			return length;
		}
	}
}
