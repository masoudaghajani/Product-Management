using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Api
{
    public class ApiResult<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }

        private ApiResult(T data)
        {
            IsSuccess = true;
            Data = data;
            ErrorMessage = null;
        }

        private ApiResult(string errorMessage)
        {
            IsSuccess = false;
            Data = default;
            ErrorMessage = errorMessage;
        }

        public static ApiResult<T> Success(T data) => new ApiResult<T>(data);
        public static ApiResult<T> Failure(string errorMessage) => new ApiResult<T>(errorMessage);
    }

}
