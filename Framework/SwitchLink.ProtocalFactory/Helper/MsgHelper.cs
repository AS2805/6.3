using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwitchLink.ProtocalFactory.Helper
{
    public class MsgHelper
    {
        private const byte Eot = 0x04;
        private const byte Enq = 0x05;
        private const byte Etx = 0x03;
        private const byte Stx = 0x02;
        private const byte Fs = 0x1c;
        private const byte Lrc = 0x56;
        private const byte Ack = 0x06;

        public static byte ENQ
        {
            get { return Enq; }
        }

        public static byte STX
        {
            get { return Stx; }
        }

        public static byte ETX
        {
            get { return Etx; }
        }

        public static byte ACK
        {
            get { return Ack; }
        }

        public static byte EOT
        {
            get { return Eot; }
        }

        public static byte FS
        {
            get { return Fs; }
        }

        public byte[] BuildMessageBytes(string msg)
        {
            return Encoding.ASCII.GetBytes(msg);
        }

        public string[] BuildMsgFromAtm(byte[] bytes)
        {
            string result = AsciiOctets2String(bytes);
            return StringArray(result);
        }

        private string[] StringArray(string msg)
        {
            msg = msg.Replace("<STX>", "").Replace("<ETX>", "");
            string[] parts = msg.Split(new[] { "<FS>" }, StringSplitOptions.None);
            parts = parts.Take(parts.Length - 1).ToArray();
            return parts;
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
            "<EOT>", "<Enq>", "<ACK>", "<BEL>",
            "<BS>", "<HT>", "<LF>", "<VT>",
            "<FF>", "<CR>", "<SO>", "<SI>",
            "<DLE>", "<DC1>", "<DC2>", "<DC3>",
            "<DC4>", "<NAK>", "<SYN>", "<ETB>",
            "<CAN>", "<EM>", "<SUB>", "<ESC>",
            "<FS>", "<GS>", "<RS>", "<US>"
        };

        public string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }

        public byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
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
    }

    public static class StringExtensions
    {
        public static string Right(this string str, int length)
        {
            return str.Substring(str.Length - length, length);
        }
    }
    
}
