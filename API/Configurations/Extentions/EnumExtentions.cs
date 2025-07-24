using System.ComponentModel;

namespace API.Configurations.Extentions;

public static class EnumExtentions
{
    public static string GetDescription<TEnum>(this TEnum value) where TEnum : Enum
    {
        var field = value.GetType().GetField(value.ToString());
        var attr = field?.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .FirstOrDefault() as DescriptionAttribute;
        return attr?.Description ?? value.ToString();
    }
}