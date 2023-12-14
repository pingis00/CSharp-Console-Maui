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

        var result = contactService.AddContactToList(newContact);

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

        var result = contactService.AddContactToList(newContact);

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

        var result = contactService.DeleteContactFromList("andy@domain.com");

        Assert.Equal(ServiceStatus.DELETED, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Once);
    }

    [Fact]
    public void DeleteContactFromList_ShouldReturnNotFound_IfContactDoesntExist()
    {
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact>());
        var contactService = new ContactService(mockContactRepository.Object);

        var result = contactService.DeleteContactFromList("andy@domain.com");

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
}
