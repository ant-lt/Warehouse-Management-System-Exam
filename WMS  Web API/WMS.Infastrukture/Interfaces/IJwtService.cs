namespace WMS.Infastructure.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(int userId, string role);
    }
}
