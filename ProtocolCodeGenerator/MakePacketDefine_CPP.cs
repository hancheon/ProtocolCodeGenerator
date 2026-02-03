using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace ProtocolCodeGenerator
{
    internal class MakePacketDefine_CPP : MakeFile
    {
        // OutFile Info
        private int startNum_ = 1000; // TODO: 사용자가 시작 번호 설정하도록 바꿀꺼임
        private string CPPInfo_ = "#pragma once\n\n#include \"stdafx.h\"\n\n";
        protected override string FileName => "PacketDefine.h";

        // Parsing Variables
        private string packet_;
        private string message_;
        private List<string> packetTypes_ = new List<string>();

        protected override string? Parse(string file)
        {
            int packetCount = 0;
            bool isStruct = false;
            bool isMessage = false;

            file = file.Replace("header", "struct PacketHeader");
            packet_ = CPPInfo_;

            using (StringReader reader = new StringReader(file)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("struct")) {
                        isStruct = true;
                    }
                    else if (line.StartsWith("message")) {
                        isMessage = true;
                        message_ += (line + "\n");
                        packetTypes_.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
                        packetCount++;
                        continue;
                    }
                    else if (line.StartsWith("}")) {
                        if (isMessage) {
                            message_ += (line + ";\n");
                            isMessage = false;
                            continue;
                        }
                        else if (isStruct) {
                            line = line.Replace("}", "};");
                            isStruct = false;
                        }
                    }
                    else if (line.StartsWith("\t")) {
                        if (isMessage) {
                            message_ += (line + "\n");
                            continue;
                        }
                    }

                    if (isMessage) continue;
                    packet_ += (line + "\n");
                }
            }

            // PacketType Enum 생성
            packet_ = packet_.TrimEnd('\n');
            packet_ += "\n\nenum PacketType : __int64 {";
            for (int cnt = 0; cnt < packetCount; cnt++) {
                string packetNum = (cnt + startNum_).ToString();
                packet_ += "\n\t/*" + packetNum + "*/\tE_" + packetTypes_[cnt] + " = " + packetNum + ",";
            }
            packet_ += "\n}";

            file = message_;

            return packet_;
        }
    }
}
