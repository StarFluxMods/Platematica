using System;
using System.Collections.Generic;
using System.Text;

namespace Platematica.Utils
{
	public class LZString
	{
		private static IDictionary<char, char> CreateBaseDict(string alphabet)
		{
			Dictionary<char, char> dictionary = new Dictionary<char, char>();
			for (int i = 0; i < alphabet.Length; i++)
			{
				dictionary[alphabet[i]] = (char)i;
			}
			return dictionary;
		}

		public static string CompressToEncodedURIComponent(string input)
		{
			bool flag = input == null;
			string text;
			if (flag)
			{
				text = "";
			}
			else
			{
				text = LZString.Compress(input, 6, (int code) => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$"[code]);
			}
			return text;
		}

		public static string DecompressFromEncodedURIComponent(string input)
		{
			bool flag = input == null;
			if (flag)
			{
				throw new ArgumentNullException("input");
			}
			input = input.Replace(" ", "+");
			return LZString.Decompress(input.Length, 32, (int index) => LZString.KeyStrUriSafeDict[input[index]]);
		}

		public static string Compress(string uncompressed)
		{
			return LZString.Compress(uncompressed, 16, (int code) => (char)code);
		}

		private static string Compress(string uncompressed, int bitsPerChar, Func<int, char> getCharFromInt)
		{
			bool flag = uncompressed == null;
			if (flag)
			{
				throw new ArgumentNullException("uncompressed");
			}
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			Dictionary<string, bool> dictionary2 = new Dictionary<string, bool>();
			string text = "";
			int num = 2;
			int num2 = 3;
			int num3 = 2;
			StringBuilder stringBuilder = new StringBuilder();
			int num4 = 0;
			int num5 = 0;
			int num6;
			foreach (char c in uncompressed)
			{
				bool flag2 = !dictionary.ContainsKey(c.ToString());
				if (flag2)
				{
					dictionary[c.ToString()] = num2++;
					dictionary2[c.ToString()] = true;
				}
				string text2 = text + c.ToString();
				bool flag3 = dictionary.ContainsKey(text2);
				if (flag3)
				{
					text = text2;
				}
				else
				{
					bool flag4 = dictionary2.ContainsKey(text);
					if (flag4)
					{
						bool flag5 = LZString.FirstOrDefault(text) < 'Ā';
						if (flag5)
						{
							for (int j = 0; j < num3; j++)
							{
								num4 <<= 1;
								bool flag6 = num5 == bitsPerChar - 1;
								if (flag6)
								{
									num5 = 0;
									stringBuilder.Append(getCharFromInt(num4));
									num4 = 0;
								}
								else
								{
									num5++;
								}
							}
							num6 = (int)LZString.FirstOrDefault(text);
							for (int j = 0; j < 8; j++)
							{
								num4 = (num4 << 1) | (num6 & 1);
								bool flag7 = num5 == bitsPerChar - 1;
								if (flag7)
								{
									num5 = 0;
									stringBuilder.Append(getCharFromInt(num4));
									num4 = 0;
								}
								else
								{
									num5++;
								}
								num6 >>= 1;
							}
						}
						else
						{
							num6 = 1;
							for (int j = 0; j < num3; j++)
							{
								num4 = (num4 << 1) | num6;
								bool flag8 = num5 == bitsPerChar - 1;
								if (flag8)
								{
									num5 = 0;
									stringBuilder.Append(getCharFromInt(num4));
									num4 = 0;
								}
								else
								{
									num5++;
								}
								num6 = 0;
							}
							num6 = (int)LZString.FirstOrDefault(text);
							for (int j = 0; j < 16; j++)
							{
								num4 = (num4 << 1) | (num6 & 1);
								bool flag9 = num5 == bitsPerChar - 1;
								if (flag9)
								{
									num5 = 0;
									stringBuilder.Append(getCharFromInt(num4));
									num4 = 0;
								}
								else
								{
									num5++;
								}
								num6 >>= 1;
							}
						}
						num--;
						bool flag10 = num == 0;
						if (flag10)
						{
							num = (int)Math.Pow(2.0, (double)num3);
							num3++;
						}
						dictionary2.Remove(text);
					}
					else
					{
						num6 = dictionary[text];
						for (int j = 0; j < num3; j++)
						{
							num4 = (num4 << 1) | (num6 & 1);
							bool flag11 = num5 == bitsPerChar - 1;
							if (flag11)
							{
								num5 = 0;
								stringBuilder.Append(getCharFromInt(num4));
								num4 = 0;
							}
							else
							{
								num5++;
							}
							num6 >>= 1;
						}
					}
					num--;
					bool flag12 = num == 0;
					if (flag12)
					{
						num = (int)Math.Pow(2.0, (double)num3);
						num3++;
					}
					dictionary[text2] = num2++;
					text = c.ToString();
				}
			}
			bool flag13 = text != "";
			if (flag13)
			{
				bool flag14 = dictionary2.ContainsKey(text);
				if (flag14)
				{
					bool flag15 = LZString.FirstOrDefault(text) < 'Ā';
					if (flag15)
					{
						for (int j = 0; j < num3; j++)
						{
							num4 <<= 1;
							bool flag16 = num5 == bitsPerChar - 1;
							if (flag16)
							{
								num5 = 0;
								stringBuilder.Append(getCharFromInt(num4));
								num4 = 0;
							}
							else
							{
								num5++;
							}
						}
						num6 = (int)LZString.FirstOrDefault(text);
						for (int j = 0; j < 8; j++)
						{
							num4 = (num4 << 1) | (num6 & 1);
							bool flag17 = num5 == bitsPerChar - 1;
							if (flag17)
							{
								num5 = 0;
								stringBuilder.Append(getCharFromInt(num4));
								num4 = 0;
							}
							else
							{
								num5++;
							}
							num6 >>= 1;
						}
					}
					else
					{
						num6 = 1;
						for (int j = 0; j < num3; j++)
						{
							num4 = (num4 << 1) | num6;
							bool flag18 = num5 == bitsPerChar - 1;
							if (flag18)
							{
								num5 = 0;
								stringBuilder.Append(getCharFromInt(num4));
								num4 = 0;
							}
							else
							{
								num5++;
							}
							num6 = 0;
						}
						num6 = (int)LZString.FirstOrDefault(text);
						for (int j = 0; j < 16; j++)
						{
							num4 = (num4 << 1) | (num6 & 1);
							bool flag19 = num5 == bitsPerChar - 1;
							if (flag19)
							{
								num5 = 0;
								stringBuilder.Append(getCharFromInt(num4));
								num4 = 0;
							}
							else
							{
								num5++;
							}
							num6 >>= 1;
						}
					}
					num--;
					bool flag20 = num == 0;
					if (flag20)
					{
						num = (int)Math.Pow(2.0, (double)num3);
						num3++;
					}
					dictionary2.Remove(text);
				}
				else
				{
					num6 = dictionary[text];
					for (int j = 0; j < num3; j++)
					{
						num4 = (num4 << 1) | (num6 & 1);
						bool flag21 = num5 == bitsPerChar - 1;
						if (flag21)
						{
							num5 = 0;
							stringBuilder.Append(getCharFromInt(num4));
							num4 = 0;
						}
						else
						{
							num5++;
						}
						num6 >>= 1;
					}
				}
				num--;
				bool flag22 = num == 0;
				if (flag22)
				{
					num3++;
				}
			}
			num6 = 2;
			for (int j = 0; j < num3; j++)
			{
				num4 = (num4 << 1) | (num6 & 1);
				bool flag23 = num5 == bitsPerChar - 1;
				if (flag23)
				{
					num5 = 0;
					stringBuilder.Append(getCharFromInt(num4));
					num4 = 0;
				}
				else
				{
					num5++;
				}
				num6 >>= 1;
			}
			for (;;)
			{
				num4 <<= 1;
				bool flag24 = num5 == bitsPerChar - 1;
				if (flag24)
				{
					break;
				}
				num5++;
			}
			stringBuilder.Append(getCharFromInt(num4));
			return stringBuilder.ToString();
		}

		public static string Decompress(string compressed)
		{
			bool flag = compressed == null;
			if (flag)
			{
				throw new ArgumentNullException("compressed");
			}
			return LZString.Decompress(compressed.Length, 32768, (int index) => compressed[index]);
		}

		private static string Decompress(int length, int resetValue, Func<int, char> getNextValue)
		{
			List<string> list = new List<string>();
			int num = 4;
			int num2 = 3;
			StringBuilder stringBuilder = new StringBuilder();
			int num3 = 0;
			char c = '\0';
			char c2 = getNextValue(0);
			int num4 = resetValue;
			int num5 = 1;
			for (int i = 0; i < 3; i++)
			{
				list.Add(((char)i).ToString());
			}
			int num6 = (int)Math.Pow(2.0, 2.0);
			for (int num7 = 1; num7 != num6; num7 <<= 1)
			{
				int num8 = (int)c2 & num4;
				num4 >>= 1;
				bool flag = num4 == 0;
				if (flag)
				{
					num4 = resetValue;
					c2 = getNextValue(num5++);
				}
				num3 |= ((num8 > 0) ? 1 : 0) * num7;
			}
			switch (num3)
			{
			case 0:
			{
				num3 = 0;
				num6 = (int)Math.Pow(2.0, 8.0);
				for (int num7 = 1; num7 != num6; num7 <<= 1)
				{
					int num8 = (int)c2 & num4;
					num4 >>= 1;
					bool flag2 = num4 == 0;
					if (flag2)
					{
						num4 = resetValue;
						c2 = getNextValue(num5++);
					}
					num3 |= ((num8 > 0) ? 1 : 0) * num7;
				}
				c = (char)num3;
				break;
			}
			case 1:
			{
				num3 = 0;
				num6 = (int)Math.Pow(2.0, 16.0);
				for (int num7 = 1; num7 != num6; num7 <<= 1)
				{
					int num8 = (int)c2 & num4;
					num4 >>= 1;
					bool flag3 = num4 == 0;
					if (flag3)
					{
						num4 = resetValue;
						c2 = getNextValue(num5++);
					}
					num3 |= ((num8 > 0) ? 1 : 0) * num7;
				}
				c = (char)num3;
				break;
			}
			case 2:
				return "";
			}
			string text = c.ToString();
			list.Add(text);
			stringBuilder.Append(c);
			for (;;)
			{
				bool flag4 = num5 > length;
				if (flag4)
				{
					break;
				}
				num3 = 0;
				num6 = (int)Math.Pow(2.0, (double)num2);
				for (int num7 = 1; num7 != num6; num7 <<= 1)
				{
					int num8 = (int)c2 & num4;
					num4 >>= 1;
					bool flag5 = num4 == 0;
					if (flag5)
					{
						num4 = resetValue;
						c2 = getNextValue(num5++);
					}
					num3 |= ((num8 > 0) ? 1 : 0) * num7;
				}
				int num9;
				switch (num9 = num3)
				{
				case 0:
				{
					num3 = 0;
					num6 = (int)Math.Pow(2.0, 8.0);
					for (int num7 = 1; num7 != num6; num7 <<= 1)
					{
						int num8 = (int)c2 & num4;
						num4 >>= 1;
						bool flag6 = num4 == 0;
						if (flag6)
						{
							num4 = resetValue;
							c2 = getNextValue(num5++);
						}
						num3 |= ((num8 > 0) ? 1 : 0) * num7;
					}
					num9 = list.Count;
					list.Add(((char)num3).ToString());
					num--;
					break;
				}
				case 1:
				{
					num3 = 0;
					num6 = (int)Math.Pow(2.0, 16.0);
					for (int num7 = 1; num7 != num6; num7 <<= 1)
					{
						int num8 = (int)c2 & num4;
						num4 >>= 1;
						bool flag7 = num4 == 0;
						if (flag7)
						{
							num4 = resetValue;
							c2 = getNextValue(num5++);
						}
						num3 |= ((num8 > 0) ? 1 : 0) * num7;
					}
					num9 = list.Count;
					list.Add(((char)num3).ToString());
					num--;
					break;
				}
				case 2:
					goto IL_03EB;
				}
				bool flag8 = num == 0;
				if (flag8)
				{
					num = (int)Math.Pow(2.0, (double)num2);
					num2++;
				}
				bool flag9 = list.Count - 1 >= num9;
				string text2;
				if (flag9)
				{
					text2 = list[num9];
				}
				else
				{
					bool flag10 = num9 == list.Count;
					if (!flag10)
					{
						goto IL_046B;
					}
					text2 = text + text[0].ToString();
				}
				stringBuilder.Append(text2);
				list.Add(text + text2[0].ToString());
				num--;
				text = text2;
				bool flag11 = num == 0;
				if (flag11)
				{
					num = (int)Math.Pow(2.0, (double)num2);
					num2++;
				}
			}
			return "";
			IL_03EB:
			return stringBuilder.ToString();
			IL_046B:
			return null;
		}

		private static char FirstOrDefault(string value)
		{
			bool flag = string.IsNullOrEmpty(value);
			char c;
			if (flag)
			{
				c = '\0';
			}
			else
			{
				c = value[0];
			}
			return c;
		}

		private const string KeyStrBase64 = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

		private const string KeyStrUriSafe = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$";

		private static readonly IDictionary<char, char> KeyStrBase64Dict = LZString.CreateBaseDict("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=");

		private static readonly IDictionary<char, char> KeyStrUriSafeDict = LZString.CreateBaseDict("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+-$");
	}
}
