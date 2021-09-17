using System;
using System.Text;

namespace AcademyG.Week2.Test.Library
{
    public class Good
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public double Amount { get; set; }
        public DateTime ReceiptDate { get; set; }
        public int QuantityStock { get; set; }

        public Good() 
        {
            QuantityStock++;
        }

        public Good(string code, string descr, double amount, DateTime recDate, int quantity)
        {
            // validazione dati
            if (string.IsNullOrEmpty(code) || string.IsNullOrEmpty(descr))
                throw new WaresException("Errore! Uno o più parametri stringa sono nulli o vuoti");
            if(amount <= 0)
                throw new WaresException("Errore! Importo prodotto non valido");
            if (recDate > DateTime.Now)
                throw new WaresException("Errore! Data non valida");
            if (quantity <= 0)
                throw new WaresException("Errore! Quantità uguale o inferiore a zero");

            Code = code;
            Description = descr;
            Amount = amount;
            ReceiptDate = recDate;
            QuantityStock = quantity;
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            sb.AppendLine($"Good with code: {Code}");
            sb.AppendLine($"Description: {Description} - Amount: {Amount} - Quantity: {QuantityStock}");
            sb.AppendLine($"Receipt date: {ReceiptDate}");

            return sb.ToString();
        }
    }
}