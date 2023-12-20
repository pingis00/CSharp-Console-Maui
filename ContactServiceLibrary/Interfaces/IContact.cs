namespace ContactServiceLibrary.Interfaces;

public interface IContact
{
    /// <summary>
    /// Gets or sets the first name of the contact
    /// </summary>
    string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the contact
    /// </summary>
    string LastName { get; set; }

    /// <summary>
    /// Gets or sets the email of the contact
    /// </summary>
    string Email { get; set; }

    /// <summary>
    /// Gets or sets the phonenumber of the contact
    /// </summary>
    string PhoneNumber { get; set; }

    /// <summary>
    /// Gets or sets the address of the contact
    /// </summary>
    string Address { get; set; }
}
