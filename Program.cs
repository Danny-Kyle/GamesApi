using System.Security.Claims;

Dictionary<string, List<string>> gamesMap = new(){
    {"player1", new List<string> (){"EA FC 24", "WWE 2k24"}},
    {"player2", new List<string> (){"F1 2024", "NBA 2k24", "PGA tour 23"}}
};

Dictionary<string, List<string>> subscriptionMap = new(){
    {"silver", new List<string> (){"EA FC 24", "WWE 2k24"}},
    {"platinum", new List<string> (){"EA FC 24", "WWE 2k24", "F1 2024", "NBA 2k24", "PGA tour 23"}}
};


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication().AddJwtBearer();
builder.Services.AddAuthorization();

var app = builder.Build();

app.MapGet("/playergames", () => gamesMap)
    .RequireAuthorization(policy => {
        policy.RequireRole("admin");
    });

app.MapGet("/mygames", (ClaimsPrincipal user) => {
    var subscription = user.FindFirstValue("subscription");
    if (subscription is not null)
    {
        return subscriptionMap.TryGetValue(subscription, out var available)
            ? Results.Ok(available)
            : Results.Forbid();
    }

    var username = user.Identity?.Name;
    return username is not null && gamesMap.TryGetValue(username, out var games)
        ? Results.Ok(games)
        : Results.Ok(Array.Empty<string>());
}).RequireAuthorization(policy => {
        policy.RequireRole("player");
    });

app.Run();
