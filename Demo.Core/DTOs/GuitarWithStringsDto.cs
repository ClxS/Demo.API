namespace Demo.Core.DTOs
{
    public class GuitarWithStringsDto
    {
        public GuitarDto GuitarDto {  get; set; }

        public List<GuitarStringDto> GuitarStringDtos {  get; set; }
    }
}