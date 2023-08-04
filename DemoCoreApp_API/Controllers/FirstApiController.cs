using DemoCoreApp_API.DemoData;
using DemoCoreApp_API.Models;
using DemoCoreApp_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace DemoCoreApp_API.Controllers
{
    //[controller] returns the name of the controller
    //[Route("api/[controller]")]
    [Route("api/FirstApi")]
    [ApiController]
    public class FirstApiController : ControllerBase
    {
        private readonly ILogger<FirstApiController> _logger;

        //Logger
        public FirstApiController(ILogger<FirstApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<DemoFDTO>> GetDemoFDTOs() 
        {
            _logger.LogInformation("Getting All Demo Datas");
            return Ok(DemoData.DemoData.DemoDataList);
        }

        [HttpGet("{id:int}", Name = "GetDemoFDTO")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<DemoFDTO> GetDemoFDTO(int Id)
        {
            var GetValue = DemoData.DemoData.DemoDataList.FirstOrDefault(u => u.Id == Id);

            if (Id == 0) 
            {
                return BadRequest();
            }

            if (GetValue == null) 
            {
                return NotFound();
            }

            return Ok(GetValue);
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<DemoFDTO> CreateDemoData ([FromBody]DemoFDTO dto) 
        {
            //WE CAN EXPLICITELY USE THIS TO CHECK THE STATE OF THE MODEL --- BY USING [ApiController] THIS CAN BE DONE AUTOMATICALLY
            //BUT DATA ANNOTAIONS IN MODEL IS REQUIRED IN BOTH CASES
            //if(!ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            var Debug = DemoData.DemoData.DemoDataList.FirstOrDefault(U => U.Name.ToLower() == dto.Name.ToLower());

            if (DemoData.DemoData.DemoDataList.FirstOrDefault(U=>U.Name.ToLower() == dto.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Name Already Exist");
                return BadRequest(ModelState);
            }

            if (dto == null)
            {
                return BadRequest(dto);
            }
            if (dto.Id > 0) 
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            dto.Id = DemoData.DemoData.DemoDataList.OrderByDescending(u => u.Id).FirstOrDefault().Id + 1;
            DemoData.DemoData.DemoDataList.Add(dto);

            return CreatedAtRoute("GetDemoFDTO", new { id = dto.Id} , dto);
        }

        [HttpDelete("{id:int}", Name = "DeleteData")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteDemoData(int id) 
        {
            if (id == 0) 
            {
                return BadRequest();
            }
            var GetValue = DemoData.DemoData.DemoDataList.FirstOrDefault(u => u.Id == id);

            if (GetValue == null)
            {
                return NotFound();
            }
            DemoData.DemoData.DemoDataList.Remove(GetValue);
            return NoContent();
        }

        //Use To Update Full Record
        [HttpPut("{id:int}", Name = "UpdateDemoData")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateDemoData(int Id , [FromBody]DemoFDTO demoFDTO)
        {
            if (demoFDTO == null || Id != demoFDTO.Id)
            {
                return BadRequest();
            }
            var GetValue = DemoData.DemoData.DemoDataList.FirstOrDefault(u => u.Id == Id);
            GetValue.Name = demoFDTO.Name;
            GetValue.sallary = demoFDTO.sallary;
            GetValue.EmployeeNumber = demoFDTO.EmployeeNumber;

            return NoContent();    
        }

        //Use To Update Only One Record
        [HttpPatch("{id:int}",Name = "UpdatePartialDemoData")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdatePartialDemoData (int id, JsonPatchDocument<DemoFDTO> patchdto)
        {
            if (patchdto == null || id == 0)
            {
                return BadRequest();
            }
            var GetValue = DemoData.DemoData.DemoDataList.FirstOrDefault(u => u.Id == id);
            if (GetValue == null)
            {
                return BadRequest();
            }
            patchdto.ApplyTo(GetValue, ModelState);
            //if (ModelState.IsValid) 
            //{
            //    return BadRequest(ModelState);
            //}
            return NoContent();
        }
    }
}
