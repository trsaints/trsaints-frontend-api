using trsaints_frontend_api.Validation;

namespace trsaints_frontend_api.Data.Entities;
public sealed class TechStack: Entity
{
    public string? Name { get; private set; }

    public TechStack(string name)
    {
        ValidateDomain(name);
    }

    public TechStack(int id, string name)
    {
        DomainExceptionValidation.When(id < 0, "Invalid Id value");
        Id = id;
        ValidateDomain(name);
    }

    public void Update(string name)
    {
        ValidateDomain(name);
    }

    private void ValidateDomain(string name)
    {
        DomainExceptionValidation.When(string.IsNullOrEmpty(name), "Name is required");
        
        DomainExceptionValidation.When(name.Length < 3, "Name too short");

        Name = name;
    }
    
    public IEnumerable<Project> Projects { get; set; }
}
