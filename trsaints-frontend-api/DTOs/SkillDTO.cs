using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace trsaints_frontend_api.DTOs;

public class SkillDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Name")]
    public string Name { get; set; }
}
