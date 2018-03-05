using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MoviePlotQuiz.Startup))]
namespace MoviePlotQuiz
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
