using System.Linq;

using System.Collections.Generic;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;
using Catalog.Dtos;

namespace Catalog.Controllers
{
  // GET / items

  [ApiController]
  // [Route("[controller]")]
  [Route("items")]
  public class ItemsController: ControllerBase
  {
    // private readonly InMemItemsRepository repository;
    private readonly IItemsRepository repository;

  public ItemsController(IItemsRepository repository){
    this.repository = repository;
  }

  // GET /items (get all items)
  [HttpGet]

  public IEnumerable<ItemDto> GetItems()
  {
    var items = repository.GetItems().Select(item =>
    item.AsDto()
    //  new ItemDto
    // {
    //   Id = item.Id,
    //   Name = item.Name,
    //   Price = item.Price,
    //   CreatedDate = item.CreatedDate
    // }
    );
    return items;
  }

// GET /items/{id}
[HttpGet("{id}")]
  // public ActionResult<Item> GetItem(Guid id)
  public ActionResult<ItemDto> GetItem(Guid id)
  {
    // var item = repository.GetItem(id);
    var item = repository.GetItem(id);

    if (item is null){
      return NotFound();
    }

    return item.AsDto();
  }

  }
}