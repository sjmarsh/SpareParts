using Refit;
using SpareParts.Shared.Models;

namespace SpareParts.Client.Services
{
    public interface IPartService
    {
        [Get("/api/part")]
        Task<PartResponse> Get(int id);

        [Get("/api/part/index")]
        Task<PartListResponse> Index(bool isCurrentOnly, int skip = 0, int? take = null);

        [Post("/api/part")]
        Task<PartResponse> Post(Part part);

        [Put("/api/part")]
        Task<PartResponse> Put(Part part);

        [Delete("/api/part")]
        Task<PartResponse> Delete(int id);
    }
}
