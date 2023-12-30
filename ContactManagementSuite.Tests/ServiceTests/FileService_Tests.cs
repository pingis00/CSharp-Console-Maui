using ContactServiceLibrary.Interfaces;
using ContactServiceLibrary.Repositories;

namespace ContactManagementSuite.Tests.ServiceTests;

public class FileService_Tests
{
    [Fact]
    public async void SaveContentToFileAsync_ShouldCreateFile_WithContent()
    {
        // Arrange
        IFileService fileService = new FileService();
        string testFilePath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        string contentToSave = "Test content";

        // Act
        bool result = await fileService.SaveContentToFileAsync(testFilePath, contentToSave);
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
    public async void GetContentFromFileAsync_ShouldReturnContent_IfExists()
    {
        // Arrange
        IFileService fileService = new FileService();
        string testFilePath = Path.GetTempFileName();
        string expectedContent = "Test content";
        File.WriteAllText(testFilePath, expectedContent);

        // Act
        string actualContent = await fileService.GetContentFromFileAsync(testFilePath);

        // Assert
        Assert.Equal(expectedContent, actualContent);

        if (File.Exists(testFilePath))
        {
            File.Delete(testFilePath);
        }
    }
}
