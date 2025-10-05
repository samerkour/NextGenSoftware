using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlocks.Core.Exception.Types;

namespace NextGen.Modules.Identity.Users.Exceptions;

public class DuplicateUserException : BadRequestException
{
    public string Property { get; }
    public string Value { get; }

    public DuplicateUserException(string property, string value)
        : base($"Duplicate value detected. {property} '{value}' is already in use.")
    {
        Property = property;
        Value = value;
    }
}
