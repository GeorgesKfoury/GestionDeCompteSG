using GestionDeCompteSG.Application;
using GestionDeCompteSG.Domain;
using GestionDeCompteSG.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace GestionDeCompteSG.Persistence
{
    public class Repository : IRepository
    {
        public SortedList<DateOnly, List<TransactionUnitaire>> SortedTransactions { get; set; } = new SortedList<DateOnly, List<TransactionUnitaire>>();
        public SortedDictionary<DateOnly, float> SoldesComptes { get; set; } = new SortedDictionary<DateOnly, float>();
        public Dictionary<string, float> TransactionsDeDebitParCategories { get; set; } = new Dictionary<string, float>();
        public SoldeCompte CompteDeReference { get; set; } = new SoldeCompte() { Date = DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd")), Amount = 0.0f };

        Task<IEnumerable<string>> IRepository.GetCategoriesPlusDebitantesAsync(int nombre)
        {
            
            return Task.FromResult(TransactionsDeDebitParCategories.OrderBy(kvp => kvp.Value)
                .Take((nombre > TransactionsDeDebitParCategories.Count) ? TransactionsDeDebitParCategories.Count : nombre)
                .Select(x => x.Key));
        }

        Task<float> IRepository.GetCompteAsync(DateOnly date)
        {
            if (!((date.CompareTo(DateOnly.Parse("2022-01-01")) >= 0) && (date.CompareTo(DateOnly.Parse("2023-03-01")) <= 0)))
            {
                throw new ArgumentException("Date must be between 2022-01-01 and 2023-03-01");
            }
            else if (date.CompareTo(DateOnly.Parse("2023-03-01")) == 0)
            {
                return Task.FromResult(CompteDeReference.Amount);
            }
            else
            {
                float result = SoldesComptes.ContainsKey(date) ? SoldesComptes[date] : SoldesComptes.First(t => t.Key.CompareTo(date) > 0).Value;
                return Task.FromResult(result);
            }
        }

        Task<bool> IRepository.TraiteCsvAsync(string path)
        {
            try 
            { 
                using (StreamReader reader = new StreamReader(path))
                {
                    string compteLine = reader.ReadLine();
                    string line = string.Empty;
                    while (((line = reader.ReadLine()) != null) && (!(line.StartsWith("Date"))))
                    {
                        Tools.SetExchangeRates(line);
                    }
                    CompteDeReference = Tools.ParseCompteActuel(compteLine);
                    SoldesComptes.Add(CompteDeReference.Date, CompteDeReference.Amount);
                    string[] parts;
                    while ((line = reader.ReadLine()) != null)
                    {
                        parts = line.Split(new char[] { ',', ';' });

                        var tu = new TransactionUnitaire()
                        {
                            Date = DateOnly.Parse(parts[0]),
                            Montant = float.Parse(parts[1], CultureInfo.InvariantCulture),
                            Devise = Enum.Parse<Devise>(parts[2], true),
                            Categorie = parts[3]
                        };
                        
                        if (SortedTransactions.ContainsKey(tu.Date))
                        {
                            SortedTransactions[tu.Date].Add(tu);
                        }
                        else
                        {
                            SortedTransactions.Add(tu.Date, new List<TransactionUnitaire> { tu });
                        }
                        
                        Tools.HandleUpdateCategories(tu, TransactionsDeDebitParCategories);
                        
                    }
                    Tools.HandleProcessingAccount(SortedTransactions, SoldesComptes);
                    Console.WriteLine(string.Join(Environment.NewLine, SoldesComptes.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
                    Console.WriteLine(string.Join(Environment.NewLine, TransactionsDeDebitParCategories.Select(kvp => $"{kvp.Key}: {kvp.Value}")));
                    
                    return Task.FromResult(true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occurred while processing CSV: {e.Message}");
                throw;
            }
        }
    }
}
