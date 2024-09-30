using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static src.DTO.CouponDTO;

namespace src.Services
{
    public interface ICouponService
    {
        //create
        Task<CouponReadDto> CreatOneAsync(CouponCreateDto createDto);

        //get all
        Task<List<CouponReadDto>> GetAllAsync();

        //get by id
        Task<CouponReadDto> GetByIdAsync(Guid id);

        //delete
        Task<bool> DeleteOneAsync(Guid id);

        //update
        Task<bool> UpdateOneAsync(Guid id , CouponUpdateDto updateDto);
        
    }
}