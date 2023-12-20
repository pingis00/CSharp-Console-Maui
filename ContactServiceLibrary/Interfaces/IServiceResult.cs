using ContactServiceLibrary.Enums;

namespace ContactServiceLibrary.Interfaces;

/// <summary>
/// Represents the result of a service operation,  including the status and any additional result data.
/// </summary>
public interface IServiceResult
{
    /// <summary>
    /// Gets or sets the status of the service operation.
    /// </summary>
    ServiceStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the result data of the service operation.
    /// </summary>
    object Result { get; set; }
}
