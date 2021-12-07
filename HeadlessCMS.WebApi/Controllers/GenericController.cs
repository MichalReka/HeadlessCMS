using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    public abstract class GenericController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected DbSet<TEntity> genericDbSet;

        protected GenericController(ApplicationDbContext dbContext)
        {
        }

        // GET: api/<GenericController>
        [HttpGet]
        public IEnumerable<TEntity> Get()
        {
            return genericDbSet.ToArray();
        }

        // GET api/<GenericController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<TEntity> Get(string id)
        {
            try
            {
                TEntity? entity = genericDbSet.Find(Guid.Parse(id));

                if (entity == null)
                {
                    return NotFound();
                }
                return entity;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        // POST api/<GenericController>
        [HttpPost]
        public void Post([FromBody] TEntity value)
        {
            genericDbSet.Add(value);
        }

        // PUT api/<GenericController>/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] TEntity value)
        {
            genericDbSet.Update(value);
        }

        // DELETE api/<GenericController>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            var entity = genericDbSet.Find(id);
            if(entity != null)
            {
                genericDbSet.Remove(entity);
            }
        }
    }
}
