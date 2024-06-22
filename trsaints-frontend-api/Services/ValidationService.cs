using System.Globalization;
using trsaints_frontend_api.Services.Interfaces;

namespace trsaints_frontend_api.Services;

public class ValidationService: IValidationService
{
    public bool ValidateDateTime(string dateTime)
    {
        return DateTime.TryParseExact(dateTime, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
    }
}