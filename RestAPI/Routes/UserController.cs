using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using GreenHand.Server.Remote.Common.UserApi;

namespace RestAPI.Routes
{
    [RoutePrefix("user")]
    public class UserController : ApiController
    {
        private readonly UserApi api = new UserApi();

        [Route("register/{email}/{password}"), HttpPost]
        public async Task<IHttpActionResult> PostUser(string email, string password)
        {
            try
            {
                var result = await api.CreateUser(email, password);

                Console.WriteLine("User created successfully");
                return Created("register", result);
            }

            catch (ArgumentException ex)
            {
                Console.WriteLine("Create user failed");
                Console.WriteLine(ex.Message);
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                return Content(HttpStatusCode.BadRequest, new HttpError(ex.Message));
            }

            catch (Exception ex)
            {
                Console.WriteLine("Create user failed");
                Console.WriteLine(ex.Message);
                File.AppendAllText(Path.Combine(Environment.CurrentDirectory, "log.txt"), ex.ToString());
                return Content(HttpStatusCode.InternalServerError, new HttpError(ex.Message));
            }
        }
    }
}