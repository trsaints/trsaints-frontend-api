using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace trsaints_frontend_api.Data.DTOs;

public class SkillDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Name")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Category is required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Category")]
    public string Category { get; set; }
}
