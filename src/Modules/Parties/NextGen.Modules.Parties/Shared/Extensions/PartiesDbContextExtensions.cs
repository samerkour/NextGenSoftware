using NextGen.Modules.Parties.Parties.Models;
using NextGen.Modules.Parties.Parties.ValueObjects;
using NextGen.Modules.Parties.Shared.Data;
using Microsoft.EntityFrameworkCore;

namespace NextGen.Modules.Parties.Shared.Extensions;

public static class PartiesDbContextExtensions
{
    public static ValueTask<Party?> FindPartyByIdAsync(this PartiesDbContext context, PartyId id)
    {
        return context.Parties.FindAsync(id);
    }

    public static Task<bool> ExistsPartyByIdAsync(this PartiesDbContext context, PartyId id)
    {
        return context.Parties.AnyAsync(x => x.Id == id);
    }
}
