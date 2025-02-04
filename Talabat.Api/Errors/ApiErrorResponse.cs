using Microsoft.AspNetCore.Http.HttpResults;

namespace Talabat.Api.Errors
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiErrorResponse(int statuscode,string? message=null)
        {
            this.StatusCode = statuscode;
            this.Message =message?? GetDefaultMessageForStatusCode(statuscode);
            
        }

        private string? GetDefaultMessageForStatusCode(int statuscode)
        {
            return statuscode switch

            {
                400 => "A bad request you have made",
                401 => "Authorized, you are not",
                404 =>"resources not found",
                500=>"there is server error",
                _=>null


            };

        }
    }
}
