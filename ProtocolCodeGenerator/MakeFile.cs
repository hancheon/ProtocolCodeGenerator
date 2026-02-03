using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolCodeGenerator
{
    internal abstract class MakeFile
    {
        protected abstract string FileName { get; }

        public void Make(string file, string outFileName)
        {
            this.WriteFile(this.Parse(file), outFileName);
        }

        protected virtual string? Parse(string file)
        {
            return null;
        }

        private void WriteFile(string? parseStr, string outFileName)
        {
            if (parseStr == null) {
                Console.WriteLine("! [Error]: " + this.FileName + " Parse Error\n");
            }

            string fullPath = Path.Combine(outFileName, this.FileName);
            FileStream fs = new FileStream(fullPath, FileMode.Create);
            using (StreamWriter writer = new StreamWriter(fs)) {
                writer.Write(parseStr);
            }
            Console.WriteLine("# [Success]: " + this.FileName + " 생성 완료\n");
        }
    }
}
