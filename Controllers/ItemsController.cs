using System.Collections.Generic;
using System.Linq;
using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
  // GET / items

  [ApiController]
  // [Route("[controller]")]
  [Route("items")]
  public class ItemsController : ControllerBase
  {
    // private readonly InMemItemsRepository repository;
    private readonly IItemsRepository repository;

    public ItemsController(IItemsRepository repository)
    {
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

      if (item is null)
      {
        return NotFound();
      }

      return item.AsDto();
    }

    // POST /items
    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreatedDate = DateTimeOffset.UtcNow

      };

      repository.CreateItem(item);

      return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());


    }

    // PUT /items/{id}
    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
    {
      var existingItem = repository.GetItem(id);
      if (existingItem is null)
      {
        return NotFound();
      }

      Item updateItem = existingItem with
      {
        Name = itemDto.Name,
        Price = itemDto.Price
      };

      repository.UpdateItem(updateItem);

      return NoContent();
    }
  }



}