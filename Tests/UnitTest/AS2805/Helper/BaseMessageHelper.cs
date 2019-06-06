using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UnitTest.AS2805.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class BaseMessageHelper
    {
        public byte[] BuildMessageBytes(string msg)
        {
            return Encoding.ASCII.GetBytes(msg); 
        }        

        public byte[] BuildMessageWithHeaderLength(string message_byte_format)
        {
            byte[] bytes = StringToBytes(message_byte_format);
            byte[] length = BitConverter.GetBytes((short)bytes.Length);
            IList<byte> completeMsg = length.Reverse().ToList();
            foreach (byte t in bytes)
                completeMsg.Add(t);
            return completeMsg.ToArray();
        }

        public string BuildStringMessageHexString(string msg)
        {
            var bytes = Encoding.ASCII.GetBytes(msg);
            string str = BitConverter.ToString(bytes).Replace("-", "");
            return str;
        }

        public string HexStringToString(string hexString)
        {
            if (hexString == null || (hexString.Length & 1) == 1)
            {
                throw new ArgumentException();
            }
            var sb = new StringBuilder();
            for (var i = 0; i < hexString.Length; i += 2)
            {
                var hexChar = hexString.Substring(i, 2);
                sb.Append((char)Convert.ToByte(hexChar, 16));
            }
            return sb.ToString();
        }

        public string AsciiOctets2String(byte[] bytes)
        {
            var sb = new StringBuilder(bytes.Length);
            foreach (char c in bytes.Select(b => (char)b))
            {
                if (c < '\u0020')
                {
                    sb.Append(ControlChars[c]);
                }
                else if (c == '\u007F')
                {
                    sb.Append("<DEL>");
                }
                else if (c > '\u007F')
                {
                    sb.AppendFormat(@"\u{0:X4}", (ushort)c);
                }
                else /* 0x20-0x7E */
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        private readonly string[] ControlChars =
        {
            "<NUL>", "<SOH>", "<STX>", "<ETX>",
            "<EOT>", "<ENQ>", "<ACK>", "<BEL>",
            "<BS>", "<HT>", "<LF>", "<VT>",
            "<FF>", "<CR>", "<SO>", "<SI>",
            "<DLE>", "<DC1>", "<DC2>", "<DC3>",
            "<DC4>", "<NAK>", "<SYN>", "<ETB>",
            "<CAN>", "<EM>", "<SUB>", "<ESC>",
            "<FS>", "<GS>", "<RS>", "<US>"
        };

        public string GenerateEncryptKey(int length)
        {
            string chars = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = "";
            Random rand = new Random();
            for (int i = 0; i < length; i++)
            {
                int num = rand.Next(0, chars.Length - 1);
                result += chars[num];
            }
            return result;
        }

        public byte[] StringToBytes(string text) { return (StringToBytes(text, -1)); }

        public byte[] StringToBytes(string text, int len)
        {

            int p = 0, l = text.Length, s = l < 2 ? 0 : text[0] == '0' && text[1] == 'x' ? 2 : 0, lr = len < 0 ? (l - s) / 2 + (l - s) % 2 : len;

            byte[] r = new byte[lr];

            for (int i = s; i < l & p < lr * 2; i++)
            {

                if (text[i] >= '0' && text[i] <= '9') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - '0') : (byte)((text[i] - '0') << 4)); p++; }

                else if (text[i] >= 'A' && text[i] <= 'F') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - 'A' + 10) : (byte)((text[i] - 'A' + 10) << 4)); p++; }

                else if (text[i] >= 'a' && text[i] <= 'f') { r[p / 2] |= ((p % 2 != 0) ? (byte)(text[i] - 'a' + 10) : (byte)((text[i] - 'a' + 10) << 4)); p++; }

            }

            if (len < 0) Array.Resize(ref r, p / 2 + p % 2);

            return (r);

        }
    }
}
