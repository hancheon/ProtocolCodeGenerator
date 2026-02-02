using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolCodeGenerator
{
    internal class MakeFile
    {
        public MakeFile() { }

        public void Make(string file, string outFileName)
        {
            this.WriteFile(this.Parse(file), outFileName);
        }

        protected virtual string? Parse(string file)
        {
            return null;
        }

        private void WriteFile(string parseStr, string outFileName)
        {
            FileStream fs = new FileStream(outFileName, FileMode.Create);
            StreamWriter writer = new StreamWriter(fs);
            writer.Write(parseStr);
            writer.Close();
        }
    }
}
