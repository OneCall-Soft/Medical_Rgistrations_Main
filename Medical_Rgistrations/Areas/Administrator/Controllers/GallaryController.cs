using Medical_Rgistrations.APIModels;
using Medical_Rgistrations.ControllerBase;
using Medical_Rgistrations.RestSharpContext;
using Medical_Rgistrations.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using System.Drawing;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;


namespace Medical_Rgistrations.Controllers
{
    [Area("Administrator")]
    public class GallaryController : BasePageController
    {
        private readonly IHostingEnvironment _hosting;
        private readonly IConfiguration _Config;
        private static string Message;
        ApiResponse apiResponse;

        public GallaryController(IConfiguration config, IHostingEnvironment hostingEnvironment) : base(config)
        {

            this._hosting = hostingEnvironment;
            this._Config = config;
        }

        public async Task<IActionResult> GallaryMaster()
        {
            var model = new GallaryViewModel();
            model.ImageGroups = await PopulateImageGroup();

            ViewBag.message = Message;
            Message=string.Empty;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> GallaryMaster(GallaryViewModel gallary)
        {
            apiResponse = new ApiResponse();
            gallary.ImageGroups = await PopulateImageGroup();
            try
            {
                var imageNames = "";

                for (int i = 0; i < gallary.imageFiles.Count; i++)
                {
                    //imageNames += Guid.NewGuid()+"_"+gallary.imageFiles[i].FileName;
                   var img= await Upload(gallary.imageFiles[i], "Image_Gallary");

                    if (i != gallary.imageFiles.Count - 1)
                    {
                        imageNames += img+"#";
                    }
                    else {
                        imageNames += img;
                    }
                }



                var galaryToBeUploaded = new Gallary
                {
                    fileName = imageNames,
                    groupName = gallary.groupName,
                    groupId = Guid.NewGuid(),
                    id = Guid.NewGuid(),
                };

                RestsharpClient restsharpClient = new RestsharpClient(apiBaseUrl);


                restsharpClient.SetBasicAuthenticator(api_username, api_password);

                var restClient = await restsharpClient.GetClientInstance("/Gallary/SetImagesByGroup");
                restsharpClient._request.AddJsonBody(JsonConvert.SerializeObject(galaryToBeUploaded));
                var response = await restClient.PostAsync(restsharpClient._request);

                if (response.IsSuccessStatusCode)
                {
                    apiResponse = JsonConvert.DeserializeObject<ApiResponse>(response.Content);

                    Message = apiResponse.Message;

                    return RedirectToAction("GallaryMaster","Gallary");
                }
            }
            catch (Exception e)
            {

            }

            return View(gallary);
        }
       
        /// <summary>
        /// Save image files
        /// </summary>
        /// <param name="file"></param>
        /// <param name="folderName">folder name where fuiles to be saved</param>
        /// <returns>saved image name </returns>
        public async Task<string> Upload(IFormFile file, string folderName)
        {

            string filename = null;

            try
            {
                filename = Guid.NewGuid() + file.FileName.Trim();



                string uploads = Path.Combine(_hosting.WebRootPath, folderName);

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
