﻿using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace team_management_app.Models
{
    public class VideoInfo
    {
        public string? Name { get; set; }
        public string? BlobUri { get; set; }
    }

    public class VodsLibraryModel : PageModel
    {
        private readonly BlobServiceClient _blobServiceClient;

        public List<VideoInfo> Videos { get; set; } = new List<VideoInfo>();

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
                    var blobClient = containerClient.GetBlobClient(blobItem.Name);
                    var videoInfo = new VideoInfo
                    {
                        Name = blobItem.Name,
                        BlobUri = blobClient.Uri.ToString()
                    };
                    Videos.Add(videoInfo);
                }
            }
        }
    }
}
