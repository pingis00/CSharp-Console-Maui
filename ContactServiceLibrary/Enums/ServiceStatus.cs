namespace ContactServiceLibrary.Enums;

public enum ServiceStatus
{
    /// <summary>
    /// Indicates that the operation failed to complete successfully.
    /// </summary>
    FAILED = 0,

    /// <summary>
    /// Indicates that the operation was completed successfully.
    /// </summary>
    SUCCESS = 1,

    /// <summary>
    /// Indicates that the operation did not proceed because the entity already exists.
    /// </summary>
    ALREADY_EXISTS = 2,

    /// <summary>
    /// Indicates that the specified entity could not be found.
    /// </summary>
    NOT_FOUND = 3,

    /// <summary>
    /// Indicates that the entity was successfully updated.
    /// </summary>
    UPDATED = 4,

    /// <summary>
    /// Indicates that the entity was successfully deleted.
    /// </summary>
    DELETED = 5
}
