namespace NextGen.Modules.Parties.RestockSubscriptions.Dtos;

public class RestockSubscriptionReadDto
{
    public string PartyName { get; init; } = null!;
    public string PartyId { get; init; } = null!;
    public string ProductId { get; init; } = null!;
    public string ProductName { get; init; } = null!;
    public bool Processed { get; init; }
    public DateTime? ProcessedTime { get; init; }
}
