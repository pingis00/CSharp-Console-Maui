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
        var existingContact = new Contact { Email = "existing@example.com" };
        var mockContactRepository = new Mock<IContactRepository>();
        mockContactRepository.Setup(x => x.LoadContacts()).Returns(new List<IContact> { existingContact });
        var contactService = new ContactService(mockContactRepository.Object);

        var newContact = new Contact { Email= "existing@example.com" };

        var result = contactService.AddContactToList(newContact);

        Assert.Equal(ServiceStatus.ALREADY_EXISTS, result.Status);
        mockContactRepository.Verify(x => x.SaveContacts(It.IsAny<List<IContact>>()), Times.Never);
    }
}
