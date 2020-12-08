using AutoMapper;
using HomePJ.API.Models;
using HomePJ.API.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePJ.API.ParkyMapper
{
    public class ParkyMapings:Profile
    {
        public ParkyMapings()
        {
            CreateMap<NationalPark, NationalParkDTO>().ReverseMap();
        }
    }
}
