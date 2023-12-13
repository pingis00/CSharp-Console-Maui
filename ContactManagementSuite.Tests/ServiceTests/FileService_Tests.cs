using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Services;

namespace ContactManagementSuite.Tests.ServiceTests;

public class FileService_Tests
{
    [Fact]
    public void SaveContentToFile_ShouldCreateFile_WithContent()
    {
        // Arrange
        IFileService fileService = new FileService();
        string testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        string contentToSave = "Test content";

        // Act
        bool result = fileService.SaveContentToFile(testFilePath, contentToSave);
        string fileContent = File.ReadAllText(testFilePath);

        // Assert
        Assert.True(result);
        Assert.Equal(contentToSave + Environment.NewLine, fileContent);

        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }

    [Fact]
    public void GetContentFromFile_ShouldReturnContent_IxExists()
    {

    }
}
