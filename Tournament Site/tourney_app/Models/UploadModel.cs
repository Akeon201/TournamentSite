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

        public async Task OnPostAsync(IFormFile videoFile, string title)
        {
            if (videoFile != null && videoFile.Length > 0)
            {
                string containerName = "vodsfortesting"; // Replace with your container name
                BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

                string fileName = Guid.NewGuid() + Path.GetExtension(videoFile.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(fileName);

                using (Stream stream = videoFile.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, overwrite: true);
                }

                // Store the title as a metadata key-value pair
                await blobClient.SetMetadataAsync(new Dictionary<string, string>
                {
                    { "Title", title }
                });

                // Redirect to the VodsLibrary page after upload
                RedirectToPage("/VodsLibrary");
            }
        }
    }
}
