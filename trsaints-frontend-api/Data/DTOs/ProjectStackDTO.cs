using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace trsaints_frontend_api.Data.DTOs;

public class ProjectStackDTO
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Name")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Date is required")]
    [DataType(DataType.Text)]
    [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
    [DisplayName("Description")]
    public string Date { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    [MinLength(3)]
    [MaxLength(100)]
    [DisplayName("Name")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(4)]
    [MaxLength(200)]
    [DisplayName("Repo URL")]
    public string RepoUrl { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(4)]
    [MaxLength(200)]
    [DisplayName("Deploy URL")]
    public string DeployUrl { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [MinLength(4)]
    [MaxLength(250)]
    [DisplayName("Banner URL")]
    public string Banner { get; set; }
    
    public string StackName { get; set;}
}
