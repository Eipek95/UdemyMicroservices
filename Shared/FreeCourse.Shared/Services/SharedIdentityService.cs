using Microsoft.AspNetCore.Http;

namespace FreeCourse.Shared.Services
{
    public class SharedIdentityService : ISharedIdentityService
    {
        private IHttpContextAccessor _contextAccessor;

        public SharedIdentityService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        //_contextAccessor.HttpContext.User.Claims.Where(x => x.Type == "sub").FirstOrDefault().Value ->alternatif değer okuma
        public string GetUserId { get => _contextAccessor.HttpContext.User.FindFirst("sub").Value; }
        //amaç tokenla kullanıcının bilgilerine ulaşıp id sini almak.sub ile tutulan id
    }
}
