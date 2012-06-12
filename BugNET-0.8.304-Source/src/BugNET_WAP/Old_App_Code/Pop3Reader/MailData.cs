using System;
using System.Text;
using Lesnikowski.Mail;
using Lesnikowski.Client;
using Lesnikowski.StringCoding;

// Patched class by Moshe Fishman to support non-latin languages encodings
namespace BugNET.POP3Reader
{
	/// <summary>
	/// Summary description for MailData.
	/// </summary>
	public class MailData : MimeData 
	{
        /// <summary>
        /// Default encoding
        /// </summary>
		public static Encoding DefaultEncoding = Encoding.Default;

        /// <summary>
        /// Initializes a new instance of the <see cref="T:MailData"/> class.
        /// </summary>
        /// <param name="body">The body.</param>
        /// <param name="contentType">Type of the content.</param>
		public MailData(string body, ContentType contentType)
		{
			this.Body = body;
			this.ContentType = contentType;
		}

        /// <summary>
        /// Creates the string.
        /// </summary>
        /// <returns></returns>
		public override string CreateString()
		{
			MimeEncoding mimeEncoding;

			StringBuilder sb = new StringBuilder();
			if (this.ContentId != null)
			{
				sb.Append("Content-id: <");
				sb.Append(this.ContentId);
				sb.Append(">\r\n");
			}
			if ((this.ContentDisposition != null) && (this.ContentDisposition.Options != null))
			{
				sb.Append(this.ContentDisposition.CreateString());
				if ((this.FileName != null) && (this.FileName != string.Empty))
				{
					sb.Append(";\r\n\tFileName=\"");
					sb.Append(this.FileName);
					sb.Append('"');
				}
				sb.Append("\r\n");
			}
			if (this.ContentDisposition != null)
			{
				sb.Append(this.ContentDisposition.CreateString());
				sb.Append("\r\n");
			}
			string text1 = string.Empty;
			if (this.body != null)
			{
				byte[] buffer1 = DefaultEncoding.GetBytes(this.body);
				mimeEncoding = Coding.ChooseFormat(buffer1);
				if (mimeEncoding == MimeEncoding.Bit7)
				{
					text1 = this.body;
				}
				else if (mimeEncoding == MimeEncoding.Base64)
				{
					text1 = Coding.EncodeBase64(buffer1);
				}
				else
				{
					text1 = EncodeQuotedPrintable(buffer1);
				}
				sb.Append(this.ContentType.CreateString());
				if (mimeEncoding != MimeEncoding.Bit7)
				{
					sb.AppendFormat(";\r\n\tcharset=\"{0}\"", DefaultEncoding.BodyName);
				}
				sb.Append("\r\n");
			}
			else
			{
				mimeEncoding = MimeEncoding.Base64;
				text1 = Coding.EncodeBase64(this.data, 0x4c);
				sb.Append(this.ContentType.CreateString());
				sb.Append("\r\n");
			}
			if (mimeEncoding != MimeEncoding.Bit7)
			{
				sb.Append(new ContentEncoding(mimeEncoding).CreateString());
				sb.Append("\r\n");
			}
			sb.Append("\r\n");
			sb.Append(text1);
			return sb.ToString();
		}

        /// <summary>
        /// Chooses the encoding.
        /// </summary>
        /// <param name="chars">The chars.</param>
        /// <returns></returns>
		public static Encoding ChooseEncoding(string chars)
		{
			foreach(char c in chars.ToCharArray())
			{
				if (c > 0x7f)
					return DefaultEncoding;
			}

			return Encoding.ASCII;
		}

        /// <summary>
        /// Headers the encode.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
		public static string HeaderEncode(string str)
		{
			return HeaderCoding.Encode(str, ChooseEncoding(str));
		}

        /// <summary>
        /// Encodes the quoted printable.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
		public static string EncodeQuotedPrintable(byte[] data)
		{
			if (data == null)
			{
				return null;
			}
			StringBuilder sb = new StringBuilder(data.Length);
			for (int i = 0; i < data.Length; i++)
			{
				if (data[i] <= 0x7f)
				{
					if (data[i] == 0x3d) // '=' patch
						sb.Append("=3D");
					else
						sb.Append((char) data[i]);
				}
				else
				{
					sb.Append('=');
					sb.Append(Convert.ToString(data[i], 0x10).ToUpper());
				}
			}
			return sb.ToString();
		}

	}
}