using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.DTOs;

public class SkillDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(128)]
    [DisplayName("Name")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Category is required")]
    [DisplayName("Category")]
    public SkillCategory SkillCategory { get; set; }
    
    [JsonIgnore]
    public IEnumerable<TechStack>? TechStacks { get; set; }
}
