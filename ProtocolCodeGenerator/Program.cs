using Microsoft.VisualBasic.FileIO;

namespace ProtocolCodeGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // TODO: 실행방법 -> 프로그램명 옵션(C++/C#) -I [프로토콜 파일 위치] -O [저장될 위치]
            bool fileType = true; // TODO: CPP / CS 옵션
            // TODO: 입력으로 받은 프로토콜 파일 위치
            string protocolPath = "C:\\Users\\Hancheon\\Desktop\\ProtocolCodeGenerator\\ProtocolCodeGenerator\\Protocol.txt";
            string outFilePath = "C:\\Users\\Hancheon\\Desktop\\test";

            if (protocolPath == null) {
                Console.WriteLine("# 프로토콜 파일 경로 오류\n");
                return;
            }

            Console.WriteLine("# Protocol Code Generator is Running...\n");

            IDLHandler idl = new IDLHandler();
            string str = idl.ReadFile(protocolPath, fileType);

            // Packet.h 생성
            MakePacket packet = new MakePacket();
            packet.Make(str, outFilePath);


            // TODO: PacketStream.h 생성

            // TODO: PacketFactory.h 생성
        }
    }
}
