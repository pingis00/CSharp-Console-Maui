using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;
using ContactServiceLibrary.Repositories;
using Moq;

namespace ContactManagementSuite.Tests.ServiceTests;

public class ContactService_Tests
{
    [Fact]
    public void AddContactToList_ShouldAddContact_WhenNotExists()
    {
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact>());
        var contactService = new ContactService(mockContactRepository.Object);

        var newContact = new Contact { Email = "andy@domain.com" };

        var result = contactService.AddContact(newContact);

        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Once);
    }

    [Fact]
    public void AddContactToList_ShouldNotAddContact_WhenExists()
    {
        var existingContact = new Contact { Email = "andy@domain.com" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        var contactService = new ContactService(mockContactRepository.Object);

        var newContact = new Contact { Email= "andy@domain.com" };

        var result = contactService.AddContact(newContact);

        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Never);
    }

    [Fact]
    public void DeleteContactFromList_ShouldDeleteContactByEmail_IfContactExist()
    {
        var existingContact = new Contact { Email = "andy@domain.com" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.DeleteContact("andy@domain.com");

        Assert.Equal(ServiceStatus.DELETED, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Once);
    }

    [Fact]
    public void DeleteContactFromList_ShouldReturnNotFound_IfContactDoesntExist()
    {
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact>());
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.DeleteContact("andy@domain.com");

        Assert.Equal(ServiceStatus.NOT_FOUND, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Never);
    }

    [Fact]
    public void GetContactFromListByEmail_ShouldGetContact_IfItExists()
    {
        var existingContact = new Contact { Email = "andy@domain.com" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.GetContactByEmailFromList("andy@domain.com");

        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        Assert.NotNull(result.Result);
        var returnedContact = result.Result as Contact;
        Assert.NotNull(returnedContact);
        Assert.Equal("andy@domain.com", returnedContact.Email);
    }


    [Fact]
    public void GetContactFromListByEmail_ShouldReturnNotFound_IfEmailDoesntExist()
    {
        var existingContact = new Contact { Email = "andy@domain.com" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.GetContactByEmailFromList("andreas@domain.com");

        Assert.Equal(ServiceStatus.NOT_FOUND, result.Status);
        Assert.Null(result.Result);
    }

    [Fact]
    public void GetContactsFromList_ShouldReturnContactList_IfListExist()
    {
        var testContacts = new List<IContact>
        {
            new Contact { FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", Email = "andy@domain.com", PhoneNumber = "0123456789" },
            new Contact { FirstName = "Fredrik", LastName = "Svensson", Address = "Hemvägen 124", Email = "fredrik@domain.com", PhoneNumber = "9876543210" }
        };

        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(testContacts);
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.GetContactsFromList();

        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        Assert.NotNull(result.Result);
        var contacts = result.Result as List<IContact>;
        Assert.NotNull(contacts);
        Assert.Equal(testContacts.Count, contacts.Count);
    }

    [Fact]
    public void GetContactsFromList_ShouldReturnEmptyList_IfNoContactsExists()
    {
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact>());
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.GetContactsFromList();

        Assert.Equal(ServiceStatus.SUCCESS, result.Status);
        Assert.NotNull(result.Result);
        var contacts = result.Result as List<IContact>;
        Assert.NotNull(contacts);
        Assert.Empty(contacts);
    }

    [Fact]
    public void UpdateContactList_ShouldUpdateContact_IfItExists()
    {
        var existingContact = new Contact { Email = "andy@domain.com", FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", PhoneNumber = "0123456789" };
        var updatedContact = new Contact { Email = "andy@domain.com", FirstName = "Fredrik", LastName = "Svensson", Address = "Hemvägen 124", PhoneNumber = "9876543210" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        mockContactRepository.Setup(x => x.SaveContacts(It.IsAny<List<IContact>>())).Verifiable();
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.UpdateContact(updatedContact);

        Assert.Equal(ServiceStatus.UPDATED, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Once);
    }

    [Fact]
    public void UpdateContactList_ShouldReturnNotFound_IfContactDoesntExist()
    {
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact>());
        var contactService = new ContactService(mockContactRepository.Object);
        var noneExistingContact = new Contact { Email = "andy@domain.com" };

        var result = contactService.UpdateContact(noneExistingContact);

        Assert.Equal(ServiceStatus.NOT_FOUND, result.Status);
    }
}
