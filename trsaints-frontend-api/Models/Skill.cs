namespace trsaints_frontend_api.Models;

public class Skill(int id, string name, string category)
{
    public int Id
    {
        get => id;
        set => id = value;
    }
    public string Name
    {
        get => name;
        set => name = value;
    }

    public string Category
    {
        get => category;
        set => category = value;
    }
}