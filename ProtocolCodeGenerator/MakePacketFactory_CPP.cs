using System;
using System.Collections.Generic;
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

        // Parsing Variables
        private string factory_ = "";

        protected override string? Parse(string file)
        {
            factory_ = (
                CPPInfo_ +
                "class PacketFactory : public Singleton<PacketFactory>\n{\npublic:\n\t" +
                "Packet* getPacket(__int64 packetType)\n\t{\n\t\tswitch (packetType) {\n"
                );

            using (StringReader reader = new StringReader(file)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("message")) {
                        string[] message = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        factory_ += ("\t\t\tcase E_" + message[1] + ":\treturn new PT_" + message[1] + "();\n");
                    }
                }

                factory_ += "\t\t}\n\t\treturn nullptr;\n\t}\n};";
            }

            return factory_;
        }
    }
}
