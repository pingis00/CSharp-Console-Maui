using ContactServiceLibrary.Enums;
using ContactServiceLibrary.Interfaces;

namespace ContactServiceLibrary.Models.Responses;

public class ServiceResult : IServiceResult
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
