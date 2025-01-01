## ASP.NET Core Web API - MinIO Object Storage Integration

This project demonstrates how to integrate **MinIO**, an open-source, S3-compatible object storage server, with an **ASP.NET Core Web API**. It provides a robust backend solution for managing file storage operations such as uploading, downloading, listing, and deleting files in a MinIO bucket.

### Key Features
- **File Upload**: Upload files to a MinIO bucket with support for large files and multipart uploads.
- **File Download**: Download files from a MinIO bucket directly via the API.
- **File Management**: List, delete, and manage files stored in a MinIO bucket.
- **MinIO Integration**: Seamless integration with MinIO using the official **MinIO .NET SDK**.
- **API Documentation**: Interactive API documentation powered by **Swagger UI**.

### Technologies Used
- **ASP.NET Core 8** (Web API)
- **MinIO** (Object Storage)
- **MinIO .NET SDK** (For MinIO integration)
- **Swagger UI** (API Documentation)
- **Dependency Injection** (For clean and modular code)
- **Entity Framework Core** (Optional, for metadata storage)

### Use Cases
- **Cloud-Native Applications**: Use MinIO as a cost-effective, S3-compatible object storage solution.
- **File Management Systems**: Build a backend for file upload/download functionality.
- **Data Archiving**: Store and retrieve large files efficiently.

### How to Run the Project
1. **Clone the Repository**:
   ```bash
   git clone https://github.com/yunuspektass/ASP.NET-Core-Web-API-MinIO-Object-Storage-Integration.git
   ```
2. **Set Up MinIO**:
   - Install and run a MinIO server (local or remote).
   - Update the MinIO configuration in `appsettings.json`:
     ```json
     "MinIO": {
       "Endpoint": "localhost:9000",
       "AccessKey": "your-access-key",
       "SecretKey": "your-secret-key",
       "BucketName": "your-bucket-name"
     }
     ```
3. **Install Dependencies**:
   ```bash
   dotnet restore
   ```
4. **Run the Project**:
   ```bash
   dotnet run
   ```
5. **Access Swagger UI**:
   Open your browser and navigate to `http://localhost:5000/swagger` to explore the API endpoints.

### Configuration
- **MinIO Settings**: Update the `appsettings.json` file with your MinIO server details.
- **Bucket Creation**: Ensure the specified bucket exists in your MinIO server. If not, create it manually or add logic to create it on startup.

### Example API Endpoints
- **Upload File**: `POST /api/files/upload`
- **Download File**: `GET /api/files/download/{fileName}`
- **List Files**: `GET /api/files/list`
- **Delete File**: `DELETE /api/files/delete/{fileName}`

### Contributing
Contributions are welcome! If you'd like to contribute, please follow these steps:
1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Submit a pull request with a detailed description of your changes.

### License
This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.

---

### Why MinIO?
MinIO is a high-performance, S3-compatible object storage solution that is perfect for modern cloud-native applications. It is easy to set up, scalable, and ideal for storing unstructured data like images, videos, and documents.

### Why ASP.NET Core?
ASP.NET Core is a powerful framework for building high-performance, cross-platform web APIs. It provides built-in support for dependency injection, middleware, and modern development practices, making it an excellent choice for backend development.
