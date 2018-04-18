using Microsoft.AspNet.Identity.EntityFramework;

namespace HSG.Exception.Web
{
    public class ExceptionDbContext : IdentityDbContext<ApplicationUser>
    {
        public ExceptionDbContext()
            : base("DefaultConnection", false)
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public static ExceptionDbContext Create()
        {
            return new ExceptionDbContext();
        }
    }
}