using Medical.Data;
using Medical.Entity;
using Microsoft.AspNetCore.Identity;

namespace Medical;

public class Seed : BackgroundService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<Seed> _logger;
    private UserManager<User> _userM;
    private RoleManager<IdentityRole> _roleM;

    public Seed(IServiceProvider provider, ILogger<Seed> logger)
    {
        _provider = provider;
        _logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _provider.CreateScope();
        _userM = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        _roleM = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        var ctx = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var conf = scope.ServiceProvider.GetRequiredService<IConfiguration>();

        var roles = new[] { "admin", "doctor", "nurse", "client" };

        foreach (var role in roles)
        {
            if (!await _roleM.RoleExistsAsync(role))
            {
                await _roleM.CreateAsync(new IdentityRole(role));
                _logger.LogInformation($"{role} added");
            }
        }

        if (await _userM.FindByEmailAsync("superadmin@ilmhub.uz") is null)
        {
            var user = new User()
            {
                Fullname = "Super Admin",
                Bio = "I am an admin. That's all",
                Dob = DateTimeOffset.UtcNow,
                Email = "superadmin@ilmhub.uz",
                PhoneNumber = "+998900070288",
                UserName = "superadmin"
            };


            var result = await _userM.CreateAsync(user, "123456");
            _logger.LogInformation("Create an admin...");
            if (result.Succeeded)
            {
                var existUser = await _userM.FindByEmailAsync("superadmin@ilmhub.uz");
                await _userM.AddToRolesAsync(existUser, new[] { "admin", "doctor", "nurse", "client" });

                _logger.LogInformation("Give roles to admin");
            }
            await ctx.SaveChangesAsync();
        }
    }
}