using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace SitesChecker.Core
{
	public class AuthOptions
	{
		public const string Issuer = "IgorZudov"; 
		public const string Audience = "http://localhost:55248/";
		public const int Lifetime = 5;
		public static SymmetricSecurityKey GetSymmetricSecurityKey()
		{
			return new SymmetricSecurityKey(Encoding.ASCII.GetBytes("key_for_this_test"));
		}
	}
}