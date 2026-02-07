using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolCodeGenerator
{
    internal class MakePacketFactory_CPP : MakeFile
    {
        // OutFile Info
        private string CPPInfo_ = "#pragma once\n\n#include \"stdafx.h\"\n#include \"PacketDefine.h\"\n#include \"PacketStream.h\"\n\n";
        protected override string FileName => "PacketFactory.h";

        protected override string? Parse(string file)
        {
            string type = "__int32";

            using (StringReader reader = new StringReader(file)) {
                string? line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.Contains("PACKET_TYPE")) {
                        string[] packetType = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        if (packetType.Length == 2)
                            type = packetType[0].Trim();
                        else if (packetType.Length == 3)
                            type = packetType[0].Trim() + " " + packetType[1];
                    }
                }
            }

                string factory = (
                CPPInfo_ +
                "class PacketFactory : public Singleton<PacketFactory>\n{\npublic:\n\t" +
                "Packet* getPacket(" + type + " packetType)\n\t{\n\t\tswitch (packetType) {\n"
                );

            using (StringReader reader = new StringReader(file)) {
                string? line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("message")) {
                        string[] message = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        factory += ("\t\t\tcase E_" + message[1] + ":\treturn new PT_" + message[1] + "();\n");
                    }
                }

                factory += "\t\t}\n\t\treturn nullptr;\n\t}\n};";
            }

            return factory;
        }
    }
}
