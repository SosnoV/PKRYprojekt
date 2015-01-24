using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PkryServ
{
    class Program
    {
        static void Main(string[] args)
        {
            Server ser = new Server("localhost");
            ser.Run();
        }
    }
}
