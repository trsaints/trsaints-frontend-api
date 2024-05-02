namespace trsaints_frontend_api.Entities;

public class Project: Entity
{
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; }
    public string RepoUrl { get; set; }
    public string DeployUrl { get; set; }
    public string Banner { get; set; }
    
    public int StackId { get; set; }
    public TechStack TechStack { get; set; }
}
