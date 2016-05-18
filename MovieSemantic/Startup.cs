using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MovieSemantic.Startup))]
namespace MovieSemantic
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
