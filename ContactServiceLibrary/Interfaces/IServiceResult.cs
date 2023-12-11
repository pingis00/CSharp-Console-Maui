using ContactServiceLibrary.Enums;

namespace ContactServiceLibrary.Interfaces;

public interface IServiceResult
{
    ServiceStatus Status { get; set; }
    object Result { get; set; }
}
