using System.ComponentModel.DataAnnotations;

namespace DemoCoreApp_API.Models.Dto
{
    public class DemoFDTO
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public int EmployeeNumber { get; set; }

        public int sallary { get; set; }
    }
}
