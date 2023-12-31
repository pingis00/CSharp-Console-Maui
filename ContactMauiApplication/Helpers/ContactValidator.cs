using ContactServiceLibrary.Utilities;
using System.Text;
using Contact = ContactServiceLibrary.Models.Contact;

namespace ContactMauiApplication.Helpers
{
    public static class ContactValidator
    {
        public static string ValidateContact(Contact contact, bool validateEmail = true)
        {
            StringBuilder validationErrors = new StringBuilder();

            if (string.IsNullOrEmpty(contact.FirstName))
            {
                validationErrors.AppendLine("First name is required.");
            }
            if (string.IsNullOrEmpty(contact.LastName))
            {
                validationErrors.AppendLine("Last name is required.");
            }
            if (string.IsNullOrEmpty(contact.Address))
            {
                validationErrors.AppendLine("Address is required.");
            }
            if (string.IsNullOrEmpty(contact.Email))
            {
                validationErrors.AppendLine("Email is required.");
            }
            else if (!ValidationUtility.IsValidEmail(contact.Email))
            {
                validationErrors.AppendLine("Invalid email format. Expected format: example@domain.com");
            }
            if (string.IsNullOrEmpty(contact.PhoneNumber))
            {
                validationErrors.AppendLine("Phone number is required.");
            }
            else if (!ValidationUtility.IsValidPhoneNumber(contact.PhoneNumber))
            {
                validationErrors.AppendLine("Invalid phone number format. Expected format: +1234567890 or 0123456789 without spaces or hyphens.");
            }

            return validationErrors.ToString();
        }
    }
}
