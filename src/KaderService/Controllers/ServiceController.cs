using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Contracts.Responses;

namespace KaderService.Controllers
{
    [Route("api/service")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [HttpGet("uptime")]
        public async Task<IActionResult> GetUptimeAsync()
        {
            DateTime startTime = Process.GetCurrentProcess().StartTime;
            TimeSpan delta = DateTime.Now - startTime;

            return Ok(new GetUptimeResponse
            {
                Formatted = $"{delta.Days:d2}:{delta.Hours:d2}:{delta.Minutes:d2}:{delta.Seconds:d2}",
                TotalSeconds = delta.TotalSeconds.ToString("n0"),
                TotalMinutes = Math.Floor(delta.TotalMinutes).ToString("n0"),
                TotalHours = Math.Floor(delta.TotalHours).ToString("n0"),
                TotalDays = Math.Floor(delta.TotalDays).ToString("n0")
            });
        }

        /// <summary>
        /// git pull > build solution > start service
        /// </summary>
        // POST api/<ServiceController>
        [HttpPost("upgrade")]
        public async Task<IActionResult> UpgradeVersionAsync()
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "start.bat",
                    CreateNoWindow = false
                }
            };

            proc.Start();
            Process.GetCurrentProcess().Kill();

            return NoContent();
        }

        /// <summary>
        /// git pull > build solution > drop database > add migration > update database > start service
        /// </summary>
        [HttpPost("aio")]
        public async Task<IActionResult> DropAddUpdateAsync()
        {
            var proc = new Process
            {
                StartInfo =
                {
                    FileName = "drop.bat",
                    CreateNoWindow = false
                }
            };

            proc.Start();
            Process.GetCurrentProcess().Kill();

            return NoContent();
        }
    }
}