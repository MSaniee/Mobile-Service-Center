using ServiceCenter.WebFramework.API.Dtos.Files;
using System.IO;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace ServiceCenter.API.Areas.Common.Controllers.v1;

[Area("Common")]
[ApiVersion("1")]
[AllowAnonymous]
[ApiExplorerSettings(GroupName = "Common")]
public class UploadController : BaseController
{
    /// <summary>
    /// آپلود فایل
    /// </summary>
    /// <param name="dto"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost]
    [ActionName("")]
    public async Task<ApiResult<string>> Post([FromForm] FileDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var file = dto.File;
            var folderName = Path.Combine("wwwroot", "Images");
            var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (file.Length > 0)
            {
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var fullPath = Path.Combine(pathToSave, fileName);
                var dbPath = Path.Combine("Images", fileName);
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return dbPath;
            }
            else
            {
                return BadRequest();
            }
        }
        catch
        {
            return BadRequest();
        }
    }
}

