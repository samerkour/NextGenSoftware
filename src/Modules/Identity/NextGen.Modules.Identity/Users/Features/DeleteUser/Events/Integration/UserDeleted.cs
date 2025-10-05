using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Abstractions.CQRS.Event.Internal;

namespace NextGen.Modules.Identity.Users.Features.DeleteUser.Events.Integration;

public record UserDeleted(Guid UserId, DateTime? DeletedOn);
