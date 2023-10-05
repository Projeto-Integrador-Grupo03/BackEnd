using ecommerce_iniciativatena.Model;

namespace ecommerce_iniciativatena.Security
{
    public interface IAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}
