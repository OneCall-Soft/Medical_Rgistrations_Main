using Microsoft.AspNetCore.Mvc;

namespace Medical_Rgistrations.ControllerBase
{
    public abstract class BasePageController : Controller
    {
        public string apiBaseUrl { get; set; }
        public string api_username { get; set; }
        public string api_password { get; set; }

        public BasePageController()
        {
            
        }
        public BasePageController(IConfiguration configuration)
        {
            //this.apiBaseUrl = configuration.GetSection("LiveUrl").Value;
            this.apiBaseUrl = configuration.GetSection("DevUrl").Value;
            this.api_username = configuration.GetSection("api_credentials:username").Value;
            this.api_password = configuration.GetSection("api_credentials:password").Value;
        }
    }
}
