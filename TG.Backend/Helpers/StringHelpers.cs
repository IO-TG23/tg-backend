namespace TG.Backend.Helpers;

public static class StringHelpers
{
    public static bool BeValidGearbox(this string value)
    {
        return !string.IsNullOrWhiteSpace(value) && Enum.TryParse<Gearbox>(char.ToUpper(value[0]) + value.ToLower()[1..], out _);
    }

    public static bool BeValidDrive(this string value)
    {
        return !string.IsNullOrWhiteSpace(value) && Enum.TryParse<Drive>(value.ToUpper(), out _);
    }

    public static Gearbox GetGearbox(this string value)
    {
        return Enum.Parse<Gearbox>(char.ToUpper(value[0]) + value[1..]);
    }

    public static Drive GetDrive(this string value)
    {
        return Enum.Parse<Drive>(value.ToUpper());
    }
}