using System.Security.Claims;

Dictionary<string, List<string>> gamesMap = new(){
    {"player1", new List<string> (){"EA FC 24", "WWE 2k24"}},
    {"player2", new List<string> (){"F1 2024", "NBA 2k24", "PGA tour 23"}}
};

Dictionary<string, List<string>> subscriptionMap = new(){
    {"silver", new List<string> (){"EA FC 24", "WWE 2k24"}},
    {"platinum", new List<string> (){"EA FC 24", "WWE 2k24, F1 2024", "NBA 2k24", "PGA tour 23"}}
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
    var hasClaim = user.HasClaim(claim => claim.Type == "subscription");

    if(hasClaim){
        var subscriptions  = user.FindFirstValue("subscription") ?? throw new Exception("Claim has no value!");
        return Results.Ok(subscriptionMap[subscriptions]);
    }

    ArgumentNullException.ThrowIfNull(user.Identity?.Name);
    var username = user.Identity.Name;

    if(!gamesMap.ContainsKey(username)){
        return Results.Empty;
    }

    return Results.Ok(gamesMap[username]);
}).RequireAuthorization(policy => {
        policy.RequireRole("player");
    });

app.Run();
