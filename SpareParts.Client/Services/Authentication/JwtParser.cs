using System.Security.Claims;
using System.Text.Json;

namespace SpareParts.Client.Services.Authentication
{
    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];

            var jsonBytes = ParseBase64WithoutPadding(payload);

            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            if(keyValuePairs != null)
            {
                claims.AddRange(keyValuePairs.Select(kvp => new Claim(ConvertKeyToClaimType(kvp.Key), GetValueString(kvp.Value))));
            }
            return claims;
        }

        private static string ConvertKeyToClaimType(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {
                var fields = typeof(ClaimTypes).GetFields();
                var field = fields.FirstOrDefault(f => f.Name.ToLower() == key.ToLower());
                if (field != null && field.GetValue(null) is string strValue)
                {
                    return strValue;
                }
            }

            return key;
        }

        private static string GetValueString(object value)
        {
            var strVal = value.ToString();
            if(strVal != null)
            {
                return strVal.ToString();
            }
            return "";
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
