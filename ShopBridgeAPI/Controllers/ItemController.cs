using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShopBridgeAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase, IItemController
    {
        #region Private Properties.
        readonly ItemContext _context;
        #endregion

        #region Construtor.
        /// <summary>
        /// To inject Context Object.
        /// </summary>
        /// <param name="context"></param>
        public ItemController(ItemContext context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods.
        /// <summary>
        /// Get All Items.
        /// </summary>
        /// <returns>List of Items</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemDTO>>> GetItems()
        {
            return await _context.Items
                .Select(x => ItemToDTO(x))
                .ToListAsync();
        }

        /// <summary>
        /// Get Item by ID.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Item</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDTO>> GetItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return ItemToDTO(item);
        }

        /// <summary>
        /// Update an Item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="itemDTO"></param>
        /// <returns>Updated Item</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateItem(int id, ItemDTO itemDTO)
        {
            if (id != itemDTO.Id)
            {
                return BadRequest();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            item.Name = itemDTO.Name;
            item.Description = itemDTO.Description;
            item.Price = itemDTO.Price;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!ItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        /// <summary>
        /// Create an Item.
        /// </summary>
        /// <param name="itemDTO"></param>
        /// <returns>Created Item</returns>
        [HttpPost]
        public async Task<ActionResult<ItemDTO>> CreateItem(ItemDTO itemDTO)
        {
            var item = new Item
            {
                Description = itemDTO.Description,
                Name = itemDTO.Name,
                Price = itemDTO.Price
            };

            _context.Items.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetItem),
                new { id = item.Id },
                ItemToDTO(item));
        }

        /// <summary>
        /// Delete an Item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            _context.Items.Remove(item);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Private Methods.
        private bool ItemExists(int id) =>
             _context.Items.Any(e => e.Id == id);

        private static ItemDTO ItemToDTO(Item item) =>
            new()
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Discount = item.Discount,
                OnSale = item.OnSale
            };
        #endregion
    }
}
