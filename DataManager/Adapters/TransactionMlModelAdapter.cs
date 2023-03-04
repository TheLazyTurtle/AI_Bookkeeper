using DataManager.Models;
using ML_algo;

namespace DataManager.Adapters;

public class TransactionMlModelAdapter
{
    public static MLModel2.ModelInput Adapt(Transaction transaction)
    {
        return new MLModel2.ModelInput
        {
            Date = transaction.Date.ToShortDateString(),
            Name = transaction.Name,
            Account = transaction.Account,
            CounterParty = transaction.Counterparty,
            DebitCredit = (int)transaction.DebitCredit == 1,
            Amount = transaction.Amount,
            TransactionType = transaction.TransactionType,
            Notification = transaction.Notifications,
            CategoryId = transaction.CategoryId ?? 0
        };
    }
}