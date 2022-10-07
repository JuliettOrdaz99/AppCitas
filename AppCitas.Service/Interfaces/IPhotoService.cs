using CloudinaryDotNet.Actions;

namespace AppCitas.Service.Interfaces;

public interface IPhotoService
{
    Task<ImageUploadResult> AddPhotoAsync(IFormFile photoFile);
    Task<DeletionResult> DeletePhotoAsync(string publicId);
}
