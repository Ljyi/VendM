using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using VendM.Model.DataModelDto;
using VendM.Service;

namespace VendM.WebAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class UserController : ApiController
    {
        UserService userService = null;
        private UserController()
        {
            userService = new UserService();
        }
        // GET api/values
        public IEnumerable<string> Get(int id)
        {
            UserDto userDto = userService.Find(id);
            return new string[] { "value1", "value2" };
        }
        public async Task<ResultModel<UserDto>> GetUser(int id)
        {
            ResultModel<UserDto> resultModel = new ResultModel<UserDto>();
            try
            {
                UserDto userDto = userService.Find(id);
            }
            catch (Exception ex)
            {
                resultModel.message = ex.Message;
            }
            return resultModel;
        }
    }
}
