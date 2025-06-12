using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GestionDeCompteSG.Domain;
using GestionDeCompteSG.Domain.Enums;

namespace GestionDeCompteSG.Application
{
    public static class Tools
    {   
        public static SoldeCompte ParseCompteActuel(string line)
        {
            try
            {
                if (line.StartsWith("Compte au "))
                {
                    var datePart = line.Substring(10, line.IndexOf(' ', 10) - 10);
                    var amountString = line.Substring(line.IndexOf(": ") + 2).Trim();
                    string[] amountAndCurrencyPart = amountString.Split(' ');
                    float amountPart = float.Parse(amountAndCurrencyPart[0], CultureInfo.InvariantCulture);
                    string currencyPart = amountAndCurrencyPart[1];
                    Devise devise = Enum.Parse<Devise>(currencyPart, true);
                    return new SoldeCompte()
                    {
                        Date = DateOnly.Parse(datePart),
                        Amount = (devise == Devise.EUR) ? amountPart : ConvertToEuros(amountPart, devise)
                    };
                }
                else
                {
                    Console.WriteLine("\"Compte au\" expected at the beginning of the line");
                    throw new IOException("\"Compte au\" expected at the beginning of the line");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured : {e.Message}");
                throw;
            }

        }

        public static float ConvertToEuros(float amount, Devise devise)
        {
            switch (devise)
            {
                case Devise.USD:
                    return amount / ExchangeRate.DollarToEuro;
                case Devise.JPY:
                    return amount / ExchangeRate.YenToEuro;
                default:
                    return amount;
            }
        }

        public static void SetExchangeRates(string line)
        {
            try
            {
                string taux = line.Substring(0, line.IndexOf(" :")).Trim();
                var splitted = line.Split(' ')[2];
                float valeur = float.Parse(line.Split(' ')[2], CultureInfo.InvariantCulture);
                switch (taux)
                {
                    case "EUR/JPY":
                        ExchangeRate.YenToEuro = valeur;
                        break;
                    case "EUR/USD":
                        ExchangeRate.DollarToEuro = valeur;
                        break;
                    case "JPY/EUR":
                        ExchangeRate.YenToEuro = 1 / valeur;
                        break;
                    case "USD/EUR":
                        ExchangeRate.DollarToEuro = 1 / valeur;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error parsing exchange rate: {ex.Message}");
                throw;
            }
        }

       
        public static bool HandleUpdateCategories(TransactionUnitaire tu, Dictionary<string, float> categories)
        {
            
            if (categories.ContainsKey(tu.Categorie))
            {
                categories[tu.Categorie] += ConvertToEuros(tu.Montant, tu.Devise);
            }
            else
            {
                categories.Add(tu.Categorie, ConvertToEuros(tu.Montant, tu.Devise));
            }
            return true;           
        }

        public static void HandleProcessingAccount(SortedList<DateOnly, List<TransactionUnitaire>>  SortedTransactions, SortedDictionary<DateOnly, float>  SoldesComptes)
        {
            try
            {
                
                foreach (var item in SortedTransactions.Reverse())
                {
                    
                    var total = item.Value.Sum(s => ConvertToEuros(s.Montant, s.Devise));
                    if (!(SoldesComptes.ContainsKey(item.Key.AddDays(-1))))
                    {
                        SoldesComptes[item.Key.AddDays(-1)] = SoldesComptes.First().Value - total;
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception occured : " + e.Message);
                throw;
            }
        }

    }
}
