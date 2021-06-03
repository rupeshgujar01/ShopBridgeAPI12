using Microsoft.AspNetCore.Mvc;
using ShopBridgeAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Controllers
{
    public interface IItemController
    {
        Task<ActionResult<ItemDTO>> CreateItem(ItemDTO itemDTO);
        Task<IActionResult> DeleteItem(int id);
        Task<ActionResult<ItemDTO>> GetItem(int id);
        Task<ActionResult<IEnumerable<ItemDTO>>> GetItems();
        Task<IActionResult> UpdateItem(int id, ItemDTO itemDTO);
    }
}