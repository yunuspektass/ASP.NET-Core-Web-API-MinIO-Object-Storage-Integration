using Minio;
using Microsoft.Extensions.Configuration;

namespace DataAccess;

public class FileManagementContext
{
    public IMinioClient MinioClient { get;  }
    public string BucketName { get; }

    public FileManagementContext(IConfiguration configuration)
    {
        var minioConfig = configuration.GetSection("Minio");

        var endpoint = minioConfig["Endpoint"];
        var accessKey = minioConfig["AccessKey"];
        var secretKey = minioConfig["SecretKey"];
        BucketName = minioConfig["BucketName"];
        var useSSL = bool.Parse(minioConfig["UseSSL"]);

        try
        {
            MinioClient = new MinioClient()
                .WithEndpoint(endpoint)
                .WithCredentials(accessKey, secretKey)
                .WithSSL(useSSL)
                .Build();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
