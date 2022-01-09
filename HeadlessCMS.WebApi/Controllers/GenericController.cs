using HeadlessCMS.ApplicationCore.Services;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    public abstract class GenericController<TEntity> : ControllerBase where TEntity : BaseEntity
    {
        protected DbSet<TEntity> genericDbSet;
        protected ApplicationDbContext applicationDbContext;
        protected IUserService userService;

        public ApplicationDbContext Context { get; }

        protected GenericController(ApplicationDbContext applicationDbContext, IUserService userService)
        {
            this.userService = userService;
            this.applicationDbContext = applicationDbContext;
            applicationDbContext.Database.EnsureCreated();
            genericDbSet = applicationDbContext.Set<TEntity>();
        }

        // GET: api/<GenericController>
        [HttpGet]
        [EnableQuery]
        public virtual IEnumerable<TEntity> Get()
        {
            return genericDbSet.ToArray();
        }

        // GET api/<GenericController>/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public virtual ActionResult<TEntity> Get(string id)
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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public virtual async Task Post([FromBody] TEntity value)
        {
            var userId = new Guid(userService.GetCurrentUserId(User));
            value.CreatedDate = DateTime.Now;
            value.UpdatedDate = DateTime.Now;
            value.CreatedBy = userId;
            value.UpdatedBy = userId;
            genericDbSet.Add(value);
            await applicationDbContext.SaveChangesAsync();
        }

        // PUT api/<GenericController>/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public virtual async Task PutAsync([FromBody] TEntity value)
        {
            var userId = new Guid(userService.GetCurrentUserId(User));
            value.UpdatedDate = DateTime.Now;
            value.UpdatedBy = userId;
            genericDbSet.Update(value);
            await applicationDbContext.SaveChangesAsync();
        }

        // DELETE api/<GenericController>/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public virtual async Task DeleteAsync(string id)
        {
            var entity = genericDbSet.Find(id);
            if (entity != null)
            {
                genericDbSet.Remove(entity);
                await applicationDbContext.SaveChangesAsync();
            }
        }
    }
}