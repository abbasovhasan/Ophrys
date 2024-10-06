var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    // FluentValidation'ı entegre ediyoruz
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<ParkDtoValidator>());

// Identity hizmetlerini ekliyoruz
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    PasswordValidationConfig.Configure(options); // Şifre validasyonu kurallarını buradan çağırıyoruz
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Yetkilendirme politikaları ekliyoruz
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});

// Kimlik doğrulama için cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/api/account/login"; // Giriş yapılmamışsa yönlendirme yapılacak endpoint
    });

// Add DbContext with SQL Server connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories for Dependency Injection
builder.Services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
builder.Services.AddScoped(typeof(IWriteRepository<>), typeof(WriteRepository<>));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger servislerini ekliyoruz

var app = builder.Build();

// Rollerin ve adminin başlatılması (Admin, User)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

    // Rolleri ve Admin kullanıcıyı başlatıyoruz
    try
    {
        await RoleInitializer.InitializeRolesAndAdmin(roleManager, userManager).ConfigureAwait(false);
    }
    catch (Exception ex)
    {
        // Hataları loglamak veya göstermek için kullanabilirsiniz
        Console.WriteLine($"Error during role initialization: {ex.Message}");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger middleware'i ekleniyor
    app.UseSwaggerUI(); // Swagger UI middleware'i ekleniyor
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Kimlik doğrulama middleware'i
app.UseAuthorization();   // Yetkilendirme middleware'i

app.MapControllers();     // Controller endpoint'lerini haritalama

app.Run();
