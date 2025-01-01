using Business.Abstract;
using Business.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FileController:ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("upload")]
    public async Task<ActionResult<FileGetDto>> UploadItem([FromForm] FileCreateDto fileCreateDto, IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("Dosya boş olamaz.");

        try
        {
            using var stream = file.OpenReadStream();
            var result = await _fileService.PostItem(fileCreateDto, stream , file.FileName);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe yükleme sırasında bir hata oluştu");
        }
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<FileGetDto>> GetItem(int id)
    {
        try
        {
            var item = await _fileService.GetItem(id);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Öğe bulunamadı.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe bilgisi alınırken bir hata oluştu.");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FileGetDto>>> GetAllItems()
    {
        try
        {
            var items = await _fileService.GetList();
            return Ok(items);
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe listesi alınırken bir hata oluştu");
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<FileGetDto>> UpdateItem([FromBody] FileUpdateDto fileUpdateDto)
    {
        try
        {
            var updatedItem = await _fileService.PutItem(fileUpdateDto);
            return Ok(updatedItem);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Güncellenecek öğe bulunamadı.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe güncellenirken bir hata oluştu.");
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        try
        {
            var result = await _fileService.DeleteItem(id);
            if (result)
            {
                return NoContent();
            }
            else
            {
                return NotFound("Silinecek öğe bulunamadı.");
            }
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe silinirken bir hata oluştu.");
        }
    }

    [HttpGet("download/{id:int}")]
    public async Task<IActionResult> DownloadItem(int id)
    {
        try
        {
            var itemInfo = await _fileService.GetItem(id);
            if (itemInfo == null)
            {
                return NotFound("İndirilcek öğe bulunamadı.");
            }

            var stream = await _fileService.DownloadItem(id);

            return new FileStreamResult(stream, itemInfo.ContentType)
            {
               FileDownloadName = itemInfo.FileName,
               EnableRangeProcessing = true
            };
        }
        catch (KeyNotFoundException)
        {
            return NotFound("İndirilcek öğe bulunamadı.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe indirilirken bir hata oluştu.");
        }
    }

    [HttpGet("size/{id:int}")]
    public async Task<ActionResult<long>> GetItemSize(int id)
    {
        try
        {
            var size = await _fileService.GetItemSize(id);
            return Ok(size);

        }
        catch (KeyNotFoundException)
        {
            return NotFound("Öğe bulunamadı");
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe boyutu alınırken bir hata oluştu.");
        }
    }

    [HttpGet("contenttype/{id:int}")]
    public async Task<ActionResult<string>> GetItemContenType(int id)
    {
        try
        {
            var contentType = await _fileService.GetItemContentType(id);
            return Ok(contentType);
        }
        catch (KeyNotFoundException)
        {
            return NotFound("Öğe bulunamadı.");
        }
        catch (Exception)
        {
            return StatusCode(500, "Öğe içerik türü alınırken bir hata oluştu.");
        }
    }


}
