using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolCodeGenerator
{
    internal class MakePacketStream_CPP : MakeFile
    {
        // OutFile Info
        private string CPPInfo_ = "#pragma once\n\n#include \"stdafx.h\"\n#include \"PacketDefine.h\"\n#include \"Stream.h\"\n\n";
        protected override string FileName => "PacketStream.h";

        // Parsing Variables
        private string class_ = "";

        protected override string? Parse(string file)
        {
            string type = "public:\n\tPacketType type() { return E_";
            string encode = "\n\tvoid encode(Stream& stream) {\n\t\tstream";
            string decode = "\n\tvoid decode(Stream& stream) {\n\t\tstream";
            file = file.Replace("message", "class");

            class_ = (
                CPPInfo_ +
                "class Packet\n{\npublic:\n\tvirtual PacketType type() = 0;" +
                "\n\tvirtual void encode(Stream& stream) { };" +
                "\n\tvirtual void decode(Stream& stream) { };\n};\n\n"
                );

            using (StringReader reader = new StringReader(file)) {
                bool isClass = false;
                int varCnt = 0;
                string[] delimeters = { " ", ";" };
                List<string> vars = new List<string>();
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("class")) {
                        string[] className = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                        class_ += className[0] + " PT_" + className[1] + " : public Packet\n{\n";
                        class_ += (type + className[1] + "; }\n\n");
                        isClass = true;
                    }
                    else if (line.StartsWith("\t")) {
                        if (isClass) {
                            class_ += (line + "\n");
                            string[] split = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);
                            vars.Add(split[split.Length - 1]);
                            varCnt++;
                        }
                    }
                    else if (line.StartsWith("}")) {
                        if (isClass) {
                            // encode
                            class_ += encode;
                            for (int cnt = 0; cnt < varCnt; cnt++) {
                                class_ += (" << " + vars[cnt]);
                            }
                            class_ += ";\n\t}\n";

                            // decode
                            class_ += decode;
                            for (int cnt = 0; cnt < varCnt; cnt++) {
                                class_ += (" >> " + vars[cnt]);
                            }
                            class_ += ";\n\t}\n};\n\n";

                            vars.Clear();
                            varCnt = 0;
                            isClass = false;
                        }
                    }
                }
            }

            return class_;
        }
    }
}
