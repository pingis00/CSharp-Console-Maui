using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Models;
using ContactServiceLibrary.Repositories;
using Moq;
using Newtonsoft.Json;

namespace ContactManagementSuite.Tests.RepositoriesTests;

public class ContactRepository_Tests
{
    [Fact]
    public async void LoadContactsAsync_ShouldReturnContacts_WhenFileHasContent()
    {
        var mockFileService = new Mock<IFileService>();
        var testContacts = new List<IContact>
        {
            new Contact { FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", Email = "andy@domain.com", PhoneNumber = "0123456789" },
            new Contact { FirstName = "Fredrik", LastName = "Svensson", Address = "Hemvägen 124", Email = "fredrik@domain.com", PhoneNumber = "9876543210" }
        };
        var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.Objects };
        var json = JsonConvert.SerializeObject(testContacts, settings);
        mockFileService.Setup(x => x.GetContentFromFileAsync(It.IsAny<string>())).ReturnsAsync(json);
        var contactRepository = new ContactRepository(mockFileService.Object, "filväg");

        var contacts = await contactRepository.LoadContactsAsync();

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
    public async void LoadContactsAsync_ShouldReturnEmptyList_WhenFileIsEmpty()
    {
        var mockFileService = new Mock<IFileService>();
        mockFileService.Setup(x => x.GetContentFromFileAsync(It.IsAny<string>())).ReturnsAsync(string.Empty);
        var contactRepository = new ContactRepository(mockFileService.Object, "filväg");

        var contacts = await contactRepository.LoadContactsAsync();

        Assert.NotNull(contacts);
        Assert.Empty(contacts);
    }

    [Fact]
    public async void SaveContactsAsync_ShouldCallSaveContentToFile_WithCorrectJson()
    {
        var mockFileService = new Mock<IFileService>();
        var testContacts = new List<IContact>
        {
            new Contact { FirstName = "Andreas", LastName = "Persson", Address = "Hemvägen 123", Email = "andy@domain.com", PhoneNumber = "0123456789" },
            new Contact { FirstName = "Fredrik", LastName = "Svensson", Address = "Hemvägen 124", Email = "fredrik@domain.com", PhoneNumber = "9876543210" }
        };
        var contactRepository = new ContactRepository(mockFileService.Object, "filväg");
        var settings = new JsonSerializerSettings
        {
            TypeNameHandling = TypeNameHandling.Objects,
            Formatting = Formatting.Indented
        };
        var expectedJson = JsonConvert.SerializeObject(testContacts, settings);

        await contactRepository.SaveContactsAsync(testContacts);

        mockFileService.Verify(x => x.SaveContentToFileAsync("filväg", It.Is<string>(json => json == expectedJson)));
    }
}
