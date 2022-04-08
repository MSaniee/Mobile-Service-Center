namespace ServiceCenter.Common.IdentityTools;

public static class IdentityExtensions
{
    public static string FindFirstValue(this ClaimsIdentity identity, string claimType)
    {
        return identity?.FindFirst(claimType)?.Value;
    }

    public static string FindFirstValue(this IIdentity identity, string claimType)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        return claimsIdentity?.FindFirstValue(claimType);
    }

    public static string GetUserId(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.NameIdentifier);
    }

    public static long GetCompanyId(this IIdentity identity)
    {
        return Convert.ToInt64(identity?.FindFirstValue("CompanyId"));
    }

    public static long? GetCompanyID(this IIdentity identity)
    {
        string companyId = identity?.FindFirstValue("CompanyId");
        return companyId is null ? null : Convert.ToInt64(companyId);
    }

    public static int GetPersonnelId(this IIdentity identity)
    {
        return Convert.ToInt32(identity?.FindFirstValue(ClaimTypes.NameIdentifier));
    }

    public static string GetCompanyType(this IIdentity identity)
    {
        return identity?.FindFirstValue("CompanyType");
    }

    public static long GetDeviceId(this IIdentity identity)
    {
        return Convert.ToInt64(identity?.FindFirstValue("DeviceId"));
    }

    public static Guid? GetGuidUserId(this IIdentity identity)
    {
        var stringUserId = GetUserId(identity);
        return stringUserId is null ? null : Guid.Parse(stringUserId);
    }

    public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
    {
        var userId = identity?.GetUserId();
        return userId.HasValue()
            ? (T)Convert.ChangeType(userId, typeof(T), CultureInfo.InvariantCulture)
            : default(T);
    }

    public static string GetUserName(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.Name);
    }

    public static string GetPhoneNumber(this IIdentity identity)
    {
        return identity?.FindFirstValue(ClaimTypes.MobilePhone);
    }

    public static List<string> GetRolesNames(this IIdentity identity)
    {
        var claimsIdentity = identity as ClaimsIdentity;
        var claims = claimsIdentity.Claims.Where(c => c.Type == ClaimTypes.Role).ToList();

        var result = new List<string>();

        foreach (var claim in claims) result.Add(claim.Value);

        return result;
    }
}
