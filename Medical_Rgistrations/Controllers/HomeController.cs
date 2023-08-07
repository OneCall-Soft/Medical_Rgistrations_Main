using DBAccess.Utility;
using DNTCaptcha.Core;
using Medical_Rgistrations.APIModels;
using Medical_Rgistrations.ControllerBase;
using Medical_Rgistrations.RestSharpContext;
using Medical_Rgistrations.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Medical_Rgistrations.Controllers
{
    [Area("Admin")]
    public class HomeController : BasePageController
    {

        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _Config;
        private List<Qualification> qualificationsList;
        private ApiResponse? apiResponse;

        public HomeController(IHostingEnvironment hostingEnvironment, IConfiguration config) : base(config)
        {
            _hosting = hostingEnvironment;
            _Config = config;
        }


        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string id)
        {

            var model = new Registrations();
            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Registration/GetById/" + id);

            var response = await restClient.PostAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                var apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                if (apiresponse != null && apiresponse.Success)
                {
                    model = JsonConvert.DeserializeObject<Registrations>(apiresponse.Data);
                }
            }

            ViewData["reference"] = model.RferenceNbr;

            return View(model);
        }

        public async Task<IActionResult> Registration()
        {
            RegisterViewModels model = new RegisterViewModels();
            try
            {
                await Contractions(model);
                await Qualifications(model);
                model.allYears = AllYears();


            }
            catch (Exception e)
            {

            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registration(RegisterViewModels model)
        {
            await Contractions(model);
            await Qualifications(model);
            model.allYears = AllYears();

            try
            {
                if (ModelState.IsValid)
                {
                    APIRegistration aPIRegistration = new APIRegistration
                    {
                        Address = model.Address,
                        GenderId = Convert.ToInt32(model.GenderId),
                        InstituteName = model.InstituteName,
                        Email = model.Email,
                        Year = model.Year,
                        Name = model.Name,
                        Mobile = model.Mobile,
                        QualificationId = Convert.ToInt32(model.QualificationId),
                        CompletePart = model.CompletePart,
                        Nationality = model.Nationality,
                        DOB = model.DOB,
                        Pincode = model.Pincode,
                        RCPCode = model.RCPCode

                    };

                    RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                    restsharpClient.SetBasicAuthenticator(api_username, api_password);

                    var restClient = await restsharpClient.GetClientInstance("/Registration/NewRgistration");

                    restsharpClient._request.AddJsonBody(JsonConvert.SerializeObject(aPIRegistration));

                    var response = await restClient.PostAsync(restsharpClient._request);

                    if (response.IsSuccessStatusCode)
                    {
                        var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                        if (Apiresponse != null && Apiresponse.Success)
                        {
                            var data = JsonConvert.DeserializeObject<Registrations>(Apiresponse.Data);

                            return RedirectToAction(nameof(Dashboard),
                                new { id = data.RegistrationId }
                            );
                        }
                    }

                }
            }
            catch (Exception e)
            {

            }

            return View(model);
        }


        public async Task<IActionResult> ContactUs()
        {
            MyHtmlContent model = new MyHtmlContent();

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Template/GetContactActiveTemplate");

            var response = await restClient.GetAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                if (Apiresponse != null && Apiresponse.Success)
                {
                    model = JsonConvert.DeserializeObject<MyHtmlContent>(Apiresponse.Data);
                    return View(model);
                }
            }
            return View();
        }


        public async Task<IActionResult> Course()
        {
            MyHtmlContent model = new MyHtmlContent();
            apiResponse = new ApiResponse();

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Template/GetCourseActiveTemplate");

            var response = await restClient.GetAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                if (apiResponse != null && apiResponse.Success)
                {

                    model = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);

                    restsharpClient = new RestsharpClient(apiBaseUrl);


                    restsharpClient.SetBasicAuthenticator(api_username, api_password);

                    restClient = await restsharpClient.GetClientInstance("/Gallary/GetImagesByGroup/" + model.GallaryGroup);

                    response = await restClient.PostAsync(restsharpClient._request);

                    if (response.IsSuccessStatusCode)
                    {
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                        if (apiResponse.Success)
                        {
                            var gallary = JsonConvert.DeserializeObject<Gallary>(apiResponse.Data);
                            model.imgages = gallary;
                        }

                    }

                    return View(model);
                }
            }
            return View();
        }


        public async Task<IActionResult> Faculty()
        {
            var model = new List<FacultyViewModel>();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Faculty/GetFaculties");

                var response = await restClient.GetAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    if (Apiresponse != null && Apiresponse.Success)
                    {
                        model = JsonConvert.DeserializeObject<List<FacultyViewModel>>(Apiresponse.Data);
                        return View(model);
                    }
                }
            }
            catch (Exception e)
            {

            }

            return View();
        }



        public async Task<IActionResult> AboutUs()
        {
            var model = new MyHtmlContent();
            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Template/GetAboutActiveTemplate");

                var response = await restClient.GetAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    if (Apiresponse != null && Apiresponse.Success)
                    {

                        model = JsonConvert.DeserializeObject<MyHtmlContent>(Apiresponse.Data);

                        restsharpClient = new RestsharpClient(apiBaseUrl);


                        restsharpClient.SetBasicAuthenticator(api_username, api_password);

                        restClient = await restsharpClient.GetClientInstance("/Gallary/GetImagesByGroup/" + model.GallaryGroup);

                        response = await restClient.PostAsync(restsharpClient._request);

                        if (response.IsSuccessStatusCode)
                        {
                            apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                            if (apiResponse.Success)
                            {
                                var gallary = JsonConvert.DeserializeObject<Gallary>(apiResponse.Data);
                                model.imgages = gallary;
                            }

                        }

                        return View(model);
                    }
                }
            }
            catch (Exception e)
            {

            }
            return View(model);
        }


        /// <summary>
        /// fetching genders and filling up in model list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Contractions(RegisterViewModels model)
        {

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);


            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Qualifications/GetGenders");
            var resp = await restClient.GetAsync(restsharpClient._request);

            //var response = await client.GetAsync(base.apiBaseUrl + "/Contraction/GetAllContraction");

            if (resp.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<ApiResponse>(resp.Content);


                if (res.Success)
                {
                    var genders = JsonConvert.DeserializeObject<List<Genders>>(res.Data);
                    model.GenderList.AddRange(genders);
                }
            }


            //return solsList;
        }
        /// <summary>
        /// fetching qualification and filling up in model list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task Qualifications(RegisterViewModels model)
        {

            qualificationsList = new List<Qualification>();

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Qualifications/GetAll");
            var resp = await restClient.GetAsync(restsharpClient._request);

            if (resp.IsSuccessStatusCode)
            {
                var res = JsonConvert.DeserializeObject<List<Qualification>>(resp.Content);

                foreach (var item in res)
                {
                    model.QualificationList.Add(new Qualification { Id = item.Id, Value = item.Value });
                }
            }
        }

        /// <summary>
        /// years list
        /// </summary>
        /// <returns>return year list of string</returns>
        static List<string> AllYears()
        {
            List<string> years = new List<string>();

            var date = DateTime.Parse("1991-01-01");
            for (var year = date.Year; year < DateTime.Now.Year; year++)
                years.Add(Convert.ToString(year));

            return years;
        }
    }





}
