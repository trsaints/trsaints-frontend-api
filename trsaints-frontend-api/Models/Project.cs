namespace trsaints_frontend_api.Models;

public class Project
{
    public int Id
    {
        get;
        set;
    }
    public string Name
    {
        get;
        set;
    }

    public string Description
    {
        get;
        set;
    }

    public string RepoUrl
    {
        get;
        set;
    }
    
    public string DeployUrl
    {
        get;
        set;
    }

    public string[] Stack
    {
        get;
        set;
    }

    public string Banner
    {
        get;
        set;
    }
}
