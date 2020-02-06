using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Controllers
{
        [ApiController]
        [Route("v1/categories")]
        public class CategoryController:ControllerBase
        {

            private DataContext _context;

            public CategoryController(DataContext context){
                _context=context;
            }

            [HttpGet]
            [Route("")]
            public async Task<ActionResult<List<Category>>> Get([FromServices] DataContext context){

                    var categories = await context.Categories.ToListAsync();
                    return categories;
            }

            [HttpGet]
            [Route("{id}")]
            public async Task<ActionResult<Category>> Get(int id,[FromServices] DataContext context){

                    var category = await context.Categories.FindAsync(id);
                    if (category == null)
                    {
                        return NotFound();
                    }
                    return category;
            }

            [HttpPost]
            public async Task<ActionResult<Category>> Post(Category category, [FromServices] DataContext context){
                context.Categories.Add(category);
                await context.SaveChangesAsync();

                return CreatedAtAction("Get", new {id = category.Id}, category);
            }

            [HttpPut("{id}")]
            public async Task<IActionResult> Put(int id, Category category, [FromServices] DataContext context){

                if (id != category.Id){
                    return BadRequest();
                }

                context.Entry(category).State=EntityState.Modified;

                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    
                    if(!CategoryExists(id, context)){
                        return NotFound();
                    }else{
                        throw;
                    }
                    
                }

                return NoContent();

            } 


            [HttpDelete("{id}")]
            public async Task<ActionResult<Category>> Delete(int id, [FromServices] DataContext context){
                var category = await context.Categories.FindAsync(id);
                if(category == null){
                    return NotFound();
                }
                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return category;
            }


             private bool CategoryExists(int id,[FromServices] DataContext dataContext)
            {
                return dataContext.Categories.Any(e => e.Id == id);
            }
    
            
        }


}