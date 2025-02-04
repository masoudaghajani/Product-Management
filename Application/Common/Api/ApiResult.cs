using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Api
{
    public class Result<T>
    {
        public bool IsSuccess { get; }
        public T? Data { get; }
        public string? ErrorMessage { get; }

        private Result(T data)
        {
            IsSuccess = true;
            Data = data;
            ErrorMessage = null;
        }

        private Result(string errorMessage)
        {
            IsSuccess = false;
            Data = default;
            ErrorMessage = errorMessage;
        }

        public static Result<T> Success(T data) => new Result<T>(data);
        public static Result<T> Failure(string errorMessage) => new Result<T>(errorMessage);
    }

}
