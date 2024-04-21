namespace trsaints_frontend_api.Models;

public class Project(string name, string description, string url, string[] skills, string banner)
{
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Description
    {
        get => description;
        set => description = value;
    }

    public string RepoUrl
    {
        get => url;
        set => url = value;
    }
    
    public string DeployUrl
    {
        get => url;
        set => url = value;
    }

    public string[] Skills
    {
        get => skills;
        set => skills = value;
    }

    public string Banner
    {
        get => banner;
        set => banner = value;
    }
}
