using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonyvtarAsztali
{
    internal interface IStatisztika
    {
        void AdatbazisbolTorles(int torlendoId);
        bool Beolvas(string databaseName);
        void ConsoleFuttat();
    }
}
