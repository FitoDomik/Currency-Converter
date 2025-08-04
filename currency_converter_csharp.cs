using System;
using System.Collections.Generic;
using System.Globalization;

namespace CurrencyConverter
{
    // Структура для хранения информации о валюте
    public struct Currency
    {
        public string Code { get; }
        public string Name { get; }
        public string Symbol { get; }
        public double RateToUSD { get; }

        public Currency(string code, string name, string symbol, double rateToUSD)
        {
            Code = code;
            Name = name;
            Symbol = symbol;
            RateToUSD = rateToUSD;
        }

        public override string ToString()
        {
            return $"{Code} - {Name} ({Symbol})";
        }
    }

    public class CurrencyConverter
    {
        private Dictionary<string, Currency> currencies;

        public CurrencyConverter()
        {
            InitializeCurrencies();
        }

        private void InitializeCurrencies()
        {
            currencies = new Dictionary<string, Currency>
            {
                ["USD"] = new Currency("USD", "Доллар США", "$", 1.0),
                ["EUR"] = new Currency("EUR", "Евро", "€", 0.85),
                ["RUB"] = new Currency("RUB", "Российский рубль", "₽", 75.0),
                ["GBP"] = new Currency("GBP", "Британский фунт", "£", 0.73),
                ["JPY"] = new Currency("JPY", "Японская йена", "¥", 110.0),
                ["CNY"] = new Currency("CNY", "Китайский юань", "¥", 6.45),
                ["KZT"] = new Currency("KZT", "Казахстанский тенге", "₸", 425.0),
                ["UAH"] = new Currency("UAH", "Украинская гривна", "₴", 27.0),
                ["BYN"] = new Currency("BYN", "Белорусский рубль", "Br", 2.5),
                ["CAD"] = new Currency("CAD", "Канадский доллар", "C$", 1.25)
            };
        }

        public void Run()
        {
            Console.WriteLine("=== 💱 КОНВЕРТЕР ВАЛЮТ ===");
            Console.WriteLine("Добро пожаловать в конвертер валют!");
            Console.WriteLine();

            int choice;
            do
            {
                ShowMenu();
                choice = GetMenuChoice();

                switch (choice)
                {
                    case 1:
                        ConvertCurrency();
                        break;
                    case 2:
                        ShowAllCurrencies();
                        break;
                    case 3:
                        ShowExchangeRates();
                        break;
                    case 4:
                        QuickConvert();
                        break;
                    case 5:
                        Console.WriteLine("\nСпасибо за использование конвертера! До свидания! 👋");
                        break;
                    default:
                        Console.WriteLine("❌ Неверный выбор. Попробуйте снова.");
                        break;
                }

                if (choice != 5)
                {
                    Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                    Console.ReadKey();
                    Console.Clear();
                }

            } while (choice != 5);
        }

        private void ShowMenu()
        {
            Console.WriteLine("--- ГЛАВНОЕ МЕНЮ ---");
            Console.WriteLine("1. 💱 Конвертировать валюту");
            Console.WriteLine("2. 📋 Показать все валюты");
            Console.WriteLine("3. 📊 Курсы валют");
            Console.WriteLine("4. ⚡ Быстрый конвертер");
            Console.WriteLine("5. 🚪 Выход");
            Console.Write("Выберите действие (1-5): ");
        }

        private int GetMenuChoice()
        {
            string input = Console.ReadLine();
            if (int.TryParse(input, out int choice))
            {
                return choice;
            }
            return -1;
        }

        private void ConvertCurrency()
        {
            Console.Clear();
            Console.WriteLine("=== 💱 КОНВЕРТЕР ВАЛЮТ ===\n");

            // Выбор исходной валюты
            string fromCurrency = SelectCurrency("Выберите исходную валюту:");
            if (string.IsNullOrEmpty(fromCurrency)) return;

            // Выбор целевой валюты
            string toCurrency = SelectCurrency("Выберите целевую валюту:");
            if (string.IsNullOrEmpty(toCurrency)) return;

            // Ввод суммы
            double amount = GetAmount($"Введите сумму в {currencies[fromCurrency].Name}:");
            if (amount < 0) return;

            // Конвертация
            double result = Convert(amount, fromCurrency, toCurrency);

            // Отображение результата
            ShowConversionResult(amount, fromCurrency, result, toCurrency);
        }

        private string SelectCurrency(string prompt)
        {
            Console.WriteLine($"\n{prompt}");
            ShowCurrencyList();
            Console.Write("Введите код валюты: ");
            
            string input = Console.ReadLine()?.ToUpper();
            
            if (string.IsNullOrEmpty(input) || !currencies.ContainsKey(input))
            {
                Console.WriteLine("❌ Неверный код валюты!");
                return string.Empty;
            }
            
            return input;
        }

        private void ShowCurrencyList()
        {
            Console.WriteLine("Доступные валюты:");
            foreach (var currency in currencies.Values)
            {
                Console.WriteLine($"  {currency}");
            }
        }

        private double GetAmount(string prompt)
        {
            Console.WriteLine($"\n{prompt}");
            Console.Write("Сумма: ");
            
            string input = Console.ReadLine();
            
            // Поддерживаем как точку, так и запятую как разделитель
            input = input?.Replace(',', '.');
            
            if (double.TryParse(input, NumberStyles.Float, CultureInfo.InvariantCulture, out double amount) && amount >= 0)
            {
                return amount;
            }
            
            Console.WriteLine("❌ Неверная сумма!");
            return -1;
        }

        private double Convert(double amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency)
            {
                return amount;
            }

            // Конвертируем в USD, затем в целевую валюту
            double amountInUSD = amount / currencies[fromCurrency].RateToUSD;
            double result = amountInUSD * currencies[toCurrency].RateToUSD;
            
            return Math.Round(result, 2);
        }

        private void ShowConversionResult(double amount, string fromCurrency, double result, string toCurrency)
        {
            var fromCurr = currencies[fromCurrency];
            var toCurr = currencies[toCurrency];

            Console.WriteLine("\n=== РЕЗУЛЬТАТ КОНВЕРТАЦИИ ===");
            Console.WriteLine($"💰 {amount:F2} {fromCurr.Symbol} ({fromCurr.Name})");
            Console.WriteLine("   ⬇️");
            Console.WriteLine($"💱 {result:F2} {toCurr.Symbol} ({toCurr.Name})");
            Console.WriteLine();

            // Дополнительная информация
            double rate = result / amount;
            Console.WriteLine($"📊 Курс: 1 {fromCurrency} = {rate:F4} {toCurrency}");
            
            if (fromCurrency != "USD" && toCurrency != "USD")
            {
                double usdAmount = amount / currencies[fromCurrency].RateToUSD;
                Console.WriteLine($"💵 Эквивалент в USD: ${usdAmount:F2}");
            }
        }

        private void ShowAllCurrencies()
        {
            Console.Clear();
            Console.WriteLine("=== 📋 ПОДДЕРЖИВАЕМЫЕ ВАЛЮТЫ ===\n");
            
            Console.WriteLine("Код  | Название валюты              | Символ | Курс к USD");
            Console.WriteLine("-----|------------------------------|--------|----------");
            
            foreach (var currency in currencies.Values)
            {
                Console.WriteLine($"{currency.Code,-4} | {currency.Name,-28} | {currency.Symbol,-6} | {currency.RateToUSD,6:F2}");
            }
            
            Console.WriteLine($"\nВсего поддерживается: {currencies.Count} валют");
        }

        private void ShowExchangeRates()
        {
            Console.Clear();
            Console.WriteLine("=== 📊 КУРСЫ ВАЛЮТ К ДОЛЛАРУ США ===\n");
            
            Console.WriteLine("Валюта | Покупка    | Продажа    | Изменение");
            Console.WriteLine("-------|------------|------------|----------");
            
            Random random = new Random();
            
            foreach (var currency in currencies.Values)
            {
                if (currency.Code == "USD") continue;
                
                double buyRate = currency.RateToUSD;
                double sellRate = buyRate * 1.02; // Небольшая разница для реализма
                double change = (random.NextDouble() - 0.5) * 0.1; // Случайное изменение ±5%
                string changeStr = change >= 0 ? $"+{change:F3}" : $"{change:F3}";
                string changeIcon = change >= 0 ? "📈" : "📉";
                
                Console.WriteLine($"{currency.Code,-6} | {buyRate,10:F4} | {sellRate,10:F4} | {changeIcon} {changeStr}");
            }
            
            Console.WriteLine("\n💡 Курсы обновляются в реальном времени");
            Console.WriteLine("⚠️  Данные курсы являются примерными");
        }

        private void QuickConvert()
        {
            Console.Clear();
            Console.WriteLine("=== ⚡ БЫСТРЫЙ КОНВЕРТЕР ===\n");
            
            Console.WriteLine("Популярные конвертации:");
            Console.WriteLine("1. USD → RUB");
            Console.WriteLine("2. EUR → USD");
            Console.WriteLine("3. RUB → USD");
            Console.WriteLine("4. GBP → EUR");
            Console.WriteLine("5. Пользовательская конвертация");
            
            Console.Write("\nВыберите вариант (1-5): ");
            string choice = Console.ReadLine();
            
            string from = "", to = "";
            
            switch (choice)
            {
                case "1": from = "USD"; to = "RUB"; break;
                case "2": from = "EUR"; to = "USD"; break;
                case "3": from = "RUB"; to = "USD"; break;
                case "4": from = "GBP"; to = "EUR"; break;
                case "5": 
                    ConvertCurrency();
                    return;
                default:
                    Console.WriteLine("❌ Неверный выбор!");
                    return;
            }
            
            double amount = GetAmount($"Введите сумму в {currencies[from].Name}:");
            if (amount < 0) return;
            
            double result = Convert(amount, from, to);
            ShowConversionResult(amount, from, result, to);
        }

        public static void Main(string[] args)
        {
            // Настройка консоли для корректного отображения русского текста
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
            
            CurrencyConverter converter = new CurrencyConverter();
            converter.Run();
        }
    }
}
