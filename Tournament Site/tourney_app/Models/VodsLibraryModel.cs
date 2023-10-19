using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace team_management_app.Models
{
    public class VodsLibraryModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public VodsLibraryModel(IConfiguration configuration)
        {
            _configuration = configuration;
            VideoList = new List<Video>(); // Initialize VideoList
        }

        public List<Video> VideoList { get; set; }

        public async Task OnGetAsync()
        {
            VideoList = await GetVideoListAsync();
        }

        public class Video
        {
            public string Title { get; set; } = string.Empty;
            public string Url { get; set; } = string.Empty;
        }

        private async Task<List<Video>> GetVideoListAsync()
        {
            var connectionString = _configuration.GetConnectionString("AzureBlobStorageConnection");
            var containerName = "vodsfortesting";

            var blobServiceClient = new BlobServiceClient(connectionString);
            var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            var videos = new List<Video>();
            await foreach (var blobItem in containerClient.GetBlobsAsync())
            {
                videos.Add(new Video
                {
                    Title = blobItem.Name, // Use the blob name as the title, or provide a custom title
                    Url = containerClient.GetBlobClient(blobItem.Name).Uri.ToString()
                });
            }

            return videos;
        }
    }
}