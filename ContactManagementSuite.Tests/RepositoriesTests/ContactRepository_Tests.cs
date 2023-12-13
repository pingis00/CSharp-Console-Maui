using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;
using ContactServiceLibrary.Repositories;
using Moq;
using Newtonsoft.Json;

namespace ContactManagementSuite.Tests.RepositoriesTests;

public class ContactRepository_Tests
{
    [Fact]
    public void LoadContacts_ShouldReturnContacts_WhenFileHasContent()
    {
        var mockFileServic = new Mock<IFileService>();
        var testContacts = new List<IContact>
        {
            new Contact { FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", Email = "andy@domain.com", PhoneNumber = "0123456789" },
            new Contact { FirstName = "Fredrik", LastName = "Svensson", Address = "Hemvägen 124", Email = "fredrik@domain.com", PhoneNumber = "9876543210" }
        };
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        var json = JsonConvert.SerializeObject(testContacts, settings);
        mockFileServic.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(json);
        var contactRepository = new ContactRepository(mockFileServic.Object, "filväg");

        var contacts = contactRepository.LoadContacts();

        Assert.NotNull(contacts);
        Assert.Equal(testContacts.Count, contacts.Count);
        
        for (int i = 0; i < testContacts.Count; i++)
        {
            Assert.Equal(testContacts[i].FirstName, contacts[i].FirstName);
            Assert.Equal(testContacts[i].LastName, contacts[i].LastName);
            Assert.Equal(testContacts[i].Address, contacts[i].Address);
            Assert.Equal(testContacts[i].Email, contacts[i].Email);
            Assert.Equal(testContacts[i].PhoneNumber, contacts[i].PhoneNumber);
        }
    }

    [Fact]
    public void LoadContacts_ShouldReturnEmptyList_WhenFileIsEmpty()
    {
        var mockFileService = new Mock<IFileService>();
        mockFileService.Setup(x => x.GetContentFromFile(It.IsAny<string>())).Returns(string.Empty);
        var contactRepository = new ContactRepository(mockFileService.Object, "filväg");

        var contacts = contactRepository.LoadContacts();

        Assert.NotNull(contacts);
        Assert.Empty(contacts);
    }
}
