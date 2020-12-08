using AutoMapper;
using HomePJ.API.Models;
using HomePJ.API.Models.DTOs;
using HomePJ.API.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomePJ.API.Controllers
{
    /// <summary>
    /// National Park Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParkController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly INationalParkRepository _nationalParkRepo;

        /// <summary>
        ///Contructor
        /// </summary>
        /// <param name="mapper">maping data entities vs dtos</param>
        /// <param name="nationalParkRepo">national park repository</param>
        public NationalParkController(IMapper mapper, INationalParkRepository nationalParkRepo)
        {
            _nationalParkRepo = nationalParkRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list national park
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetNationalParks()
        {
            var listNationalPark = _nationalParkRepo.GetNationalParks();

            var objDto = new List<NationalParkDTO>();
            foreach (var obj in listNationalPark)
            {
                objDto.Add(_mapper.Map<NationalParkDTO>(obj));
            }

            return Ok(objDto);
        }


        /// <summary>
        /// Get national park
        /// </summary>
        /// <param name="nationalParkId">national park id</param>
        /// <returns></returns>
        [HttpGet("{nationalParkId:int}", Name = "GetNationalPark")]
        public IActionResult GetNationalPark(int nationalParkId)
        {
            var obj = _nationalParkRepo.GetNationalPark(nationalParkId);
            if (obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<NationalParkDTO>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Create national park
        /// </summary>
        /// <param name="nationalParkDto">national park info</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CreateNationalPark([FromBody] NationalParkDTO nationalParkDto)
        {
            if (nationalParkDto == null)
            {
                return BadRequest(ModelState);
            }

            bool isExists = _nationalParkRepo.NationalParkExists(nationalParkDto.Name);
            if (isExists)
            {
                ModelState.AddModelError(CommonText.MSG_RESPONSE, "NationalPark Exists");
                return StatusCode(StatusCodes.Status404NotFound, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDto);


            if (!_nationalParkRepo.CreateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError(CommonText.MSG_RESPONSE, $"Sảy ra lỗi khi thêm mới {nationalParkObj.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return CreatedAtRoute("GetNationalPark", new { nationalParkId = nationalParkObj.Id }, nationalParkObj);
        }

        /// <summary>
        /// Update national park
        /// </summary>
        /// <param name="nationalParkId">national park id</param>
        /// <param name="nationalParkDTO">national park updated</param>
        /// <returns></returns>
        [HttpPatch("{nationalParkId:int}", Name = "UpdateNationalPark")]
        public IActionResult UpdateNationalPark(int nationalParkId, [FromBody] NationalParkDTO nationalParkDTO)
        {
            if (nationalParkDTO == null || nationalParkId != nationalParkDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var nationalParkObj = _mapper.Map<NationalPark>(nationalParkDTO);

            if (!_nationalParkRepo.UpdateNationalPark(nationalParkObj))
            {
                ModelState.AddModelError(CommonText.MSG_RESPONSE, $"Sảy ra lỗi khi update {nationalParkObj.Name}");
                return StatusCode(CommonText.SERVER_ERRORS, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Delete national park
        /// </summary>
        /// <param name="nationalParkId">national park id</param>
        /// <returns></returns>
        [HttpDelete("{nationalParkId:int}", Name = "DeleteNationalPark")]
        public IActionResult DeleteNationalPark(int nationalParkId)
        {
            bool isExists = _nationalParkRepo.NationalParkExists(nationalParkId);
            if (!isExists)
            {
                return NotFound();
            }
            var objDto = _nationalParkRepo.GetNationalPark(nationalParkId);

            if (!_nationalParkRepo.DeleteNationalPark(objDto))
            {
                ModelState.AddModelError(CommonText.MSG_RESPONSE, $"Có lỗi sảy ra khi xoá bản ghi {objDto.Name}");
                return StatusCode(StatusCodes.Status500InternalServerError, ModelState);
            }
            return NoContent();
        }
    }
}
