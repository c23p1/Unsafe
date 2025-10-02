using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Domain.Extensions;

public static class EnumExtensions
{
	public static string GetDisplayName(this Enum enumValue)
	{
		if (enumValue is null)
			throw new ArgumentNullException(nameof(enumValue));
		var memberInfo = enumValue.GetType()
			.GetMember(enumValue.ToString())
			.FirstOrDefault();
		if (memberInfo is null)
			return enumValue.ToString();
		var displayAttribute = memberInfo.GetCustomAttribute<DisplayAttribute>();
		return displayAttribute?.GetName() ?? enumValue.ToString();
	}
}