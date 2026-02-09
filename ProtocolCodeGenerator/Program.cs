using Microsoft.VisualBasic.FileIO;
using static System.Net.Mime.MediaTypeNames;

namespace ProtocolCodeGenerator
{
    enum FileType : int { CPP = 0, CS = 1 };

    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 4) {
                Console.WriteLine("! [Error]: 프로그램을 실행하기 위한 인수가 부족합니다.\n");
                return;
            }

            FileType fileType;
            if (args[0].Equals("CPP") || args[0].Equals("cpp")) {
                fileType = FileType.CPP;
            } else if (args[0].Equals("CS") || args[0].Equals("cs")) {
                fileType = FileType.CS;
            }
            else {
                Console.WriteLine("! [Error]: 프로그램을 실행하기 위한 인수가 부족합니다.\n");
                return;
            }

            string protocolPath = args[1];
            string outFilePath = args[2];

            if (protocolPath == null) {
                Console.WriteLine("! [Error]: 프로토콜 파일 경로 오류\n");
                return;
            }
            
            if (outFilePath == null) {
                Console.WriteLine("! [Error]: 파일 저장 경로 오류\n");
                return;
            }

            string packetNum = args[3];

            Console.WriteLine("# Protocol Code Generator is Running...\n");

            IDLHandler idl = new IDLHandler();
            string convert = idl.ReadFile(protocolPath, fileType);

            if (fileType == FileType.CPP) {
                // PacketDefine.h 생성
                MakePacketDefine_CPP packetDefineCPP = new MakePacketDefine_CPP(packetNum);
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
