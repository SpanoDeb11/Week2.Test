using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AcademyG.Week2.Test.Library
{
    public enum TypeDrinkEnum
    {
        WHISKY,
        WODKA,
        GRAPPA,
        GIN,
        OTHER
    }

    public class SpiritDrinkGood : Good
    {
        public TypeDrinkEnum TypeDrink { get; set; } 
        public double AlcoholContent { get; set; }

        public SpiritDrinkGood() { }

        public SpiritDrinkGood(string code, string descr, double amount, DateTime recDate, 
                               int quantity, TypeDrinkEnum typeDrink, double alcoholCont) 
                               : base(code, descr, amount, recDate, quantity) 
        {
            // validazione
            if (alcoholCont < 0 || alcoholCont > 100) // considero la gradazione alcolica in percentuale
                throw new WaresException("Errore! Gradazione alcolica non valida");

            TypeDrink = typeDrink;
            AlcoholContent = alcoholCont;
        }

        public override string ToString()
        {
            StringBuilder sb = new();
            sb.AppendLine("\n\tSpirit drink good");
            sb.AppendLine(base.ToString());
            sb.AppendLine($"Type drink: {TypeDrink} - Alcohol content: {AlcoholContent}");

            return sb.ToString();
        }
    }
}
