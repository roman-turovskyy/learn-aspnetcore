namespace Example.Domain.Entities;

public class PacAuthorization : AuthorizationByProduct
{
    public DateTime AdmitDate { get; set; }
    public bool? Pacman { get; set; }
}
