using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KonyvtarAsztali
{
    internal class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            Statisztika stat = new Statisztika();

            if (args.Length > 0 && args[0] == "--stat")
            {
                stat.ConsoleFuttat();
            }
            else
            {
                var app = new App();
                app.Run(new konyvtar());
            }
        }
    }
}
