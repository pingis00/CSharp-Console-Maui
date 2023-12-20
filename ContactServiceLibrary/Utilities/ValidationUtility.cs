using System.Text.RegularExpressions;

namespace ContactServiceLibrary.Utilities;

public class ValidationUtility
{
    /// <summary>
    /// Validates an email address based on standard email format.
    /// </summary>
    /// <param name="email">The email address to validate.</param>
    /// <returns>true if the email address is valid; otherwise, false.</returns>
    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Validates a phone number based on a general international format.
    /// </summary>
    /// <param name="phoneNumber">The phone number to validate.</param>
    /// <returns>true if the phone number is valid; otherwise, false.</returns>
    /// <remarks>
    /// This method checks if the phone number starts with an optional plus followed by up to 15 digits.
    /// </remarks>
    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = new Regex(@"^\+?[0-9]\d{1,14}$");
        return regex.IsMatch(phoneNumber);
    }
}
