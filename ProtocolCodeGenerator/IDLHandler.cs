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
        private string convertFile_;
        private string protocolFile_;
        private bool CPP = true;

        public string ReadFile(string filePath, bool fileType)
        {
            using (StreamReader reader = new StreamReader(filePath)) {
                protocolFile_ = reader.ReadToEnd(); // 한번에 파일 다 읽어오기
            }

            return this.ConvertType(protocolFile_);
        }

        public string ConvertType(string filePath)
        {
            bool isEnum = false;
            string[] delimeters = { "\t", ":", ",", " " };
            using (StringReader reader = new StringReader(protocolFile_)) {
                string line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("enum")) {
                        isEnum = true;
                    }
                    else if (line.StartsWith("\t")) {
                        if (isEnum) {
                            convertFile_ += (line + "\n");
                            continue;
                        }

                        string[] member = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

                        // 자료형 변환
                        switch (member[1]) {
                            case "CHAR":
                                member[1] = "char";
                                break;
                            case "BYTE":
                                member[1] = (CPP ? "unsigned char" : "byte");
                                break;
                            case "SHORT":
                                member[1] = "short";
                                break;
                            case "WORD":
                                member[1] = (CPP ? "unsigned short" : "ushort");
                                break;
                            case "INT32":
                                member[1] = (CPP ? "__int32" : "int");
                                break;
                            case "UINT32":
                                member[1] = (CPP ? "unsigned __int32" : "uint");
                                break;
                            case "LONG":
                                member[1] = "long";
                                break;
                            case "DWORD":
                                member[1] = (CPP ? "unsigned long" : "long");
                                break;
                            case "INT64":
                                member[1] = (CPP ? "__int64" : "Int64");
                                break;
                            case "UINT64":
                                member[1] = (CPP ? "unsigned __int64" : "UInt64");
                                break;
                        }

                        line = "\t" + member[1] + " " + member[0] + ";";
                    }
                    else if (line.StartsWith("}")) {
                        if (isEnum) isEnum = false;
                    }

                    convertFile_ += (line + "\n");
                }
            }

            return convertFile_;
        }
    }
}
