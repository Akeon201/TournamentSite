using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Storage.Blobs;
using System.Threading.Tasks;

namespace team_management_app.Models
{
    public class VodsLibraryModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;

        public List<string> Videos { get; set; } = new List<string>();

        public VodsLibraryModel(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task OnGetAsync()
        {
            var containerName = "vodsfortesting"; // Replace with your container name
            var containerClient = _blobServiceClient.GetBlobContainerClient(containerName);

            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                if (blobItem.Name.EndsWith(".mp4", StringComparison.OrdinalIgnoreCase))
                {
                    Videos.Add(blobItem.Name);
                }
            }
        }
    }
}
