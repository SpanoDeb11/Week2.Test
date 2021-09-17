using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week2.Test.Library
{
    public class ElectronicGood : Good
    {
        public string Producer { get; set; }

        public ElectronicGood() { }

        public ElectronicGood(string code, string descr, double amount, DateTime recDate, 
                              int quantity, string producer)
                              : base (code, descr, amount, recDate, quantity)
        {
            // validazione
            if (string.IsNullOrEmpty(producer))
                throw new WaresException("Errore! Stringa del produttore vuota o nulla");

            Producer = producer;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("\n\tElectronic good");
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Producer: {Producer}");

            return sb.ToString();
        }
    }
}
