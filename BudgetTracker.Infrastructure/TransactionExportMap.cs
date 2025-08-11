using BudgetTracker.Application.Models.Transaction;
using BudgetTracker.Domain.Models.Transaction;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace BudgetTracker.Infrastructure;

public sealed class TransactionExportMap : ClassMap<Transaction>
{
    
    public TransactionExportMap()
    {
        Map(t=> t.CreatedAt).Index(0).Name("Date");
        Map(t=> t.Amount).Index(1).Name("Amount").TypeConverter<MoneyToStringConverter>();
        Map(t=>t.Category.Name).Index(2).Name("Category");
        Map(t=>t.Description).Index(3).Name("Description").TypeConverter<NullToNaConverter>();
        Map(t=>t.PaymentMethod.Name).Index(4).Name("Payment Method").TypeConverter<NullToNaConverter>();
        Map(t=>t.TransactionTags).Index(5).Name("Tags").TypeConverter<TagsConverter>();
    }
}

public class MoneyToStringConverter : DefaultTypeConverter
{
    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is Money money)
        {
            return $"{money.Amount} {money.Currency}";
        }

        return "N/A";
    }
}
public class NullToNaConverter : DefaultTypeConverter
{
    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        return value == null ? "N/A" : base.ConvertToString(value, row, memberMapData);
    }
}

public class TagsConverter:DefaultTypeConverter
{
    public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
    {
        if (value is List<TransactionTag> tags && tags.Count > 0)
        {
            return string.Join(",", tags.Select(t=> t.Tag.Name));
        }

        return "N/A";
    }
}