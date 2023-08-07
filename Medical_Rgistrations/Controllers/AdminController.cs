using DBAccess.Utility;
using Medical_Rgistrations.APIModels;
using Medical_Rgistrations.ControllerBase;
using Medical_Rgistrations.RestSharpContext;
using Medical_Rgistrations.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Buffers.Text;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Medical_Rgistrations.Controllers
{
    public class AdminController : BasePageController
    {

        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _Config;
        private static string Message;
        ApiResponse apiResponse;
        public AdminController(IConfiguration config, IHostingEnvironment hostingEnvironment) : base(config)
        {

            this._hosting = hostingEnvironment;
            this._Config = config;
        }


    
        public async Task<ApiResponse> GetDashboardTable()
        {
            apiResponse = new ApiResponse();
            try
            {

                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Registration/GetAllRegistrations");

                var response = await restClient.GetAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    return apiResponse;
                }
            }
            catch (Exception e)
            {

            }
            return apiResponse;
        }

        public IActionResult Dashboard()
        {
            IEnumerable<RegisterViewModels> model = new List<RegisterViewModels>();
            return View(model);
        }


        [Route("Admin-DS-Contact")]
        [HttpGet]
        public async Task<IActionResult> ContactMaster()
        {
            ViewBag.message = Message;
            Message = string.Empty;
            return View();
        }
        [Route("Admin-DS-About")]
        [HttpGet]
        public async Task<IActionResult> AboutMaster()
        {
            ViewBag.message = Message;
            Message = string.Empty;
            return View();
        }

        [Route("Admin-DS-Course")]
        [HttpGet]
        public async Task<IActionResult> CourseMaster()
        {
            ViewBag.message = Message;
            return View();
        }


        [Route("Admin-SetTemplate")]
        [HttpPost]
        public async Task<ApiResponse> SetMaster(IFormCollection formdata)
        {
            apiResponse = new ApiResponse();

            var base64 = formdata["htmlData"];
            var pageName = formdata["page"];
            var templateName = formdata["templateName"];

            var templateUrl = "";

            switch (pageName)
            {
                case "about":
                    templateUrl = "/Template/SetAboutTemplate";
                    break;
                case "contact":
                    templateUrl = "/Template/SetContactTemplate";
                    break;
                case "course":
                    templateUrl = "/Template/SetCourseTemplate";
                    break;
                //case "faculty":
                //    templateUrl = "/Admin/SetFacultyTemplate";
                //    break;
                default:
                    break;
            }


            var bytes = Convert.FromBase64String(base64);
            var html = System.Text.Encoding.UTF8.GetString(bytes);

            MyHtmlContent htmlContent = new MyHtmlContent { HtmlData = html, TemplateName = templateName };

            RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance(templateUrl);

            restsharpClient._request.AddJsonBody(JsonConvert.SerializeObject(htmlContent));

            var response = await restClient.PostAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                return apiResponse;
            }

            return apiResponse;
        }


        public async Task<string> Upload(IFormFile file)
        {

            string filename = null;

            filename = Guid.NewGuid() + file.FileName.Trim();

            string uploads = Path.Combine(_hosting.WebRootPath, "faculties images");

            if (!Directory.Exists(uploads))
            {
                Directory.CreateDirectory(uploads);
            }

            if (file.Length > 0)
            {
                string filePath = Path.Combine(uploads, filename);
                using (Stream fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return filename;
        }


        [Route("Admin-UpdateTemplate")]
        [HttpPost]
        public async Task<IActionResult> UpdateTemplate(MyHtmlContent template,string pageName)
        {
            var templateUrl = "";

            switch (pageName)
            {
                case "about":
                    templateUrl = "/Template/UpdateAboutTemplate";
                    break;
                case "contact":
                    templateUrl = "/Template/UpdateContactTemplate";
                    break;
                case "course":
                    templateUrl = "/Template/UpdateCourseTemplate";
                    break;
                //case "faculty":
                //    templateUrl = "/Admin/SetFacultyTemplate";
                //    break;
                default:
                    break;
            }

            apiResponse = new ApiResponse();
            ViewBag.message = "";
            var filename = string.Empty;
            if (ModelState.IsValid)
            {

                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);


                var requestData = new MyHtmlContent
                {
                    Id = template.Id,
                    Active = template.Active,
                    HtmlData = template.HtmlData,
                    TemplateName = template.TemplateName,

                };


                var restClient = await restsharpClient.GetClientInstance(templateUrl);

                restsharpClient._request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(requestData));

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {

                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    return Json(apiResponse);
                }
            }

            return Json("Invalid request");
        }

        /// <summary>
        /// get all contact templates
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> PopulateTemplate(string pageName)
        {


            var templateUrl = "";

            switch (pageName)
            {
                case "about":
                    templateUrl = "/Template/GetAboutTemplate";
                    break;
                case "contact":
                    templateUrl = "/Template/GetContactTemplate";
                    break;
                case "course":
                    templateUrl = "/Template/GetCourseTemplate";
                    break;
                //case "faculty":
                //    templateUrl = "/Admin/SetFacultyTemplate";
                //    break;
                default:
                    break;
            }

            var jsonresponse = new ApiResponse();
            List<Faculty> faculties = new List<Faculty>();

            RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance(templateUrl);

            var response = await restClient.GetAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                jsonresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                //if (Apiresponse != null && Apiresponse.Success)
                //{
                //    faculties = JsonConvert.DeserializeObject<List<Faculty>>(Apiresponse.Data);

                //    jsonresponse.Success = true;
                //    jsonresponse.Data = Apiresponse.Data;
                //}
            }
            return jsonresponse;
        }

    }
}
