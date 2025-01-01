using Business.DTOs;

namespace Business.Abstract;

public interface IFileService
{
    Task<FileGetDto> PostItem(FileCreateDto fileCreateDto , Stream fileStream, string orginalFileName);
    Task<FileGetDto> GetItem(int id);
    Task<IEnumerable<FileGetDto>> GetList();
    Task<FileGetDto> PutItem(FileUpdateDto fileUpdateDto);
    Task<bool> DeleteItem(int id);
    Task<Stream> DownloadItem(int id);

    Task<long> GetItemSize(int id);
    Task<string> GetItemContentType(int id);
}
