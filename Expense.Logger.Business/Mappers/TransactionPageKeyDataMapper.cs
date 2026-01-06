using Expense.Logger.Business.Models.Transaction;
using System.Text;

namespace Expense.Logger.Business.Mappers;

public static class TransactionPageKeyDataMapper
{
    public static string ToStringKey(this TransactionQueryPageData pageKeyData)
    {
        string keyString = $"{pageKeyData.NextDate};{pageKeyData.NextTransactionId}";
        byte[] keyBytes = Encoding.UTF8.GetBytes(keyString);
        return Convert.ToBase64String(keyBytes);
    }

    public static TransactionQueryPageData ToTransactionPageKeyData(this string pageKey)
    {
        byte[] keyBytes = Convert.FromBase64String(pageKey);
        string keyString = Encoding.UTF8.GetString(keyBytes);

        string[] parts = keyString.Split(';');

        if (parts.Length != 2)
            throw new FormatException("Invalid page key format.");

        if (!DateTime.TryParse(parts[0], out var nextDate) ||
            !long.TryParse(parts[1], out long nextTransactionId))
            throw new FormatException("Invalid page key format.");

        return new TransactionQueryPageData
        {
            NextDate = nextDate,
            NextTransactionId = nextTransactionId
        };
    }
}
