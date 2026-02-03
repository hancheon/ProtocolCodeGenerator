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
        private string convert_ = "";
        private string protocol_ = "";

        public string ReadFile(string filePath, FileType fileType)
        {
            using (StreamReader reader = new StreamReader(filePath)) {
                protocol_ = reader.ReadToEnd(); // 한번에 파일 다 읽어오기
            }

            return this.ConvertType(protocol_, fileType);
        }

        public string ConvertType(string filePath, FileType fileType)
        {
            bool isEnum = false;
            string[] delimeters = { "\t", ":", ",", " " };
            using (StringReader reader = new StringReader(protocol_)) {
                string? line;
                while ((line = reader.ReadLine()) != null) {
                    if (line.StartsWith("enum")) {
                        isEnum = true;
                    }
                    else if (line.StartsWith("\t")) {
                        if (isEnum) {
                            convert_ += (line + "\n");
                            continue;
                        }

                        string[] member = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries);

                        // 자료형 변환
                        switch (member[1]) {
                            case "CHAR":
                                member[1] = "char";
                                break;
                            case "BYTE":
                                member[1] = (fileType == FileType.CPP ? "unsigned char" : "byte");
                                break;
                            case "SHORT":
                                member[1] = "short";
                                break;
                            case "WORD":
                                member[1] = (fileType == FileType.CPP ? "unsigned short" : "ushort");
                                break;
                            case "INT32":
                                member[1] = (fileType == FileType.CPP ? "__int32" : "int");
                                break;
                            case "UINT32":
                                member[1] = (fileType == FileType.CPP ? "unsigned __int32" : "uint");
                                break;
                            case "LONG":
                                member[1] = "long";
                                break;
                            case "DWORD":
                                member[1] = (fileType == FileType.CPP ? "unsigned long" : "long");
                                break;
                            case "INT64":
                                member[1] = (fileType == FileType.CPP ? "__int64" : "Int64");
                                break;
                            case "UINT64":
                                member[1] = (fileType == FileType.CPP ? "unsigned __int64" : "UInt64");
                                break;
                            case "string":
                                member[1] = (fileType == FileType.CPP ? "std::string" : "string");
                                break;
                        }

                        line = "\t" + member[1] + " " + member[0] + ";";
                    }
                    else if (line.StartsWith("}")) {
                        if (isEnum) isEnum = false;
                    }

                    convert_ += (line + "\n");
                }
            }

            return convert_;
        }
    }
}
