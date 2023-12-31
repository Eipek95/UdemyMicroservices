﻿using FreeCourse.Services.Catalog.Dtos;
using FreeCourse.Shared.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeCourse.Services.Catalog.Services.CategoryServices
{
    public interface ICategoryService
    {
        Task<Response<List<CategoryDto>>> GetAllAsync();
        Task<Response<CategoryDto>> CreateAsync(CategoryDto categoryDto);//dto yu yapmadigimiz icin category aldık dto yu da yapabiliriz
        Task<Response<CategoryDto>> GetByIdAsync(string id);
    }
}
