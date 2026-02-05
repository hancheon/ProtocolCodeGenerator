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
        private string CPPInfo_ = "#pragma once\n\n#include \"stdafx.h\"\n#pragma pack(push, 1)\n\n";
        protected override string FileName => "PacketDefine.h";

        protected override string? Parse(string file)
        {
            file = file.Replace("header", "struct PacketHeader");
            string packet = CPPInfo_;

            bool isStruct = false;
            bool isMessage = false;
            int packetCount = 0;
            List<string> packetTypes = new List<string>();

            using (StringReader reader = new StringReader(file)) {
                string? line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("struct")) {
                        isStruct = true;
                    }
                    else if (line.StartsWith("message")) {
                        isMessage = true;
                        packetTypes.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries)[1]);
                        packetCount++;
                        continue;
                    }
                    else if (line.StartsWith("}")) {
                        if (isMessage) {
                            isMessage = false;
                            continue;
                        }
                        else if (isStruct) {
                            isStruct = false;
                        }
                        line = line.Replace("}", "};");
                    }

                    if (isMessage) continue;
                    packet += (line + "\n");
                }
            }

            // PacketType Enum 생성
            packet = packet.TrimEnd('\n');
            packet += "\n\nenum PacketType : __int32 {";
            for (int cnt = 0; cnt < packetCount; cnt++) {
                string packetNum = (cnt + startNum_).ToString();
                packet += "\n\t/*" + packetNum + "*/\tE_" + packetTypes[cnt] + " = " + packetNum + ",";
            }
            packet += "\n};\n\n#pragma pack(pop)";

            return packet;
        }
    }
}
