using OnlineMarket.Service.DTOs.Carts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarket.Service.Interfaces;

public interface ICartService
{
    Task<CartResultDto> GetByUserId(long userId);
}
