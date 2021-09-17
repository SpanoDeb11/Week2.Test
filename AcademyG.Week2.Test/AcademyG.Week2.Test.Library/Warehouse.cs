using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AcademyG.Week2.Test.Library
{
    public class Warehouse : IEnumerable<Good>
    {
        public event EventHandler ReadStart;
        public event EventHandler<ReadEventArgs> ReadProgress;
        public event EventHandler<ReadEventArgs> ReadCompleted;

        public Guid Id { get; set; }
        public string Address { get; set; }
        public double TotAmount { get; set; }
        public DateTime LastOpDate { get; set; }
        private List<Good> _goods;

        public Warehouse()
        {
            this._goods = new();
            this.LastOpDate = DateTime.Now;
            this.TotAmount = 0.0;
            Id = Guid.NewGuid();
        }

        public Warehouse(string address) : this()
        {
            if (string.IsNullOrEmpty(address))
                throw new WaresException("Errore! Stringa nulla o vuota");
            Address = address;
        }

        #region OVERLOAD DI + E -

        public static Warehouse operator +(Warehouse warehouse, Good good)
        {
            if (good.Equals(default(Good))) // se l'oggetto è nullo lancio un eccezione
                throw new WaresException("Errore! La merce che si vuole aggiungere risulta nulla");

            warehouse.LastOpDate = DateTime.Now;
            warehouse.TotAmount += good.Amount;

            warehouse._goods.Add(good);

            return warehouse; // ritorno l'istanza aggiornata
        }

        public static Warehouse operator -(Warehouse warehouse, Good good)
        {
            if (good.Equals(default(Good))) // se l'oggetto è nullo lancio un eccezione
                throw new WaresException("Errore! La merce che si vuole rimuovere risulta nulla");

            warehouse.LastOpDate = DateTime.Now;
            warehouse.TotAmount -= good.Amount;

            if (!warehouse._goods.Remove(good)) // se Remove restituisce false (quindi se l'oggetto non è presente nella lista) lancio un eccezione
                throw new WaresException("Errore! La merce che si vuole rimuovere non è presente in magazzino", good);

            return warehouse;
        }

        #endregion

        #region METODO DI STAMPA DEI DETTAGLI

        public void StockList()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("__________________________________________________________");
            sb.AppendLine(this.ToString());
            sb.AppendLine($"Last operation: {LastOpDate.ToShortDateString()}");
            sb.AppendLine($"Total amount of goods: {TotAmount}");
            sb.AppendLine("\tList of goods:");
            foreach (var g in _goods)
            {
                sb.AppendLine("-------------");
                sb.AppendLine("\t" + g.ToString());
                sb.AppendLine("-------------");
            }

            sb.AppendLine("__________________________________________________________");
            Console.WriteLine(sb);
         }

        public override string ToString()
        {
            return $"\nMagazzino: {Id} - Via {Address}";
        }

        #endregion

        #region METODI DI IENUMERABLE

        public IEnumerator<Good> GetEnumerator()
        {
            foreach (var w in this._goods)
                yield return w;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region UPLOAD DA FILE

        public void UploadFromFile(string path)
        {
            try
            {
                using StreamReader reader = File.OpenText(path);

                if (ReadStart != null) // evento di inizio lettura
                    ReadStart(this, EventArgs.Empty);

                reader.ReadLine(); // intestazione

                // formattazione file
                // Code;Description;Amount;ReceiptDate;Quantity;TypeGood;
                // dove TypeGood indica la classe del prodotto
                // in base al tipo di prodotto ci saranno anche i parametri di quella classe
                // Electronic avrà dopo TypeGood - Producer
                // Perishable avrà dopo TypeGood - ExpirationDate;Method
                // Spirit drink avrà dopo TypeGood - TypeDrink;AlcoholContent

                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    Good good = null;
                    string[] values = line.Split(";");

                    // provo a convertire i dati in comune con tutte le classi
                    double.TryParse(values[2], out double amount);
                    DateTime.TryParse(values[3], out DateTime recDate);
                    int.TryParse(values[4], out int quantity);

                    switch (values[5].ToUpper())
                    {
                        case "ELECTRONICGOOD":
                            if (values.Length == 7) // mi assicuro che nella riga ci siano tutti i dati che mi servono
                                good = new ElectronicGood(values[0], values[1], amount, recDate, quantity, values[6]);
                            else
                                throw new WaresException("Errore! Riga file non valida");
                            break;
                        case "PERISHABLEGOOD":
                            if (values.Length == 8)
                            {
                                DateTime.TryParse(values[6], out DateTime expDate);
                                Enum.TryParse(values[7], out StorageMethod method);
                                good = new PerishableGood(values[0], values[1], amount, recDate, quantity, expDate, method);
                            }
                            else
                                throw new WaresException("Errore! Riga file non valida");
                            break;
                        case "SPIRITDRINKGOOD":
                            if (values.Length == 8)
                            {
                                Enum.TryParse(values[6], out TypeDrinkEnum type);
                                double.TryParse(values[7], out double alcohol);
                                good = new SpiritDrinkGood(values[0], values[1], amount, recDate, quantity, type, alcohol);
                            }
                            else
                                throw new WaresException("Errore! Riga file non valida");
                            break;
                        default:
                            if (values.Length == 6)
                                good = new Good(values[0], values[1], amount, recDate, quantity);
                            else
                                throw new WaresException("Errore! Riga file non valida");
                            break;
                    }

                    TotAmount += good.Amount;
                    _goods.Add(good);

                    if (ReadProgress != null) // evento dopo la lettura di ogni prodotto
                        ReadProgress(this, new ReadEventArgs
                                           {
                                               Item = good,
                                               WarehouseId = this.Id
                                           });
                }
                if (ReadCompleted != null)
                    ReadCompleted(this, new ReadEventArgs
                                        {
                                            WarehouseId = this.Id
                                        });

            }
            catch (IOException ioe)
            {
                Console.WriteLine($"Errore IO durante la lettura del file: {ioe.Message}");
            }
            catch (Exception ex)
            {
                throw new WaresException($"Errore generico - {ex.Message}");
            }
        }

        #endregion
    }
}
