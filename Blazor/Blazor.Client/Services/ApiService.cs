using Shared.Dtos;
using Shared.Results;
using System.Net.Http.Json;

namespace Blazor.Client.Services;

public class ApiService
{
    private readonly HttpClient _httpClient;

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Authors
    public async Task<IEnumerable<GetDepartmentDto>> GetAllDepartmentsAsync()
    {
        var result = await _httpClient.GetFromJsonAsync<IEnumerable<GetDepartmentDto>>("api/departments");
        return result ?? [];
    }

    internal async Task<Result> DeleteCompanyAsync(int id)
    {
        var response = await _httpClient.DeleteAsync($"api/companies/{id}");
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<Result>();
            return result ?? Result.Error("Fehler beim Deserializieren der Antwort");
        }
        return Result.Error($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
    }

    internal async Task<PagedResult<GetCompanyDto>?> GetCompanyByDepartmentId(int value, int page, int pageSize)
    {
        return await _httpClient.GetFromJsonAsync<PagedResult<GetCompanyDto>>(
            $"api/companies/by-department/{value}?page={page}&pageSize={pageSize}");
    }

    //// Books
    //public async Task<PagedResult<GetBookDto>?> GetBooksByAuthorAsync(int authorId, int page = 0, int pageSize = 3)
    //{
    //    return await _httpClient.GetFromJsonAsync<PagedResult<GetBookDto>>(
    //        $"api/books/by-author/{authorId}?page={page}&pageSize={pageSize}");
    //}
    //public async Task<Result> DeleteBooksAsync(int bookId)
    //{
    //    var response = await _httpClient.DeleteAsync($"api/books/{bookId}");

    //    if (response.IsSuccessStatusCode)
    //    {
    //        var result = await response.Content.ReadFromJsonAsync<Result>();
    //        return result ?? Result.Error("Fehler beim Deserialisieren der Antwort");
    //    }

    //    return Result.Error($"HTTP {(int)response.StatusCode}: {response.ReasonPhrase}");
    //}

}
