using MySpot.Application.Abstractions;
using MySpot.Application.DTO;

namespace MySpot.Application.Queries;

public sealed class GetUsers : IQuery<IEnumerable<UserDto>>
{
}