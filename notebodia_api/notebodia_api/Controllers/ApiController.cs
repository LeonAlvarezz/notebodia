using Microsoft.AspNetCore.Mvc;
using notebodia_api.Response;

namespace notebodia_api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected ActionResult<ApiResponse<T>> ApiOk<T>(T data, string message = "Success")
        {
            return Ok(ApiResponse<T>.Create(data, message));
        }

        protected ActionResult<ApiResponse<object>> ApiSimpleSuccess(string message = "Success")
        {
            return Ok(ApiResponse<object>.Create(message));
        }
    }
}