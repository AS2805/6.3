
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hik.Communication.Scs.Communication.Messages.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class MessageHelper
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

        public byte[] BuildMessageBytes(string msg)
        {
            IList<byte> bytes = new List<byte>();
            bytes.Add(Stx);

            foreach (var s in StringArray(msg))
            {
                Encoding.ASCII.GetBytes(s).ToList().ForEach(bytes.Add);
                bytes.Add(Fs);
            }
            bytes.Add(Etx);
            bytes.Add(Lrc);
            return bytes.ToArray();
        }

        public string[] StringArray(string msg)
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
    }
}
