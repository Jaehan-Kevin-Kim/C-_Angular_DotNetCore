
using System.Collections.Generic;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers
{
  // GET / items

  [ApiController]
  // [Route("[controller]")]
  [Route("items")]
  public class ItemsController: ControllerBase
  {
    private readonly InMemItemsRepository repository;

  public ItemsController(){
    repository = new InMemItemsRepository();
  }

  // GET /items
  [HttpGet]

  public IEnumerable<Item> GetItems()
  {
    var items = repository.GetItems();
    return items;
  }
  }
}