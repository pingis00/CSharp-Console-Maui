using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Repositories;

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
    public void GetContentFromFile_ShouldReturnContent_IfExists()
    {
        // Arrange
        IFileService fileService = new FileService();
        string testFilePath = Path.GetTempFileName();
        string expectedContent = "Test content";
        File.WriteAllText(testFilePath, expectedContent);

        // Act
        string actualContent = fileService.GetContentFromFile(testFilePath);

        // Assert
        Assert.Equal(expectedContent, actualContent);

        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }
}
