using Demo.Core.DTOs;
using Demo.Core.Repositories;
using Demo.Core.Services;
using Demo.Infrastructure;
using FluentValidation;

namespace Demo.API.Endpoints
{
    internal static class GuitarEndpoints
    {
        internal static void MapGuitarEndpoints(this WebApplication app)
        {
            app.MapPost("/guitars", CreateAsync);
            app.MapGet("/guitars/{id}", ReadAsync);
            app.MapGet("/guitars", ReallAllAsync);
            app.MapPost("/guitars/string", StringAsync);
            app.MapPut("/guitars/tune", TuneAsync);
            app.MapPut("/guitars", UpdateAsync);
            app.MapDelete("/guitars/{id}", DeleteAsync);
        }

        internal static void AddGuitarServices(this WebApplicationBuilder builder, string repositoryImplementation)
        {
            builder.Services.AddScoped<IGuitarService, GuitarService>();

            if (repositoryImplementation == "EF")
            {
                builder.Services.AddScoped<IGuitarRepository, EFGuitarRepository>();
            }
            else
            {
                builder.Services.AddSingleton<IGuitarRepository, InMemoryGuitarRepository>();
            }
        }

        internal async static Task<IResult> CreateAsync(IGuitarService guitarService, IValidator<GuitarDto> validator, GuitarDto guitarDto)
        {
            var validationResult = validator.Validate(guitarDto);
            if (!validationResult.IsValid)
            {
                return Results.BadRequest(new { errors = validationResult.Errors.Select(x => x.ErrorMessage) });
            }

            guitarDto = await guitarService.CreateAsync(guitarDto);
            return Results.Created($"/guitars/{guitarDto.Id}", guitarDto);
        }

        internal async static Task<IResult> ReadAsync(IGuitarService guitarService, int id)
        {
            var guitarDto = await guitarService.ReadAsync(id);
            if (guitarDto == null)
            {
                return Results.NotFound(id);
            }

            return Results.Ok(guitarDto);
        }

        internal async static Task<IResult> ReallAllAsync(IGuitarService guitarService)
        {
            var guitarDtos = await guitarService.ReadAllAsync();
            return Results.Ok(guitarDtos);
        }

        internal async static Task<IResult> StringAsync(IGuitarRepository guitarRepository, IValidator<GuitarTuningDto> validator, GuitarTuningDto guitarTuningDto)
        {
            var guitar = await guitarRepository.ReadAsync(guitarTuningDto.GuitarId);
            if (guitar != null)
            {
                var errors = ValidateGuitarTuningDto(validator, guitarTuningDto, guitar.MaxNumberOfStrings);
                if (errors.Any())
                {
                    return Results.BadRequest(new { errors });
                }

                guitarTuningDto.GuitarStrings.ForEach(x => guitar.String(x.Number, x.Gauge, x.Tuning));
                await guitarRepository.SaveChangesAsync();

                return Results.NoContent();
            }
            
            return Results.NotFound(guitarTuningDto.GuitarId);
        }

        internal async static Task<IResult> TuneAsync(IGuitarRepository guitarRepository, IValidator<GuitarTuningDto> validator, GuitarTuningDto guitarTuningDto)
        {
            var guitar = await guitarRepository.ReadAsync(guitarTuningDto.GuitarId);
            if (guitar != null)
            {
                var errors = ValidateGuitarTuningDto(validator, guitarTuningDto, guitar.MaxNumberOfStrings);
                if (errors.Any())
                {
                    return Results.BadRequest(new { errors });
                }

                guitarTuningDto.GuitarStrings.ForEach(x => guitar.String(x.Number, x.Gauge, x.Tuning));
                await guitarRepository.SaveChangesAsync();

                return Results.NoContent();
            }

            return Results.NotFound(guitarTuningDto.GuitarId);
        }

        internal async static Task<IResult> UpdateAsync(IGuitarService guitarService, IGuitarRepository guitarRepository, GuitarDto guitarDto)
        {
            var guitar = await guitarRepository.FindAsync(guitarDto.Id);
            if (guitar != null)
            {
                await guitarService.UpdateAsync(guitar, guitarDto);
                return Results.NoContent();
            }

            return Results.NotFound(guitarDto.Id);
        }

        internal async static Task<IResult> DeleteAsync(IGuitarRepository guitarRepository, int id)
        {
            var guitar = await guitarRepository.ReadAsync(id);
            if (guitar != null)
            {
                guitarRepository.Delete(guitar);
                await guitarRepository.SaveChangesAsync();

                return Results.NoContent();
            }

            return Results.NotFound(id);
        }

        private static List<string> ValidateGuitarTuningDto(IValidator<GuitarTuningDto> validator, GuitarTuningDto guitarTuningDto, int maxNumberOfStringsForGuitar)
        {
            var errors = new List<string>();

            var validationResult = validator.Validate(guitarTuningDto);
            if (!validationResult.IsValid)
            {
                errors.AddRange(validationResult.Errors.Select(x => x.ErrorMessage));
            }

            if (guitarTuningDto.GuitarStrings != null && guitarTuningDto.GuitarStrings.Any())
            {
                var invalidStringNumbers = guitarTuningDto.GuitarStrings.Where(x => x.Number > maxNumberOfStringsForGuitar).Select(x => x.Number).Distinct();
                if (invalidStringNumbers.Any())
                {
                    errors.Add($"The following string numbers are not valid for a {maxNumberOfStringsForGuitar} string guitar: {string.Join(",", invalidStringNumbers)}");
                }
            }

            return errors;
        }
    }    
}