using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Online_book_shop.Startup))]
namespace Online_book_shop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
