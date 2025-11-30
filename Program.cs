using APIUsuario.Application.DTOs;
using APIUsuario.Application.Interfaces;
using APIUsuario.Application.Services;
using APIUsuario.Application.Validators;
using APIUsuario.Infrastructure.Persistence;
using APIUsuario.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString));


builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();


builder.Services.AddScoped<IValidator<UsuarioCreateDto>, UsuarioCreateDtoValidator>();
builder.Services.AddScoped<IValidator<UsuarioUpdateDto>, UsuarioUpdateDtoValidator>();

var app = builder.Build();


app.MapGet("/usuarios", async (IUsuarioService service, CancellationToken ct) =>
{
    var usuarios = await service.ListarAsync(ct);
    return Results.Ok(usuarios);
});


app.MapGet("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var usuario = await service.ObterAsync(id, ct);
    return usuario is not null ? Results.Ok(usuario) : Results.NotFound();
});


app.MapPost("/usuarios", async (UsuarioCreateDto dto, IUsuarioService service, CancellationToken ct) =>
{
    try 
    {
        var criado = await service.CriarAsync(dto, ct);
        
        return Results.Created($"/usuarios/{criado.Id}", criado);
    }
    catch (ValidationException ex)
    {
        
        return Results.BadRequest(ex.Errors);
    }
    catch (Exception ex)
    {
        
        return Results.BadRequest(new { error = ex.Message });
    }
});


app.MapPut("/usuarios/{id}", async (int id, UsuarioUpdateDto dto, IUsuarioService service, CancellationToken ct) =>
{
    try
    {
        var atualizado = await service.AtualizarAsync(id, dto, ct);
        return Results.Ok(atualizado);
    }
    catch (ValidationException ex)
    {
        
        return Results.BadRequest(ex.Errors);
    }
    catch (Exception ex)
    {
        
        if (ex.Message == "Usuário não encontrado")
            return Results.NotFound(new { error = ex.Message });

        return Results.BadRequest(new { error = ex.Message });
    }
});


app.MapDelete("/usuarios/{id}", async (int id, IUsuarioService service, CancellationToken ct) =>
{
    var sucesso = await service.RemoverAsync(id, ct);
    return sucesso ? Results.NoContent() : Results.NotFound();
});


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.Run();