using System;
using System.Text;
using System.Text.RegularExpressions;
using Lesnikowski.Mail;
using Lesnikowski.Client;
using Lesnikowski.StringCoding;

// Patched class by Moshe Fishman to support non-latin languages encodings
namespace BugNET.POP3Reader
{
	/// <summary>
	/// Summary description for MailAddress.
	/// </summary>
	public class MailAddress
	{
        /// <summary>
        /// Parses the specified s.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
		public static MailBox Parse(string s)
		{
			if (s == null)
			{
				return null;
			}

			Encoding encoding = MailData.ChooseEncoding(s);

			string a1 = @"([^\s,;<>]*@[^\s,;<>]*?)";
			string[] textArray1 = new string[9] { "\\s*(\"(?<1>([^\\\\]|\\\\.)*?)\"\\s*<\\s*(?<2>", a1, @")\s*>|<\s*(?<2>", a1, @")\s*>|(?<1>[^,]*?)\s*<\s*(?<2>", a1, @")\s*>|(?<2>", a1, @"))\s*" } ;
			string b1 = string.Concat(textArray1);
			Regex c1 = new Regex('^' + b1 + '$', RegexOptions.Compiled | RegexOptions.ExplicitCapture);

			Match match1 = c1.Match(s);
			if (!match1.Success)
			{
				throw new MailException("MailBox parse exception");
			}

			MailAddress ma = new MailAddress();

			if (!match1.Groups[1].Success)
				return new MailBox(match1.Groups[2].Value, null);

			return new MailBox(match1.Groups[2].Value, HeaderCoding.Encode(match1.Groups[1].Value, encoding));
		}
	}
}
