using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FreeCourse.Shared.Dtos
{
    public class Response<T>
    {

        public T Data { get; private set; }//dışarıdan nesnler değiştirilemez.başarılı olursa çalışacak yer
        [JsonIgnore]//ignore alanlar yazılım içinde kullanılmayı sağlar.kullanıcıya hiçbir şekilde görünmez
        public int StatusCode { get; private set; }
        [JsonIgnore]
        public bool IsSuccessful { get; private set; }

        public List<string> Errors { get; set; }


        //Static Factory Method=> Static methodlarla beraber geriye yeni bir nesne dönen methodlar
        public static Response<T> Success(T data, int statusCode)
        {
            return new Response<T> { StatusCode = statusCode, Data = data, IsSuccessful = true };
        }

        public static Response<T> Success(int statusCode)//update  ve delete için
        {
            return new Response<T> { Data = default(T), StatusCode = statusCode, IsSuccessful = true };
        }

        public static Response<T> Fail(List<string> errors, int statusCode)
        {
            return new Response<T> { Errors = errors, IsSuccessful = false, StatusCode = statusCode };
        }

        public static Response<T> Fail(string error, int statusCode)
        {
            return new Response<T>
            {
                Errors = new List<string>() { error },
                StatusCode = statusCode,
            };
        }
    }
}
