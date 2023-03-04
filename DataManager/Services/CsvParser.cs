using System.Globalization;
using DataManager.Models;

namespace DataManager.Services;

public static class CsvParser
{
    public static IEnumerable<Transaction> Parse(string filePath)
    {
        return File.ReadAllLines(filePath)
            .Skip(1)
            .Select(line => line.Split(";")).ToList()
            .Select(lines => lines.Select(line => line.Replace("\"", "")).ToList())
            .Select(item => new Transaction
            {
                Date = ParseDate(item[0]),
                Name = item[1],
                Account = item[2],
                Counterparty = item[3],
                DebitCredit = ChooseCorrectType(item[5]),
                Amount = float.Parse(item[6], NumberStyles.Currency),
                TransactionType = item[7],
                Notifications = item[8],
                CategorySelection = CategorySelection.HandFill
            });
    }
    
    private static TransactionType ChooseCorrectType(string type)
    {
        return type switch
        {
            "Debit" => TransactionType.Expense,
            "Credit" => TransactionType.Income,
            var _ => throw new Exception("Unlimited SADNESS")
        };
    }
    
    private static DateTime ParseDate(string date)
    {
        var year = int.Parse(date.Substring(0, 4));
        var month= int.Parse(date.Substring(4, 2));
        var day = int.Parse(date.Substring(6, 2));
        return new DateTime(year, month, day);
    }
}