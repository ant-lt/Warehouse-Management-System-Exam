namespace WMS.Infastrukture.Interfaces
{
    public interface IJwtService
    {
        string GetJwtToken(int userId, string role);
    }
}
