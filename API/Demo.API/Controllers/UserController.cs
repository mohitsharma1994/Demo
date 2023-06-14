using AutoMapper;
using Demo.BLL.IService;
using Demo.Infrastructure.Entity;
using Demo.Infrastructure.RequestModel;
using Demo.Infrastructure.ResultModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Demo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(UserManager<User> userManager, IMapper mapper, IUserService userService)
        {
            _userManager = userManager;
            _mapper = mapper;
            _userService = userService;
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserModel model)
        {
            BaseResult? result;
            try
            {
                var user = _mapper.Map<User>(model);
                var createUser = await _userManager.CreateAsync(user, model.Password);

                if (!createUser.Succeeded)
                {
                    result = new BaseResult { Status = HttpStatusCode.BadRequest, Message = createUser.Errors.Select(x => x.Description).FirstOrDefault()?.ToString() };
                    return Ok(result);
                }

                result = new BaseResult { Status = HttpStatusCode.Created };
            }
            catch (Exception ex)
            {
                result = new BaseResult { Status = HttpStatusCode.InternalServerError, Message = ex.Message };
            }

            return Ok(result);
        }

        /// <summary>
        /// Get User List
        /// </summary>
        /// <param name="userParameters"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserList([FromQuery] GetUserListRequest userParameters)
        {
            try
            {
                var userListResult = await _userService.GetUserListAsync(userParameters);
                return Ok(userListResult);
            }
            catch (Exception ex)
            {
                var result = new BaseResult { Status = HttpStatusCode.InternalServerError, Message = ex.Message };
                return Ok(result);
            }
        }

        /// <summary>
        /// Get User By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);

                if (user == null)
                {
                    var baseResult = new BaseResult { Status = HttpStatusCode.NotFound, Message = "User does not found." };
                    return Ok(baseResult);
                }

                var result = _mapper.Map<UserResult>(user);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var baseResult = new BaseResult { Status = HttpStatusCode.InternalServerError, Message = ex.Message };
                return Ok(baseResult);
            }
        }

        /// <summary>
        /// Get User logs by user Id.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet("{id}/logs")]
        public async Task<IActionResult> GetUserLogs(string id, [FromQuery] UserLogRequest request)
        {
            try
            {
                var result = await _userService.GetUserLogListAsync(id, request);

                return Ok(result);
            }
            catch (Exception ex)
            {
                var baseResult = new BaseResult { Status = HttpStatusCode.InternalServerError, Message = ex.Message };
                return Ok(baseResult);
            }
        }
    }
}
