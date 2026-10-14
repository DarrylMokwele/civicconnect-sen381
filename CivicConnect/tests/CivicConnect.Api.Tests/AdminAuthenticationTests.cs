using System.Net;
using System.Net.Http.Json;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CivicConnect.Api.Tests;

public sealed class AdminAuthenticationTests
{
    [Fact]
    public async Task Staff_CanLogin_ReadOwnIdentity_AndLogout()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();
        await host.SeedAuthenticationUsersAsync();

        var login = await client.PostAsJsonAsync("/api/admin/login", new
        {
            username = "staff",
            password = "Staff@123"
        });

        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var me = await client.GetAsync("/api/admin/me");
        Assert.Equal(HttpStatusCode.OK, me.StatusCode);
        Assert.Contains("Staff", await me.Content.ReadAsStringAsync());

        var logout = await client.PostAsync("/api/admin/logout", null);
        Assert.Equal(HttpStatusCode.NoContent, logout.StatusCode);

        var afterLogout = await client.GetAsync("/api/admin/me");
        Assert.Equal(HttpStatusCode.Unauthorized, afterLogout.StatusCode);
    }

    [Fact]
    public async Task Admin_CanLogin_AndAccessProtectedOverview()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();
        await host.SeedAuthenticationUsersAsync();

        var login = await client.PostAsJsonAsync("/api/admin/login", new
        {
            username = "admin",
            password = "Admin@123"
        });

        Assert.Equal(HttpStatusCode.OK, login.StatusCode);
        var overview = await client.GetAsync("/api/admin/overview");
        Assert.Equal(HttpStatusCode.OK, overview.StatusCode);
    }

    [Fact]
    public async Task InvalidCredentials_AreRejected()
    {
        using var host = new ApiHost();
        using var client = await host.SeedAsync();
        await host.SeedAuthenticationUsersAsync();

        var login = await client.PostAsJsonAsync("/api/admin/login", new
        {
            username = "admin",
            password = "wrong-password"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, login.StatusCode);
    }
}
