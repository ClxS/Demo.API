using Demo.Domain;

namespace Demo.Core.DTOs
{
    public class GuitarDto
    {
        public int Id {  get; set; }

        public GuitarType GuitarType { get; set; }

        public int MaxNumberOfStrings { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        public DateTime DateManufactured { get; set; }
    }
}