using Demo.Core.DTOs;
using Demo.Core.Repositories;
using Demo.Domain;

namespace Demo.Core.Services
{
    public interface IGuitarService
    {
        Task<GuitarDto> CreateAsync(GuitarDto guitarDto);
        Task<GuitarWithStringsDto?> ReadAsync(int id);
        Task<List<GuitarWithStringsDto>> ReadAllAsync();
        Task UpdateAsync(Guitar guitar, GuitarDto guitarDto);
    }

    public class GuitarService : IGuitarService
    {
        private readonly IGuitarRepository _guitarRepository;

        public GuitarService(IGuitarRepository guitarRepository)
        {
            _guitarRepository = guitarRepository;
        }

        public async Task<GuitarDto> CreateAsync(GuitarDto guitarDto)
        {
            var guitar = new Guitar(guitarDto.GuitarType, guitarDto.MaxNumberOfStrings, guitarDto.Make, guitarDto.Model);            

            await _guitarRepository.CreateAsync(guitar);
            await _guitarRepository.SaveChangesAsync();

            guitarDto.Id = guitar.Id;
            return guitarDto;
        }

        public async Task<GuitarWithStringsDto?> ReadAsync(int id)
        {
            var guitar = await _guitarRepository.ReadAsync(id);
            return guitar != null ? ConvertToDto(guitar) : null;
        }

        public async Task<List<GuitarWithStringsDto>> ReadAllAsync()
        {
            var guitars = await _guitarRepository.ReadAllAsync();
            if (guitars.Any())
            {
                return guitars.Select(x => ConvertToDto(x)).ToList();
            }

            return new List<GuitarWithStringsDto>();
        }

        public async Task UpdateAsync(Guitar guitar, GuitarDto guitarDto)
        {
            guitar.Make = guitarDto.Make;
            guitar.Model = guitarDto.Model;
            _guitarRepository.Update(guitar);
            await _guitarRepository.SaveChangesAsync();
        }

        private static GuitarWithStringsDto ConvertToDto(Guitar guitar)
        {
            return new GuitarWithStringsDto
            {
                GuitarDto = new GuitarDto
                {
                    Id = guitar.Id,
                    GuitarType = guitar.GuitarType,
                    Make = guitar.Make,
                    Model = guitar.Model,
                    DateManufactured = guitar.DateManufactured
                }
                ,
                GuitarStringDtos = guitar.GuitarStrings
                    .Select(x => new GuitarStringDto
                    {
                        Id = x.Id,
                        Number = x.Number,
                        Gauge = x.Gauge,
                        Tuning = x.Tuning,
                        Created = x.Created,
                        DateRestrung = x.DateRestrung
                    }).ToList()
            };
        }
    }
}