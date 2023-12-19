using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;
using ContactServiceLibrary.Repositories;
using Moq;

namespace ContactManagementSuite.Tests.ServiceTests;

public class ContactService_Tests
{
    private readonly Mock<IContactRepository> mockContactRepository;
    private readonly ContactService contactService;

    public ContactService_Tests()
    {
        mockContactRepository = new Mock<IContactRepository>();
        contactService = new ContactService(mockContactRepository.Object);
    }

    [Theory]
    [InlineData("andy@domain.com", ServiceStatus.SUCCESS, true)]
    [InlineData("existing@domain.com", ServiceStatus.ALREADY_EXISTS, false)]
    public void AddContactToList_ShouldReturnExpectedStatus_AndCallSaveCorrectly(string email, ServiceStatus expectedStatus, bool shouldCallSave)
    {
        var existingContact = new Contact { Email = "existing@domain.com" };
        var contacts = new List<IContact> { existingContact };
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(contacts);
        var newContact = new Contact { Email = email };

        var result = contactService.AddContact(newContact);

        Assert.Equal(expectedStatus, result.Status);
        var times = shouldCallSave ? Times.Once() : Times.Never();
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), times);
    }

    [Theory]
    [InlineData("andy@domain.com", ServiceStatus.DELETED, true)]
    [InlineData("nonexisting@domain.com", ServiceStatus.NOT_FOUND, false)]
    public void DeleteContactFromList_ShouldReturnExpectedStatus_AndCallSaveCorrectly(string email, ServiceStatus expectedStatus, bool shouldCallSave)
    {
        var contacts = new List<IContact>();
        if (email == "andy@domain.com")
        {
            contacts.Add(new Contact { Email = "andy@domain.com" });
        }
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(contacts);

        var result = contactService.DeleteContact(email);

        Assert.Equal(expectedStatus, result.Status);
        var times = shouldCallSave ? Times.Once() : Times.Never();
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), times);
    }

    [Theory]
    [InlineData("andy@domain.com", ServiceStatus.SUCCESS, true)]
    [InlineData("nonexisting@domain.com", ServiceStatus.NOT_FOUND, false)]
    public void GetContactFromFromListByEmail_ShouldReturnExpectedStatus(string email, ServiceStatus expectedStatus, bool shouldExist)
    {
        var contacts = new List<IContact>();
        if (shouldExist)
        {
            contacts.Add(new Contact { Email = "andy@domain.com" });
        }
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(contacts);

        var result = contactService.GetContactByEmailFromList(email);

        Assert.Equal(expectedStatus, result.Status);
        if(expectedStatus == ServiceStatus.SUCCESS)
        {
            Assert.NotNull(result.Result);
            var returnedContact = result.Result as Contact;
            Assert.NotNull(returnedContact);
            Assert.Equal(email, returnedContact.Email);
        }
        else
        {
            Assert.Null(result.Result);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void GetContactsFromList_ShouldReturnExpectedNumberOfContacts(int expectedCount)
    {
        var testContacts = CreateTestContacts(expectedCount);
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(testContacts);

        var result = contactService.GetContactsFromList();

        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        var contacts = result.Result as List<IContact>;
        Assert.Equal(expectedCount, contacts?.Count);
    }

    private List<IContact> CreateTestContacts(int count)
    {
        return Enumerable.Range(1, count).Select(i => (IContact) new Contact
        {
                FirstName = $"First{i}",
                LastName = $"Last{i}",
                Email = $"email{i}@example.com",
                PhoneNumber = $"0123456789{i}",
                Address = $"Address{i}"
        }).ToList();
    }

    [Theory]
    [InlineData("andy@domain.com", "Andreas", "Persson", "Hemvägen 123", "0123456789", ServiceStatus.UPDATED, true)]
    [InlineData("nonexistent@domain.com", "Fredrik", "Svensson", "Hemvägen 124", "9876543210", ServiceStatus.NOT_FOUND, false)]
    public void UpdateContactList_ShouldReturnExpectedStatus_AndCallSaveCorrectly(
        string emailToUpdate,
        string newFirstName,
        string newLastName,
        string newAddress,
        string newPhoneNumber,
        ServiceStatus expectedStatus,
        bool shouldCallSave)
    {
        var existingContact = new Contact { Email = "andy@domain.com", FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", PhoneNumber = "0123456789" };

        var contacts = new List<IContact> { existingContact };
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(contacts);

        var contactToUpdate = new Contact { Email = emailToUpdate, FirstName = newFirstName, LastName = newLastName, Address = newAddress, PhoneNumber = newPhoneNumber };

        var result = contactService.UpdateContact(contactToUpdate);

        mockContactRepository.Verify(x => x.LoadContacts(), Times.Once);
        Assert.Equal(expectedStatus, result.Status);

        if (expectedStatus == ServiceStatus.UPDATED)
        {
            var updatedContact = contacts.First(c => c.Email == existingContact.Email);
            Assert.Equal(newFirstName, updatedContact.FirstName);
            Assert.Equal(newLastName, updatedContact.LastName);
            Assert.Equal(newAddress, updatedContact.Address);
            Assert.Equal(newPhoneNumber, updatedContact.PhoneNumber);
        }

        var times = shouldCallSave ? Times.Once() : Times.Never();
        mockContactRepository.Verify(x => x.SaveContacts(contacts), times);
    }
}
