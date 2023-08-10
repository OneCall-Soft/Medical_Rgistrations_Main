using DBAccess.Utility;
using DNTCaptcha.Core;
using Medical_Rgistrations.APIModels;
using Medical_Rgistrations.ControllerBase;
using Medical_Rgistrations.RestSharpContext;
using Medical_Rgistrations.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using RestSharp;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace Medical_Rgistrations.Controllers
{
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


        public async Task<IActionResult> Index()
        {
            MyHtmlContent model = new MyHtmlContent();
            apiResponse = new ApiResponse();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Template/GetActiveTemplate?pageName=home");

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }


                if (apiResponse.Data != null)
                    model = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);

                restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                restClient = await restsharpClient.GetClientInstance("/Gallary/GetImagesByGroup/home");

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


                restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                restClient = await restsharpClient.GetClientInstance("/Template/GetDashboardLinks");

                response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    if (apiResponse.Success)
                    {
                        var dashboardLinks = JsonConvert.DeserializeObject<DashboardLinkView>(apiResponse.Data);

                        if (dashboardLinks != null)
                        {
                            model.ImportentNotification = dashboardLinks.NotificationLink;
                            model.DownloadLinks = dashboardLinks.DownloadLink;
                        }
                    }

                }

            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Dashboard(string id)
        {

            var model = new Registrations();
            try
            {
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
                        ViewData["reference"] = model.RferenceNbr;
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

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

                var templateUrl = "/Template/GetActiveTemplate?pageName=tnc";

                apiResponse = new ApiResponse();
                List<Faculty> faculties = new List<Faculty>();


                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance(templateUrl);

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    }
                    model.HtmlContent = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);
                }


            }
            catch (Exception e)
            {

            }

            return View(model);
        }

        public async Task<MyHtmlContent> GetDashboardTemplate()
        {
            var model = new MyHtmlContent();

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);
            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Template/GetActiveTemplate?pageName=getintouch");

            var response = await restClient.PostAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                if (apiResponse.Success)
                {
                    model = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);
                }

            }
            return model;
        }

        public async Task<MyHtmlContent> GetDashboardLinks()
        {
            var model = new MyHtmlContent();

            RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);
            restsharpClient.SetBasicAuthenticator(api_username, api_password);

            var restClient = await restsharpClient.GetClientInstance("/Template/GetDashboardLinks");

            var response = await restClient.PostAsync(restsharpClient._request);

            if (response.IsSuccessStatusCode)
            {
                apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                if (apiResponse.Success)
                {
                    var dashboardLinks = JsonConvert.DeserializeObject<DashboardLinkView>(apiResponse.Data);

                    if (dashboardLinks != null)
                    {
                        model.ImportentNotification = dashboardLinks.NotificationLink;
                        model.DownloadLinks = dashboardLinks.DownloadLink;
                        model.QuickLinks = dashboardLinks.FooterLink1;
                        model.ImportantLinks = dashboardLinks.FooterLink2;
                    }
                }

            }

            return model;
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

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Template/GetActiveTemplate?pageName=contact");

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    var Apiresponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                    if (Apiresponse != null && Apiresponse.Success)
                    {
                        model = JsonConvert.DeserializeObject<MyHtmlContent>(Apiresponse.Data);
                        return View(model);
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View(model);
        }


        public async Task<IActionResult> Course()
        {
            MyHtmlContent model = new MyHtmlContent();
            apiResponse = new ApiResponse();

            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Template/GetActiveTemplate?pageName=course");

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }

                if (apiResponse.Data != null)
                    model = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);

                restsharpClient = new RestsharpClient(apiBaseUrl);


                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                restClient = await restsharpClient.GetClientInstance("/Gallary/GetImagesByGroup/course");

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
            }
            catch (Exception)
            {
                throw;
            }

            return View(model);
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
                throw;
            }

            return View(model);
        }



        public async Task<IActionResult> AboutUs()
        {
            var model = new MyHtmlContent();
            apiResponse = new ApiResponse();
            try
            {
                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Template/GetActiveTemplate?pageName=about");

                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);
                }


                if (apiResponse.Data != null)
                    model = JsonConvert.DeserializeObject<MyHtmlContent>(apiResponse.Data);

                restsharpClient = new RestsharpClient(apiBaseUrl);

                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                restClient = await restsharpClient.GetClientInstance("/Gallary/GetImageGetImagesByGroup/about");

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
            try
            {
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
            }
            catch (Exception)
            {
                throw;
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

            try
            {
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
            catch (Exception)
            {
                throw;
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
