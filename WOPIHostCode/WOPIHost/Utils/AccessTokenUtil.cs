using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Xml;
using System.Web;
namespace WOPIHost.Utils
{
    /// <summary>
    /// A class to generate and validate access token
    /// </summary>
    public class AccessTokenUtil
    {
        private const string sec = "ProEMLh5e_qnzdNUQrqdHPgp";
        private const string sec1 = "ProEMLh5e_qnzdNU";
        
        /// <summary>
        /// Generate the token
        /// </summary>
        /// <param name="user">User name</param>
        /// <param name="docId">File name</param>
        /// <returns>The token</returns>
        public static JwtSecurityToken GenerateToken(string user, string docId)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SymmetricSecurityKey securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            SigningCredentials signingCredentials = new SigningCredentials(
                securityKey,
                SecurityAlgorithms.HmacSha512);

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user),
                new Claim("docId", docId),
            };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.CreateJwtSecurityToken("issuer", "Audience", claimsIdentity, DateTime.Now, DateTime.Now.AddHours(1), DateTime.Now, signingCredentials);

            return jwtSecurityToken;
        }

        /// <summary>
        /// Validate the token
        /// </summary>
        /// <param name="tokenString">The token string</param>
        /// <param name="docId">File name</param>
        /// <returns>Return true if success; Otherwise, return false</returns>
        public static bool ValidateToken(string tokenString, string docId)
        {
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SymmetricSecurityKey securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));

            JwtSecurityToken jwt = new JwtSecurityToken(tokenString);

            // Verification
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    "Audience"
                },
                ValidIssuers = new string[]
                {
                    "issuer"
                },
                IssuerSigningKey = securityKey,
                // This is the decryption key
                TokenDecryptionKey = securityKey1,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            try
            {
                SecurityToken validatedToken = null;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = handler.ValidateToken(tokenString, tokenValidationParameters, out validatedToken);
                bool result = (principal.HasClaim("docId", docId));
                return result;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// Get user name from token string
        /// </summary>
        /// <param name="tokenString">The token string</param>
        /// <returns>The user name</returns>
        public static string GetUserFromToken(string tokenString)
        {
            // Initialize the token handler and validation parameters
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec));
            SymmetricSecurityKey securityKey1 = new SymmetricSecurityKey(Encoding.Default.GetBytes(sec1));
            JwtSecurityToken jwt = new JwtSecurityToken(tokenString);

            // Verification
            TokenValidationParameters tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    "Audience"
                },
                ValidIssuers = new string[]
                {
                    "issuer"
                },
                IssuerSigningKey = securityKey,
                TokenDecryptionKey = securityKey1,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true
            };

            try
            {
                // Try to extract the user principal from the token
                SecurityToken validatedToken = null;
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                ClaimsPrincipal principal = handler.ValidateToken(tokenString, tokenValidationParameters, out validatedToken);
                return principal.Identity.Name;
            }
            catch (Exception)
            {
                return String.Empty;
            }
        }

        /// <summary>
        /// Get token string
        /// </summary>
        /// <param name="token">The token</param>
        /// <returns>The token string</returns>
        public static string WriteToken(JwtSecurityToken token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }

        /// <summary>
        /// Get user permission
        /// </summary>
        /// <param name="username">User name</param>
        /// <param name="docid">File name</param>
        /// <returns>The user permission</returns>
        public static string readUserXml(string username, string docid)
        {
            string permission = "write";
            string xmlFilePath = HttpContext.Current.Server.MapPath(@"\App_Data\UserPermissions.xml");

            XmlDocument doc = new XmlDocument();
            doc.Load(xmlFilePath);
            XmlNodeList userpermission = doc.SelectNodes("/userpermission");
            if (userpermission != null)
            {
                foreach (XmlNode usernode in userpermission)
                {
                    XmlNodeList user = usernode.SelectNodes("user");
                    foreach (XmlNode userinfo in user)
                    {
                        if ((userinfo.Attributes["name"].Value).Equals(username, StringComparison.InvariantCultureIgnoreCase)
                            || (userinfo.Attributes["name"].Value).Equals("AllUsers", StringComparison.InvariantCultureIgnoreCase))
                        {
                            XmlNodeList fileList = userinfo.SelectNodes("file");
                            foreach (XmlNode fileNode in fileList)
                            {
                                if ((fileNode.Attributes["name"].Value).Equals(docid, StringComparison.InvariantCultureIgnoreCase))
                                {
                                    permission = fileNode.InnerText.Trim();
                                    return permission;
                                }
                            }
                        }
                    }
                }
            }

            return permission;
        }
    }
}