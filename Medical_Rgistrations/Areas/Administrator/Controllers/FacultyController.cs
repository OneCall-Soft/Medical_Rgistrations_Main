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
    [Area("Administrator")]
    public class FacultyController : BasePageController
    {

        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _Config;
        private static string Message;
        ApiResponse apiResponse;
        public FacultyController(IConfiguration config, IHostingEnvironment hostingEnvironment) : base(config)
        {

            this._hosting = hostingEnvironment;
            this._Config = config;
        }


     
        [HttpPost]
        public async Task<ApiResponse> DeleteFaculty(string id)
        {
            apiResponse = new ApiResponse();
            try
            {

                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Faculty/RemoveFacultyById/" + id);

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }
            }
            catch (Exception e)
            {
                apiResponse.Success = false;
                apiResponse.Message = e.Message;

                return apiResponse;
            }

            return apiResponse;
        }

        [HttpPost]
        public async Task<JsonResult> FacultyActive(MassActive massActive)
        {
            apiResponse = new ApiResponse();
            try
            {

                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Faculty/MassUpdate");
                restsharpClient._request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(massActive));
                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }
            }
            catch (Exception e)
            {
                apiResponse.Success = false;
                apiResponse.Message = e.Message;

                return Json(apiResponse);
            }

            return Json(apiResponse);
        }
              

       
        [HttpGet]
        public async Task<IActionResult> FacultyMaster()
        {
            var model = new FacultyViewModel();

            ViewBag.message = Message;

            return View(model);
        }
        //[Route("PostFaculty")]
        [HttpPost]
        public async Task<IActionResult> FacultyMaster(FacultyViewModel faculty)
        {

            //faculty.Faculties=await PopulateFaculties();

            if (ModelState.IsValid)
            {

                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var filename = await Upload(faculty.ProfileImg);

                var requestData = new Faculty
                {
                    Name = faculty.Name,
                    Description = faculty.Description,
                    Email = faculty.Email,
                    ProfileName = filename,
                    InstituteName = faculty.InstituteName,
                    Active = faculty.Active
                };


                var restClient = await restsharpClient.GetClientInstance("/Faculty/NewFaculty");

                restsharpClient._request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(requestData));

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    if (apiResponse != null && apiResponse.Success)
                    {
                        Message = apiResponse.Message;
                        return RedirectToAction("FacultyMaster");
                    }
                }
            }

            return View(faculty);
        }

        [HttpGet]
        public async Task<IActionResult> EditFaculty(string id)
        {
            ViewBag.message = "";

            FacultyEditViewModel model = new FacultyEditViewModel();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Faculty/GetFacultyById/" + id);

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    if (Apiresponse != null && Apiresponse.Success)
                    {
                        ViewBag.message = Apiresponse.Message;

                        var res = JsonConvert.DeserializeObject<Faculty>(Apiresponse.Data);

                        model = new FacultyEditViewModel
                        {
                            FacultyId = res.FacultyId,
                            Description = res.Description,
                            Email = res.Email,
                            Name = res.Name,
                            InstituteName = res.InstituteName,
                            ProfileName = res.ProfileName,
                            Active = res.Active,
                        };

                        return View(model);
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }

           
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> DeleteFaculty(string id)
        //{
        //    ViewBag.message = "";

        //    FacultyViewModel model = new FacultyViewModel();

        //    RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

        //    restsharpClient.SetBasicAuthenticator(api_username, api_password);

        //    var restClient = await restsharpClient.GetClientInstance("/Faculty/RemoveFacultyById/" + id);

        //    var response = await restClient.PostAsync(restsharpClient._request);

        //    if (response.IsSuccessStatusCode)
        //    {
        //        var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
        //        if (Apiresponse != null && Apiresponse.Success)
        //        {
        //            ViewBag.message = Apiresponse.Message;

        //            var res = JsonConvert.DeserializeObject<bool>(Apiresponse.Data);

        //            return RedirectToAction("FacultyMaster", "Admin");
        //        }
        //    }

        //    return View();
        //}

        [HttpPost]
        public async Task<IActionResult> UpdateFaculty(FacultyEditViewModel faculty)
        {
            ViewBag.message = "";
            var filename = string.Empty;
            try
            {
                if (ModelState.IsValid)
                {

                    RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                    restsharpClient.SetBasicAuthenticator(api_username, api_password);

                    if (faculty.ProfileImg == null)
                    {
                        filename = faculty.ProfileName;
                    }
                    else
                    {
                        filename = await Upload(faculty.ProfileImg);
                    }

                    var requestData = new Faculty
                    {
                        FacultyId = faculty.FacultyId,
                        Name = faculty.Name,
                        Description = faculty.Description,
                        Email = faculty.Email,
                        ProfileName = filename ?? faculty.ProfileName,
                        InstituteName = faculty.InstituteName,
                        Active = faculty.Active,
                    };


                    var restClient = await restsharpClient.GetClientInstance("/Faculty/UpdateFaculty");

                    restsharpClient._request.AddJsonBody(Newtonsoft.Json.JsonConvert.SerializeObject(requestData));

                    var response = await restClient.PostAsync(restsharpClient._request);

                    if (response.IsSuccessStatusCode)
                    {
                        var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                        ViewBag.message = Apiresponse.Message;
                        if (Apiresponse != null && Apiresponse.Success)
                        {

                            var res = JsonConvert.DeserializeObject<bool>(Apiresponse.Data);
                            return RedirectToAction("FacultyMaster");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

           

            return View("EditFaculty", faculty);
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


        public async Task<ApiResponse> PopulateFaculties()
        {
            var jsonresponse = new ApiResponse();
            List<Faculty> faculties = new List<Faculty>();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(base.apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Faculty/GetFaculties");

                var response = await restClient.GetAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    if (Apiresponse != null && Apiresponse.Success)
                    {
                        faculties = JsonConvert.DeserializeObject<List<Faculty>>(Apiresponse.Data);

                        jsonresponse.Success = true;
                        jsonresponse.Data = Apiresponse.Data;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
           
            return jsonresponse;
        }
      
    }
}
