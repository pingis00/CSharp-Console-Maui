using System.Text.RegularExpressions;

namespace ContactServiceLibrary.Utilities;

public class ValidationUtility
{
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

    public static bool IsValidPhoneNumber(string phoneNumber)
    {
        var regex = new Regex(@"^\+?[0-9]\d{1,14}$");
        return regex.IsMatch(phoneNumber);
    }
}
