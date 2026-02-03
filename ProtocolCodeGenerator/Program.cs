using Microsoft.VisualBasic.FileIO;
using static System.Net.Mime.MediaTypeNames;

namespace ProtocolCodeGenerator
{
    enum FileType : int { CPP = 0, CS = 1 };
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: 실행방법 -> 프로그램명 옵션(C++/C#) -I [프로토콜 파일 위치] -O [저장될 위치]
            FileType fileType = FileType.CPP; // TODO: CPP / CS 옵션
            // TODO: 입력으로 받은 프로토콜 파일 위치
            string protocolPath = "C:\\Users\\Hancheon\\Desktop\\ProtocolCodeGenerator\\ProtocolCodeGenerator\\Protocol.txt";
            string outFilePath = "C:\\Users\\Hancheon\\Desktop\\test";

            if (protocolPath == null) {
                Console.WriteLine("! [Error]: 프로토콜 파일 경로 오류\n");
                return;
            }
            
            if (outFilePath == null) {
                Console.WriteLine("! [Error]: 파일 저장 경로 오류\n");
                return;
            }

            Console.WriteLine("# Protocol Code Generator is Running...\n");

            IDLHandler idl = new IDLHandler();
            string convert = idl.ReadFile(protocolPath, fileType);

            if (fileType == FileType.CPP) {
                // PacketDefine.h 생성
                MakePacketDefine_CPP packetDefineCPP = new MakePacketDefine_CPP();
                packetDefineCPP.Make(convert, outFilePath);

                // PacketStream.h 생성
                MakePacketStream_CPP packetStreamCPP = new MakePacketStream_CPP();
                packetStreamCPP.Make(convert, outFilePath);


                // PacketFactory.h 생성
                MakePacketFactory_CPP packetFactoryCPP = new MakePacketFactory_CPP();
                packetFactoryCPP.Make(convert, outFilePath);
            }
            else if (fileType == FileType.CS) {

            }
        }
    }
}
