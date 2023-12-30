namespace ContactServiceLibrary.Interfaces;

public interface IContactService
{
    /// <summary>
    /// Adds a new contact to the contact list.
    /// </summary>
    /// <param name="contact">A contact of type IContact to add</param>
    /// <returns>An IServiceResult containing the result of the add operation, including status and any error messages.</returns>
    Task<IServiceResult> AddContactAsync(IContact contact);

    /// <summary>
    /// Deletes a contact from the contact list based on their email address.
    /// </summary>
    /// <param name="email">The email address of the contact to be deleted.</param>
    /// <returns>An IServiceResult containing the result of the delete operation, including status and any error messages.</returns>
    Task<IServiceResult> DeleteContactAsync(string email);

    /// <summary>
    /// Updates a contact from the list.
    /// </summary>
    /// <param name="contact">The contact of type IContact to update</param>
    /// <returns>An IServiceResult containing the result of the update operation, including status and any error messages.</returns>
    Task<IServiceResult> UpdateContactAsync(IContact contact);

    /// <summary>
    /// Retrieves the entire contact list.
    /// </summary>
    /// <returns>An IServiceResult containing the contact list. If the list is empty or does not exist, it returns an empty list. The result also includes the operation status and any error messages</returns>
    Task<IServiceResult> GetContactsFromListAsync();

    /// <summary>
    /// Retrieves a contact from the contact list based on their email address.
    /// </summary>
    /// <param name="email">The email address of the contact to retrieve</param>
    /// <returns>An IServiceResult containing the result of the GetContact operation, including status and any error messages.</returns>
    Task<IServiceResult> GetContactByEmailFromListAsync(string email);

    event EventHandler ContactsUpdated;
}
