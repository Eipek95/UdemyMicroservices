using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class ResponseDto<T>
    {

        public T Data { get; private set; }//dışarıdan nesnler değiştirilemez.başarılı olursa çalışacak yer
        [JsonIgnore]//ignore alanlar yazılım içinde kullanılmayı sağlar.kullanıcıya hiçbir şekilde görünmez
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }


        //Static Factory Method=> Static methodlarla beraber geriye yeni bir nesne dönen methodlar
        public static ResponseDto<T> Success(T data, int statusCode)
        {
            return new ResponseDto<T> { StatusCode = statusCode, Data = data, IsSuccessful = true };
        }

        public static ResponseDto<T> Success(int statusCode)//update  ve delete için
        {
            return new ResponseDto<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static ResponseDto<T> Fail(List<string> errors, int statusCode)
        {
            return new ResponseDto<T> { Errors = errors, IsSuccessful = false, StatusCode = statusCode };
        }

        public static ResponseDto<T> Fail(string error, int statusCode)
        {
            return new ResponseDto<T>
            {
                Errors = new List<string>() { error },
                StatusCode = statusCode,
            };
        }
    }
}
