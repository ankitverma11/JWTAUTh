using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

//add services to controller 
builder.Services.AddControllers();
// Bind JwtSettings
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

// Retrieve the secret key for JWT
var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
var key =  Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
// Add jwt Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(option => {
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5111",
        ValidAudience = "http://localhost:5111",
        IssuerSigningKey = new SymmetricSecurityKey(key),
       ClockSkew = TimeSpan.Zero    
    };
});

//add authorization
builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
