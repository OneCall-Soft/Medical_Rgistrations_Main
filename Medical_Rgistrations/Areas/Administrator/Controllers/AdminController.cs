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
using System.Linq.Expressions;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Medical_Rgistrations.Areas.Admin.Controllers
{

    [Area("Administrator")]
    public class AdminController : BasePageController
    {

        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _Config;
        private static string Message;
        ApiResponse apiResponse;
        public AdminController(IConfiguration config, IHostingEnvironment hostingEnvironment) : base(config)
        {

            _hosting = hostingEnvironment;
            _Config = config;
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
                throw e;
            }
            return apiResponse;
        }

        public IActionResult Dashboard()
        {
            IEnumerable<RegisterViewModels> model = new List<RegisterViewModels>();
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }


        [Route("Admin-DS-Contact")]
        [HttpGet]
        public async Task<IActionResult> ContactMaster()
        {
            try
            {
                ViewBag.message = Message;
                Message = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }
        [Route("Admin-DS-About")]
        [HttpGet]
        public async Task<IActionResult> AboutMaster()
        {
            try
            {
                ViewBag.message = Message;
                Message = string.Empty;
            }
            catch (Exception)
            {

                throw;
            }

            return View();
        }

        [Route("Admin-DS-Course")]
        [HttpGet]
        public async Task<IActionResult> CourseMaster()
        {
            try
            {
                ViewBag.message = Message;

            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Template()
        {
            try
            {
                var htmlModel = new MyHtmlContent();
                htmlModel.PagesGroup = await PopulateImageGroup();
                ViewBag.message = Message;

                return View(htmlModel);

            }
            catch (Exception)
            {
                throw;
            }

        }


        [Route("Admin-SetTemplate")]
        [HttpPost]
        public async Task<ApiResponse> SetMaster(MyHtmlContent formdata)
        {

            apiResponse = new ApiResponse();

            try
            {
                var base64 = formdata.HtmlData;
                //var pageName = formdata["page"];
                //var templateName = formdata["templateName"];
                //var isActive = formdata["active"];

                var templateUrl = "/Template/SetTemplate"; ;

                var bytes = Convert.FromBase64String(base64);
                var html = Encoding.UTF8.GetString(bytes);

                MyHtmlContent htmlContent = new MyHtmlContent { Active = formdata.Active, HtmlData = html, TemplateName = formdata.TemplateName, Page = formdata.Page.ToString().ToLower() };

                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance(templateUrl);

                restsharpClient._request.AddJsonBody(JsonConvert.SerializeObject(htmlContent));

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        apiResponse = new ApiResponse { Success = true, Message = "Template has been added" };

                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            

            return apiResponse;
        }


        public async Task<string> Upload(IFormFile file)
        {
            string filename = null;
            try
            {
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
            }
            catch (Exception)
            {

                throw;
            }

            

           
            return filename;
        }


        [Route("Admin-UpdateTemplate")]
        [HttpPost]
        public async Task<IActionResult> UpdateTemplate(MyHtmlContent template)
        {
            var templateUrl = "/Template/UpdateTemplate";

            apiResponse = new ApiResponse();
            ViewBag.message = "";
            var filename = string.Empty;

            try
            {
                if (ModelState.IsValid)
                {

                    RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                    restsharpClient.SetBasicAuthenticator(api_username, api_password);


                    var requestData = new MyHtmlContent
                    {
                        Id = template.Id,
                        Active = template.Active,
                        HtmlData = template.HtmlData,
                        TemplateName = template.TemplateName,
                        Page = template.Page
                    };


                    var restClient = await restsharpClient.GetClientInstance(templateUrl);

                    restsharpClient._request.AddJsonBody(JsonConvert.SerializeObject(requestData));

                    var response = await restClient.PostAsync(restsharpClient._request);

                    if (response.IsSuccessStatusCode)
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            apiResponse = new ApiResponse { Message = JsonConvert.DeserializeObject<string>(response.Content), Success = true };
                        }
                        else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                        }
                        return Json(apiResponse);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           

            return Json("Invalid request");
        }

        /// <summary>
        /// get all contact templates
        /// </summary>
        /// <returns></returns>
        public async Task<ApiResponse> PopulateTemplate(string pageName)
        {
            var templateUrl = "/Template/GetTemplates?page=" + pageName;

             apiResponse = new ApiResponse();
            List<Faculty> faculties = new List<Faculty>();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance(templateUrl);

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        apiResponse = new ApiResponse { Success = true };
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        apiResponse = new ApiResponse { Message = JsonConvert.DeserializeObject<string>(response.Content), Success = true };
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

           
            return apiResponse;
        }

        public async Task<List<string>> PopulateImageGroup()
        {
            List<string> imageGroups = new List<string>();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Gallary/GetImagesGroups");

                var response = await restClient.GetAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    imageGroups = JsonConvert.DeserializeObject<List<string>>(response.Content);

                }
            }
            catch (Exception)
            {

                throw;
            }

           
            return imageGroups;
        }
    }
}
