using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using ServiceCenter.Website.Interfaces;
using System.Net.Http.Headers;

namespace ServiceCenter.Website.Components;

public partial class ImageUpload
{
    [Parameter]
    public string ImgUrl { get; set; }
    [Parameter]
    public EventCallback<string> OnChange { get; set; }

    [Inject]
    public IReceiptService ReceiptService { get; set; }

    private async Task HandleSelected(InputFileChangeEventArgs e)
    {
        var imageFiles = e.GetMultipleFiles();
        foreach (var imageFile in imageFiles)
        {
            if (imageFile != null)
            {
                var resizedFile = await imageFile.RequestImageFileAsync("image/png", 300, 500);

                using (var ms = resizedFile.OpenReadStream(resizedFile.Size))
                {
                    var content = new MultipartFormDataContent();
                    content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data");
                    content.Add(new StreamContent(ms, Convert.ToInt32(resizedFile.Size)), "File", imageFile.Name);
                    ImgUrl = await ReceiptService.UploadImage(content);
                    await OnChange.InvokeAsync(ImgUrl);
                }
            }
        }
    }
}
