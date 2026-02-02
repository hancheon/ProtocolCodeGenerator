namespace ProtocolCodeGenerator
{
    internal class main
    {
        static void Main(string[] args)
        {
            Console.WriteLine("# Protocol Code Generator is Running...\n");

            IDLHandler test = new IDLHandler();
            string testStr = test.ReadFile("C:\\Users\\Hancheon\\Desktop\\ProtocolCodeGenerator\\ProtocolCodeGenerator\\test.txt");

            MakePacket packet = new MakePacket();
            packet.Make(testStr, "C:\\Users\\Hancheon\\Desktop\\test\\Packet.h");

            // TODO: Packet.h 생성

            // TODO: PacketStream.h 생성

            // TODO: PacketFactory.h 생성
        }
    }
}
