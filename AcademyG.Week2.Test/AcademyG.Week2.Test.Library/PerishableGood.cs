using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week2.Test.Library
{
    public enum StorageMethod
    {
        FREEZER,
        FRIDGE,
        SELF
    }
    public class PerishableGood : Good
    {
        public DateTime ExpirationDate { get; set; }
        public StorageMethod Method { get; set; }

        public PerishableGood() { }

        public PerishableGood(string code, string descr, double amount, DateTime recDate, 
                              int quantity, DateTime expDate, StorageMethod method)
                              : base (code, descr, amount, recDate, quantity)
        {
            // validazione
            if (expDate < DateTime.Now)
                throw new WaresException("Il prodotto che si vuole aggiungere è scaduto");

            ExpirationDate = expDate;
            Method = method;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("\n\tPerishable good");
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Storage method: {Method} - Expiration date: {ExpirationDate}");

            return sb.ToString();
        }
    }
}
