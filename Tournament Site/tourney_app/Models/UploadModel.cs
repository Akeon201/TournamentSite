using System;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace team_management_app.Models
{
    public class UploadModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;

        public UploadModel(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnPost(IFormFile videoFile, string title)
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                // ... (upload logic)

                // Redirect to the VodsLibrary page after upload
                return RedirectToPage("/VodsLibrary");
            }

            // Handle the case where no file was uploaded
            // You can customize this part based on your requirements.
            return Page();
        }


    }
}