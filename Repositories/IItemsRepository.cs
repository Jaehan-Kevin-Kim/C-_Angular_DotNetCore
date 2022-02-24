using System;
using Catalog.Entities;

namespace Catalog.Repositories
{
  public interface IItemsRepository
  {
    // Item GetItemAsync(Guid id);
    // IEnumerable<Item> GetItemsAsync();
    // void CreateItemAsync(Item item);
    // void UpdateItemAsync(Item item);
    // void DeleteItemAsync(Guid id);

    // Async를 위해 아래와 같이 변경
    Task<Item> GetItemAsync(Guid id);
    Task<IEnumerable<Item>> GetItemsAsync();
    Task CreateItemAsync(Item item);
    Task UpdateItemAsync(Item item);
    Task DeleteItemAsync(Guid id);

  }
}