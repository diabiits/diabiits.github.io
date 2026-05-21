//using Diabits.Web.DTOs;
//using Diabits.Web.Infrastructure.Api;

//namespace Diabits.Web.Infrastructure.Services.HealthData;

//public class HealthDataService(ApiClient apiClient)
//{
//    public async Task<HealthDataResponse> GetHealthDataAsync(DateTime startDate, DateTime endDate)
//    {
//        var start = startDate.ToString("yyyy-MM-ddTHH:mm:ss");
//        var end = endDate.ToString("yyyy-MM-ddTHH:mm:ss");
//        var response = await apiClient.GetAsync<HealthDataResponse>($"HealthData?startDate={start}&endDate={end}");

//        if (response.Data == null ) //TODO Refactor
//        { 
//            throw new HttpRequestException(response.Error ?? "No data found"); 
//        }   

//        return response.Data;
//    }
//}
