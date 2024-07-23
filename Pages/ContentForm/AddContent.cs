using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Modisette.Models;

namespace Modisette.Pages;

public class AddContentModel : PageModel
{
    private readonly Modisette.Data.SiteContext _context;
    private readonly string _targetFilePath;

    public AddContentModel (Modisette.Data.SiteContext context, IConfiguration config)
    {
        _context = context;
        _targetFilePath = config.GetValue<string>("StoredFilesPath");
    }


    public IActionResult OnGet()
    {
        return Page();
    }

    [BindProperty]
    public Course Course { get; set; } = default!;
    [BindProperty]
    public BufferedFiles BufferedFiles { get; set; }

    // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        _context.Courses.Add(Course);
        await _context.SaveChangesAsync();
        await OnPostUploadAsync();

        return RedirectToPage("./Index");
    }

    public async Task OnPostUploadAsync()
    {
        foreach (var formFile in BufferedFiles.FormFiles)
        {
            var formFileContent = 
                await FileHelpers.ProcessFormFile<BufferedFiles>(formFile, ModelState); 

            var trustedFileNameForFileStorage = Path.GetRandomFileName();
            var filePath = Path.Combine(
                _targetFilePath, trustedFileNameForFileStorage);

            using (var fileStream = System.IO.File.Create(filePath))
            {
                    await fileStream.WriteAsync(formFileContent);
            }
        }
    }   
}

public class BufferedFiles
{
    [Required]
    [Display(Name = "File")]
    public List<IFormFile> FormFiles { get; set; }
}

public static class FileHelpers
{
    public static async Task<byte[]> ProcessFormFile<T>(IFormFile formFile, ModelStateDictionary modelState)
    {
        var fieldDisplayName = string.Empty;

        MemberInfo property = 
            typeof(T).GetProperty(formFile.Name.Substring(
                formFile.Name.IndexOf(".", StringComparison.Ordinal) + 1));

        if (property != null)
        {
            if (property.GetCustomAttribute(typeof(DisplayAttribute)) is
                    DisplayAttribute displayAttribute)
            {
                fieldDisplayName = $"{displayAttribute.Name} ";
            }
        }

        var trustedFileNameForDisplay = WebUtility.HtmlEncode(
                formFile.FileName);

        if (formFile.Length == 0)
        {
            modelState.AddModelError(formFile.Name, 
                $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");

            return Array.Empty<byte>();
        }
        try
        {
            using (var memoryStream = new MemoryStream())
            {
                await formFile.CopyToAsync(memoryStream);

                // Check the content length in case the file's only
                // content was a BOM and the content is actually
                // empty after removing the BOM.
                if (memoryStream.Length == 0)
                {
                    modelState.AddModelError(formFile.Name,
                        $"{fieldDisplayName}({trustedFileNameForDisplay}) is empty.");
                }
                else
                {
                    return memoryStream.ToArray();
                }
            }
        }
        catch (Exception ex)
        {
            modelState.AddModelError(formFile.Name,
                $"{fieldDisplayName}({trustedFileNameForDisplay}) upload failed. " +
                $"Please contact the Help Desk for support. Error: {ex.HResult}");
            // Log the exception
        }

        return Array.Empty<byte>();
    }
}
