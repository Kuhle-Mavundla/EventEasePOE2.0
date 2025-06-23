using Azure.Storage.Blobs;

namespace EventEasePOE2._0.Services
{
    // This class helps us save pictures to the cloud (Azure Blob Storage)
    public class BlobService
    {
        private readonly IConfiguration _configuration; // Holds settings like where to save the pictures

        // When we make a BlobService, we give it the settings it needs
        public BlobService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This function uploads a picture and gives back a link to it
        public async Task<string> UploadVenueImageAsync(IFormFile file)
        {
            // Get the connection info and container name from settings
            var connectionString = _configuration["AzureBlobStorage:ConnectionString"];
            var containerName = _configuration["AzureBlobStorage:ContainerName"];

            // Connect to the place in the cloud where we save pictures
            var containerClient = new BlobContainerClient(connectionString, containerName);

            // Make sure the container exists, if not, create it
            await containerClient.CreateIfNotExistsAsync();

            // Make a unique file name for the picture so it doesn't get mixed up
            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // Get a client to upload this specific picture
            var blobClient = containerClient.GetBlobClient(fileName);

            // Open the picture file and upload it to the cloud
            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, overwrite: true);
            }

            // Return the link where the picture is stored so we can use it later
            return blobClient.Uri.ToString();
        }
    }
}
