using System.ComponentModel.DataAnnotations;
namespace SharpSeer.Services;

public static class Extensions
{
    public static string GetDisplayName(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            var attribute = field.GetCustomAttributes(typeof(DisplayAttribute), false)
                                 .FirstOrDefault() as DisplayAttribute;

            return attribute?.Name ?? value.ToString();
        }
}
