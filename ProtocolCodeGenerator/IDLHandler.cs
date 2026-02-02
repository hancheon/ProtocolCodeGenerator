using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ProtocolCodeGenerator
{
    internal class IDLHandler
    {
        private string file_;

        private string[] message_;

        private string[] keyword = { "header", "message", "enum", "struct" };

        public string ReadFile(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath)) {
                file_ = reader.ReadToEnd(); // 한번에 파일 다 읽어오기
            }

            if (file_ != null) {
                file_ = file_.Replace("header", "struct PacketHeader");
            }

            return file_;
        }
    }
}
