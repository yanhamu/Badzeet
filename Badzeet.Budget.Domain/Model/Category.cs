namespace Badzeet.Budget.Domain.Model;

public class Category
{
    public long Id { get; set; }
    public long AccountId { get; set; }
    public Account Account { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Order { get; set; }
    public bool DisplayInSummary { get; set; }
}