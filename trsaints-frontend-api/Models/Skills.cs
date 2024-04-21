namespace trsaints_frontend_api.Models;

public class Skills(string name, string category)
{
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