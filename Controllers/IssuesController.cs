using Microsoft.AspNetCore.Mvc;
using ModelBindingSample.Models;
using ModelBindingSample.Services;
using MongoDB.Driver;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ModelBindingSample.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly IMongoContext<Issue> _context;
        public IssuesController(IMongoContext<Issue> context) => _context = context;
        
        /// <summary>
        /// Get issue with id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<Issue> Get(string id) => (await _context.FindAsync(Builders<Issue>.Filter.Eq(x => x.Id, id))).FirstOrDefault() ?? new Issue();

        /// <summary>
        /// Get all issues
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ICollection<Issue>> Get() => await _context.FindAsync(Builders<Issue>.Filter.Empty);

        /// <summary>
        /// Insert a new issue
        /// </summary>
        /// <param name="issue"></param>
        [HttpPost]
        public async void Post(Issue issue) => await _context.InsertOneAsync(issue);

        /// <summary>
        /// Updates an existing issue
        /// </summary>
        /// <param name="id"></param>
        /// <param name="issue"></param>
        [HttpPut("{id}")]
        public async void Put(string id, [FromBody] Issue issue) => await _context.UpdateOneAsync(id, issue);

        /// <summary>
        /// Deletes an existing issue
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public async void Delete(string id) => await _context.DeleteOneAsync(Builders<Issue>.Filter.Eq(x => x.Id, id));
            
     }
}
