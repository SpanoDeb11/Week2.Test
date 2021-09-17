using AcademyG.Week2.Test.Library;
using System;

namespace AcademyG.Week2.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Warehouse warehouse = new Warehouse("Roma");

                Console.WriteLine("Scegliere come si vogliono inserire le merci");
                Console.WriteLine("0 - Input");
                Console.WriteLine("1 - File");
                int.TryParse(Console.ReadLine(), out int choice);

                if (choice == 0)
                {
                    #region INSERIMENTO MERCI DA INPUT UTENTE

                    bool whileBool = true;
                    while (whileBool == true)
                    {
                        Good good;
                        Console.WriteLine("Che tipo di merce si vuole inserire? (inserire il nome senza spazi)");
                        string typeGood = Console.ReadLine().ToUpper();

                        // dati in comune a tutti
                        Console.WriteLine("Inserire codice merce");
                        string code = Console.ReadLine();
                        Console.WriteLine("Inserire la descrizione");
                        string descr = Console.ReadLine();
                        Console.WriteLine("Inserire il prezzo");
                        double.TryParse(Console.ReadLine(), out double amount);
                        Console.WriteLine("Inserire la quantità in giacenza");
                        int.TryParse(Console.ReadLine(), out int quantity);

                        switch (typeGood)
                        {
                            case "ELECTRONICGOOD":
                                Console.WriteLine("Inserire il produttore");
                                string producer = Console.ReadLine();
                                good = new ElectronicGood(code, descr, amount, DateTime.Now, quantity, producer);
                                break;
                            case "PERISHABLEGOOD":
                                Console.WriteLine("Inserire la data di scadenza");
                                DateTime.TryParse(Console.ReadLine(), out DateTime expDate);
                                Console.WriteLine("Inserire il metodo di conservazione");
                                Enum.TryParse(Console.ReadLine().ToUpper(), out StorageMethod method);
                                good = new PerishableGood(code, descr, amount, DateTime.Now, quantity, expDate, method);
                                break;
                            case "SPIRITDRINKGOOD":
                                Console.WriteLine("Inserire il tipo di drink");
                                Enum.TryParse(Console.ReadLine(), out TypeDrinkEnum typeDrink);
                                Console.WriteLine("Inserire la gradazione alcolica");
                                int.TryParse(Console.ReadLine(), out int alcoholCont);
                                good = new SpiritDrinkGood(code, descr, amount, DateTime.Now, quantity, typeDrink, alcoholCont);
                                break;
                            default:
                                good = new Good(code, descr, amount, DateTime.Now, quantity);
                                break;
                        }

                        warehouse += good;
                        Console.WriteLine("Prodotto aggiunto con successo!\n");

                        Console.WriteLine("Vuoi inserire un'altra merce? (Y - yes)");
                        if (!Console.ReadLine().ToUpper().Equals("Y"))
                            whileBool = false;
                    }
                    #endregion
                }
                else if (choice == 1)
                {
                    #region INSERIRE MERCI DA FILE

                    string path = @"C:\Users\debora.spano\Desktop\Week2.Test\AcademyG.Week2.Test\lista_merci.txt";
                    warehouse.UploadFromFile(path);

                    #endregion
                }
                else
                    Console.WriteLine("Errore inserimento valore");

                #region STAMPA DATI DEL MAGAZZINO

                warehouse.StockList();

                #endregion

            }
            catch(WaresException wex)
            {
                Console.WriteLine($"Si è verificato un problema - {wex.Message}");
            }
        }
    }
}
