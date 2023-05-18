using CoreApiSamples.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoreApiSamples.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HangfiredemoController : ControllerBase
    {
        private readonly IHangfireService _hangfireService;

        public HangfiredemoController(IHangfireService hangfireService)
        {
            _hangfireService = hangfireService;
        }


        [HttpPost]
        public void CreateRecurringJob()
        {
            _hangfireService.CreateRecurringJob();
        }
    }
}
