using System.ComponentModel;

namespace Common.Enums;

public enum ERoles
{
    [Description("admin")] Admin = 1,
    [Description("client")] Client = 2,
    [Description("audience")] Audience = 3
}