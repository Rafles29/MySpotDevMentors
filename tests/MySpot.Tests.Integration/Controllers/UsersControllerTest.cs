using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MySpot.Application.Commands;
using MySpot.Application.DTO;
using MySpot.Core.Entities;
using MySpot.Core.ValueObjects;
using MySpot.Infrastructure.Security;
using MySpot.Infrastructure.Time;

namespace MySpot.Tests.Integration.Controllers;

[Collection("api")]
public class UsersControllerTest : ControllerTests, IDisposable
{
    [Fact]
    public async Task SignUp_ShouldReturn201()
    {
        await _testDatabase.Context.Database.MigrateAsync();
        var command = new SignUp(Guid.NewGuid(), "test@email.com",
            "rafles29", "Qwerty1!", "Rafal Wozniak", "user");

        var response = await Client.PostAsJsonAsync("/users", command);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task SignIn_ShouldReturn200()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        var clock = new Clock();
        const string password = "secret";
        var user = new User(Guid.NewGuid(), "test@myspot.com", "test-user",
            passwordManager.Secure(password), "Test User", Role.User(), clock.Current());
        
        await _testDatabase.Context.Database.MigrateAsync();
        await _testDatabase.Context.Users.AddAsync(user);
        await _testDatabase.Context.SaveChangesAsync();
        
        var command = new SignIn(user.Email, password);
        
        var response = await Client.PostAsJsonAsync("/users/sign-in", command);
        var jwt = response.Content.ReadFromJsonAsync<JwtDto>().Result;

        jwt.Should().NotBeNull();
        jwt.AccessToken.Should().NotBeNullOrWhiteSpace();
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
    
    [Fact]
    public async Task GetUserMe_ShouldReturnUsers()
    {
        var passwordManager = new PasswordManager(new PasswordHasher<User>());
        var clock = new Clock();
        const string password = "secret";
        var user = new User(Guid.NewGuid(), "test@myspot.com", "test-user",
            passwordManager.Secure(password), "Test User", Role.User(), clock.Current());
        
        await _testDatabase.Context.Database.MigrateAsync();
        await _testDatabase.Context.Users.AddAsync(user);
        await _testDatabase.Context.SaveChangesAsync();

        Authorize(user.Id, user.Role);
        var userDto = await Client.GetFromJsonAsync<UserDto>("users/me");
        
        userDto.Should().NotBeNull();
        userDto.Id.Should().Be(user.Id);
    }

    private TestDatabase _testDatabase;

    public UsersControllerTest(OptionsProvider optionsProvider) : base(optionsProvider)
    {
        _testDatabase = new TestDatabase();
    }

    public void Dispose()
    {
        _testDatabase.Dispose();
    }
}