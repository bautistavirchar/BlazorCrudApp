using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace BlazorCrudApp.Shared;

public static class StringHelpers
{
	public static bool IsEmpty(this string value) =>
		string.Equals(value, string.Empty, StringComparison.Ordinal) || string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

	public static bool IsNotEmpty(this string? value) => !value!.IsEmpty();

	public static DateTime ToDateTime(this DateOnly value, string timeOnly = "00:00:00") => value.ToDateTime(TimeOnly.Parse(timeOnly));
	public static DateOnly ToDateOnly(this DateTime value) => DateOnly.FromDateTime(value);
	public static DateTime ToDateFromUtc(this DateTime value, string timezone = "Asia/Manila")
	{
		if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			timezone = TZConvert.IanaToWindows(timezone);

		var infoTime = TZConvert.GetTimeZoneInfo(timezone);
		return TimeZoneInfo.ConvertTimeFromUtc(value, infoTime);
	}
	public static string ToDateWithFullTime(this DateTime value, string timezone = "Asia/Singapore") =>
		value.ToDateFromUtc(timezone).ToString("MMM dd, yyyy hh:mm:ss tt");
	public static string ToDate(this DateTime value, string timezone = "Asia/Singapore") =>
		value.ToDateFromUtc(timezone).ToString("MMM dd, yyyy");
	public static string ToDate(this DateOnly value) => value.ToString("MMM dd, yyyy");
	public static string GetName<TEnum>(this TEnum tEnum) => Enum.GetName(typeof(TEnum), tEnum!)!;

	public static void Dump(this object value, bool writeIndented = true)
	{
		var options = new JsonSerializerOptions
		{
			WriteIndented = writeIndented
		};

		var serialized = JsonSerializer.Serialize(value, options);
		Console.WriteLine(serialized);
	}
	public static T ToEnum<T>(this string value, bool ignoreCase = true) =>
		(T)Enum.Parse(typeof(T), value, ignoreCase);

	public static T ToEnum<T>(this int value) =>
		(T)Enum.ToObject(typeof(T), value);
}
