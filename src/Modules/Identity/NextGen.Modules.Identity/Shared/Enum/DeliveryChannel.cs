using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NextGen.Modules.Identity.Shared.Enum;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum DeliveryChannel
{
    [EnumMember(Value = "Email")]
    Email,

    [EnumMember(Value = "Phone")]
    Phone
}
