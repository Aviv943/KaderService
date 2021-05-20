using System.Collections.Generic;
using System.Threading.Tasks;
using KaderService.Services.Constants;
using KaderService.Services.Models;
using KaderService.Services.Models.AuthModels;
using Refit;

namespace KaderService.Contracts.Api
{
    public interface IKaderCategoriesApi
    {
        [Get("/api/categories")]
        Task<IEnumerable<Category>> GetCategories();

        [Post("/api/categories")]
        Task PostCategory(Category category);
    }
}