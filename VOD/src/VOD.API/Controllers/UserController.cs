using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace VOD.API.Controllers
{
    [Route("api/v1/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        /*private List<UserDetailsDto> userDtos = new List<UserDetailsDto>()
            {
                new UserDetailsDto { Id = new Guid("00000000-0000-0000-0000-000000000000"), 
                    UserName = "User0", Email = "user0@test.com", CreationDate = DateTime.Now },
                new UserDetailsDto { Id = new Guid("11111111-1111-1111-1111-111111111111"),
                    UserName = "User1", Email = "user1@test.com", CreationDate = DateTime.Now },
                new UserDetailsDto { Id = new Guid("22222222-2222-2222-2222-222222222222"),
                    UserName = "User2", Email = "user2@test.com", CreationDate = DateTime.Now },
                new UserDetailsDto { Id = new Guid("33333333-3333-3333-3333-333333333333"),
                    UserName = "User3", Email = "user3@test.com", CreationDate = DateTime.Now }
            };

        [HttpGet]
        public ActionResult<List<UserDetailsDto>> GetAll()
        {
            return Ok(userDtos);
        }

        [HttpGet("byId/{id}")]
        public ActionResult<UserDetailsDto> GetById(string id)
        {
            UserDetailsDto s = userDtos.FirstOrDefault(s => s.Id.ToString() == id);

            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpGet("{userName}")]
        public ActionResult<UserDetailsDto> GetByUserName(string userName)
        {
            UserDetailsDto s = userDtos.FirstOrDefault(s => s.UserName == userName);

            if (s == null)
            {
                return NotFound();
            }

            return Ok(s);
        }

        [HttpPost]
        public ActionResult Post([FromBody]UserDetailsDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userDtos.Add(model);

            string key = model.UserName;
            return Created($"api/user/" + key, null);
        }

        [HttpPut("{userName}")]
        public ActionResult Put(string userName, [FromBody]UserDetailsDto model)
        {
            UserDetailsDto s = userDtos.FirstOrDefault(s => s.UserName == userName);

            if (s == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            userDtos[userDtos.IndexOf(s)] = model;

            return NoContent();
        }

        [HttpDelete("{userName}")]
        public ActionResult Delete(string userName)
        {
            UserDetailsDto s = userDtos.FirstOrDefault(s => s.UserName == userName);

            if (s == null)
            {
                return NotFound();
            }

            userDtos.Remove(s);

            return NoContent();
        }*/
    }
}