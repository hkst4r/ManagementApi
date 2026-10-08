using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DTOs
{
    public class MachineResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
    }
}