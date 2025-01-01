using AutoMapper;
using Business.Abstract;
using Business.DTOs;
using DataAccess;
using DataAccess.Abstract;
using DataAccess.Concrete;
using Minio.DataModel.Args;
using FileInfo = Domain.Entities.FileInfo;

namespace Business.Concrete;

public class FileManager : IFileService
{
    private readonly FileManagementContext _context;
    private readonly IMapper _mapper;
    private readonly FileRepository _fileRepository;

    public FileManager(FileManagementContext context, IMapper mapper , FileRepository fileRepository)
    {
        _context = context;
        _mapper = mapper;
        _fileRepository = fileRepository;
    }

    public async Task<FileGetDto> PostItem(FileCreateDto fileCreateDto, Stream fileStream, string originalFileName)
    {
        if (fileStream == null || fileStream.Length == 0)
            throw new ArgumentException("Dosya boş olamaz.");

        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        string fileName = Path.GetFileNameWithoutExtension(originalFileName);
        string extension = Path.GetExtension(originalFileName);
        string objectName = $"{fileName}_{timestamp}{extension}";

        string contentType = DetermineContentType(originalFileName);

        var putObjectArgs = new PutObjectArgs()
            .WithBucket(_context.BucketName)
            .WithObject(objectName)
            .WithStreamData(fileStream)
            .WithObjectSize(fileStream.Length)
            .WithContentType(contentType);

        await _context.MinioClient.PutObjectAsync(putObjectArgs);

        var fileInfo = new FileInfo
        {
            FileName = fileCreateDto.FileName,
            Size = fileStream.Length,
            ContentType = contentType,
            BucketName = _context.BucketName,
            ObjectName = objectName
        };

        await _fileRepository.Add(fileInfo);

        return _mapper.Map<FileGetDto>(fileInfo);
    }

    public async Task<FileGetDto> GetItem(int id)
    {
        var fileInfo = await _fileRepository.Find(id);

        if (fileInfo == null)
            throw new KeyNotFoundException("Dosya bulunamadı.");

        var minioSize = await GetItemSize(id);
        var minioContentType = await GetItemContentType(id);

        if (fileInfo.Size != minioSize || fileInfo.ContentType != minioContentType)
        {
            fileInfo.Size = minioSize;
            fileInfo.ContentType = minioContentType;
            await _fileRepository.Update(fileInfo);
        }

        return _mapper.Map<FileGetDto>(fileInfo);
    }

    public async Task<IEnumerable<FileGetDto>> GetList()
    {
        var files = await _fileRepository.GetAll();
        var result = new List<FileGetDto>();

        foreach (var file in files)
        {
            try
            {
                file.Size = await GetItemSize(file.Id);
                file.ContentType = await GetItemContentType(file.Id);
                await _fileRepository.Update(file);
                result.Add(_mapper.Map<FileGetDto>(file));
            }
            catch (Exception)
            {
                continue;
            }
        }

        return result;
    }

    public async Task<FileGetDto> PutItem( FileUpdateDto fileUpdateDto)
    {
        var existingFile = await _fileRepository.Find(fileUpdateDto.Id);

        if (existingFile == null)
            throw new KeyNotFoundException("Dosya bulunamadı.");

        _mapper.Map(fileUpdateDto, existingFile);
        await _fileRepository.Update(existingFile);

        return _mapper.Map<FileGetDto>(existingFile);

    }

    public async Task<bool> DeleteItem(int id)
    {
        var fileInfo = await _fileRepository.Find(id);
        if (fileInfo == null)
            return false;

        var removeObjectArgs = new RemoveObjectArgs()
            .WithBucket(_context.BucketName)
            .WithObject(fileInfo.ObjectName);

        await _context.MinioClient.RemoveObjectAsync(removeObjectArgs);
        await _fileRepository.Delete(fileInfo);

        return true;
    }

    public async Task<Stream> DownloadItem(int id)
    {
        var fileInfo = await _fileRepository.Find(id);
        if (fileInfo == null)
            throw new KeyNotFoundException("Dosya bulunamadı.");

        var tempFilePath = Path.GetTempFileName();
        var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 4096, FileOptions.DeleteOnClose);

        var getObjectArgs = new GetObjectArgs()
            .WithBucket(_context.BucketName)
            .WithObject(fileInfo.ObjectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(fileStream);
                fileStream.Position = 0;
            });

        await _context.MinioClient.GetObjectAsync(getObjectArgs);

        return fileStream;
    }

    public async Task<long> GetItemSize(int id)
    {
        var fileInfo = await _fileRepository.Find(id);
        if (fileInfo == null)
            throw new KeyNotFoundException("Dosya bulunamadı.");

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(_context.BucketName)
            .WithObject(fileInfo.ObjectName);

        var objectStat = await _context.MinioClient.StatObjectAsync(statObjectArgs);
        return objectStat.Size;
    }

    public async Task<string> GetItemContentType(int id)
    {
        var fileInfo = await _fileRepository.Find(id);
        if (fileInfo == null)
            throw new KeyNotFoundException("Dosya bulunamadı.");

        var statObjectArgs = new StatObjectArgs()
            .WithBucket(_context.BucketName)
            .WithObject(fileInfo.ObjectName);

        var objectStat = await _context.MinioClient.StatObjectAsync(statObjectArgs);
        return objectStat.ContentType;
    }

    private string DetermineContentType(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        switch (extension)
        {
            case ".txt":
                return "text/plain";
            case ".pdf":
                return "application/pdf";
            case ".jpg":
            case ".jpeg":
                return "image/jpeg";
            case ".png":
                return "image/png";
            case ".gif":
                return "image/gif";
            case ".doc":
                return "application/msword";
            case ".docx":
                return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
            case ".xls":
                return "application/vnd.ms-excel";
            case ".xlsx":
                return "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            case ".ppt":
                return "application/vnd.ms-powerpoint";
            case ".pptx":
                return "application/vnd.openxmlformats-officedocument.presentationml.presentation";
            case ".zip":
                return "application/zip";
            case ".rar":
                return "application/x-rar-compressed";
            case ".mp3":
                return "audio/mpeg";
            case ".mp4":
                return "video/mp4";
            default:
                return "application/octet-stream";
        }
    }
}
