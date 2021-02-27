using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KaderService.Services.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KaderService.Services.Models;
using KaderService.Services.Services;

namespace KaderService.Controllers
{
    [Route("api/groups")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly KaderContext _context;
        private readonly GroupsService _service;

        public GroupsController(KaderContext context,GroupsService service)
        {
            _context = context;
            _service = service;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroupsAsync()
        {
            return Ok(await _service.GetGroupsAsync());
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroupAsync(string id)
        {
            var group = await _service.GetGroupAsync(id);

            if (group == null)
            {
                return NotFound();
            }

            return Ok(group);
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroupAsync(string id, Group group)
        {
            await _service.UpdateGroupAsync(id, group);
            return NoContent();
        }

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroupAsync(Group group)
        {
            await _service.CreateGroupAsync(group);

            return CreatedAtAction("GetGroupsAsync", new { id = group.Id }, group);
        }

        // DELETE: api/Groups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupAsync(string id)
        {
            await _service.DeleteGroupAsync(id);

            return NoContent();
        }

        
    }
}
