using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using trsaints_frontend_api.Data.Entities;

namespace trsaints_frontend_api.Data.DTOs;

public class TechStackDTO
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    public string Name { get; set; }
    
    [JsonIgnore]
    public IEnumerable<Skill>? Skills { get; set; }
}
