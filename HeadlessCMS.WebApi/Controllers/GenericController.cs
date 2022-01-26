using AutoMapper;
using HeadlessCMS.Domain.Dtos;
using HeadlessCMS.Domain.Entities;
using HeadlessCMS.Persistence;
using HeadlessCMS.Persistence.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HeadlessCMS.WebApi.Controllers
{
    public abstract class GenericController<TEntity, TEntityDto> : ControllerBase
        where TEntity : BaseEntity
        where TEntityDto : BaseEntityDto
    {
        protected DbSet<TEntity> genericDbSet;
        protected ApplicationDbContext applicationDbContext;
        protected IBaseEntityRepository baseEntityRepository;
        protected IMapper _mapper;

        protected GenericController(ApplicationDbContext applicationDbContext, IBaseEntityRepository baseEntityRepository, IMapper mapper)
        {
            this.applicationDbContext = applicationDbContext;
            this.baseEntityRepository = baseEntityRepository;
            applicationDbContext.Database.EnsureCreated();
            genericDbSet = applicationDbContext.Set<TEntity>();
            _mapper = mapper;
        }

        // GET: api/<GenericController>
        [HttpGet]
        [EnableQuery]
        public virtual IEnumerable<TEntityDto> Get()
        {
            var mappedSet = _mapper.ProjectTo<TEntityDto>(genericDbSet);
            return mappedSet.ToArray();
        }

        // POST api/<GenericController>
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public virtual async Task Post([FromBody] TEntityDto value)
        {
            var entity = _mapper.Map<TEntity>(value);
            baseEntityRepository.Add(entity, User);
            await applicationDbContext.SaveChangesAsync();
        }

        // PUT api/<GenericController>/5
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public virtual async Task PutAsync([FromBody] TEntityDto value)
        {
            var entity = _mapper.Map<TEntity>(value);
            baseEntityRepository.Update(entity, User);
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