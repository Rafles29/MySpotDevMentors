using MySpot.Application.Abstractions;

namespace MySpot.Application.Commands;

public sealed record SignUp(Guid UserId, string Email, string Username, string Password, string FullName, string Role) : ICommand;