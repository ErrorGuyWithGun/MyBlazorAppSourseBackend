using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using WebApplication1.Models.Authentication;
using WebApplication1.Models.Inventory;

namespace WebApplication1.Controllers
{
    [EnableCors("AllowLocalhost7170")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase 
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<InventoryController> _logger;


        public InventoryController(
            UserManager<User> userManager,
            AppDbContext appDbContext,
            ILogger<InventoryController> logger
        )
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
            _logger = logger;
            ;
        }

        
        [HttpPost("InventoryUserAccess/{userId}")]
        public async Task<IActionResult> CreateUserAccess([FromBody] InventoryAccessModel inventoryAccess, string userId) 
        {
            var user = await _userManager.FindByIdAsync(inventoryAccess.userId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status="Error", Message="User not found"});
            }
            var userAccess = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "User not found" });
            }

            var inventory = _appDbContext.InventoryModel.FirstOrDefault(i => i.Id == inventoryAccess.inventoryId);
            if (inventory == null) {
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Inventory not found" });
            }

            var accessInventory = _appDbContext.InventoryAccessModel.Where(i => i.inventoryId == inventoryAccess.inventoryId && i.userId == inventoryAccess.userId);
            if (accessInventory == null)
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed,
                   new Response { Status = "Error", Message = "User Don't Have Access" });
            }

            await _appDbContext.InventoryAccessModel.AddAsync( new InventoryAccessModel { inventoryId = inventoryAccess.inventoryId, userId = userId});
            await _appDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted,
                   new Response { Status = "Error", Message = "User Get Access" });
        }

        [Authorize]
        [HttpPost("CreateInventory")]
        public async Task<IActionResult> CreateNewInventory([FromBody] InventoryModel inventory)
        {
            var userExist = await _userManager.FindByIdAsync(inventory.userId);
            if (userExist == null)
            {
                return StatusCode(StatusCodes.Status404NotFound,
                        new Response { Status = "Error", Message = "User not found" });
            }

            var nameReq = _appDbContext.InventoryModel.FirstOrDefault(x => x.Name == inventory.Name);
            if (nameReq != null)
                return StatusCode(StatusCodes.Status400BadRequest,
                        new Response { Status = "Error", Message = "Inventory name is required" });

            
            inventory.userId = inventory.userId;
            inventory.Id = Guid.NewGuid();
            inventory.isPublic = inventory.isPublic;
            if (inventory.categoryId == null)
                inventory.categoryId = new Guid("00000000-0000-0000-0000-000000000004");

            await _appDbContext.InventoryModel.AddAsync(inventory);
            await _appDbContext.InventoryAccessModel.AddAsync(new InventoryAccessModel { inventoryId=inventory.Id, userId = inventory.userId });
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK,
                        new Response { Status = "Success", Message = "Inventary create" });

        }

       
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetInventory(string userId, [FromQuery] string searchTerm = null, [FromQuery] string sortBy = null)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "User Not Found" });

            IQueryable<InventoryModel> inventory = _appDbContext.InventoryModel.Where(i => i.userId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                inventory = inventory.Where(i => i.Name.ToLower().Contains(searchTerm));
            }

            switch(sortBy)
            { case "Name":
                    inventory = inventory.OrderBy(p=>p.Name);
                    break;
              case "categoryId":
                    inventory = inventory.OrderBy(p=>p.categoryId);
                    break;
              case "userId":
                    inventory = inventory.OrderBy(p=>p.userId);
                    break;
                default:
                    break;
            }
            return Ok(inventory);
        }

        [Authorize]
        [HttpPut("EditInventory")]
        public async Task<IActionResult> EditInventory([FromBody] InventoryModel inventory)
        {
            var inventorySearch = await _appDbContext.InventoryModel.FindAsync(inventory.Id);
            if (inventorySearch == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Inventory Not Found" });

            var editAccess = _appDbContext.InventoryAccessModel.FirstOrDefault(u => u.userId == inventory.userId && u.inventoryId == inventory.Id);
            if (editAccess == null)
                return StatusCode(StatusCodes.Status406NotAcceptable,
                    new Response { Status = "Error", Message = "You Don't Have Access" });

            inventorySearch.Name = inventory.Name;
            inventorySearch.isPublic = inventory.isPublic;
            inventorySearch.categoryId = inventory.categoryId;
      
            await _appDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted,
                new Response { Status = "Success", Message = "Inventory Edit" });
        }

        [Authorize]
        [HttpDelete("DeleteInventory/{inventoryId}/{userId}")]
        public async Task<IActionResult> DeleteInventory(Guid inventoryId, string userId)
        {
            var inventorySearch = await _appDbContext.InventoryModel.FindAsync(inventoryId);
            if (inventorySearch == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Inventory Not Found" });

            var deleteAccess = _appDbContext.InventoryAccessModel.FirstOrDefault(u => u.userId == userId && u.inventoryId == inventoryId);
            if (deleteAccess == null)
                return StatusCode(StatusCodes.Status406NotAcceptable,
                    new Response { Status = "Error", Message = "You Don't Have Access" });

            List<DiscussionModel> inventoryDiscussin = await _appDbContext.DiscussionModel
                                                        .Where(d => d.inventoryId == inventoryId)
                                                        .ToListAsync();
            if (inventoryDiscussin != null)
            {
                _appDbContext.DiscussionModel.RemoveRange(inventoryDiscussin);
            }

            List<ItemModel> inventoryItem = await _appDbContext.itemModel
                                            .Where(i =>i.inventoryId == inventoryId)
                                            .ToListAsync();
            if (inventoryItem != null)
            {
                 _appDbContext.itemModel.RemoveRange(inventoryItem);
            }
            _appDbContext.InventoryModel.Remove(inventorySearch);
            await _appDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status202Accepted,
                new Response { Status = "Success", Message = "Inventory Delete" });
        }

        [Authorize(Roles ="Admin")]
        [HttpGet("GetAllInventory")]
        public async Task<IActionResult> GetAllInventory([FromQuery] string searchTerm = null, [FromQuery] string sortBy = null)
        {

            IQueryable<InventoryModel> inventory =  _appDbContext.InventoryModel;
       
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                inventory = inventory.Where(i => i.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            switch(sortBy)
            { case "Name":
                    inventory = inventory.OrderBy(p=>p.Name);
                    break;
              case "categoryId":
                    inventory = inventory.OrderBy(p=>p.categoryId);
                    break;
              case "userId":
                    inventory = inventory.OrderBy(p=>p.userId);
                    break;
                default:
                    break;
            }

            return Ok(inventory);
        }

        [AllowAnonymous]
        [HttpGet("GetPublicInventory")]
        public async Task<IActionResult> GetAllPublicInventory([FromQuery] string searchTerm = null, [FromQuery] string sortBy = null)
        {
            IQueryable<InventoryModel> inventoriesPublic = _appDbContext.InventoryModel.Where(i => i.isPublic == true);
          
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                inventoriesPublic = inventoriesPublic.Where(i => i.Name.ToLower().Contains(searchTerm));
            }

            switch (sortBy)
            {
                case "Name":
                    inventoriesPublic = inventoriesPublic.OrderBy(p => p.Name);
                    break;
                case "categoryId":
                    inventoriesPublic = inventoriesPublic.OrderBy(p => p.categoryId);
                    break;
                case "userId":
                    inventoriesPublic = inventoriesPublic.OrderBy(p => p.userId);
                    break;
                default:
                    break;
            }
            return Ok(inventoriesPublic);
        }

        [AllowAnonymous]
        [HttpGet("GetItem/{inventoryId}")]
        public async Task<IActionResult> GetItem(Guid inventoryId)
        {

            List<ItemModel> itemReturn = new List<ItemModel>();
            List<ItemModel> itemList =  _appDbContext.itemModel.ToList();
            foreach (var item in itemList)
            {
                if(item.inventoryId == inventoryId)
                    itemReturn.Add(item);
            }
            return Ok(itemReturn);
        }

        [Authorize]
        [HttpPost("CreateItem")]
        public async Task<IActionResult> CreateItem([FromBody] ItemModel item)
        {
            var inventoryExist = _appDbContext.InventoryModel.FirstOrDefault(inv => inv.Id == item.inventoryId);
            if(inventoryExist == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Don't Have Access To This Inventory" });

            item.Id = Guid.NewGuid();

            await _appDbContext.itemModel.AddAsync(item);
            await _appDbContext.SaveChangesAsync();

            return StatusCode(StatusCodes.Status200OK,
                        new Models.Response { Status = "Success", Message = "Inventary create" });

        }

        [Authorize]
        [HttpPut("EditItem")]
        public async Task<IActionResult> EditItem([FromBody] ItemModel item)
        {
            var findInventory = await _appDbContext.InventoryModel.FindAsync(item.inventoryId);
            if (findInventory == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Don't Have Access To This Inventory" });

            var findItem = await _appDbContext.itemModel.FindAsync(item.Id);
            if(findItem == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Item Not Found" });

            findItem.Title = item.Title;
            findItem.Price = item.Price;
            findItem.Description = item.Description;
            findItem.inventoryId = item.inventoryId;
            await _appDbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status200OK,
                new Models.Response { Status = "Success", Message = "Item Edit" });
        }

        [Authorize]
        [HttpDelete("DeleteItem/{inventoryId}/{itemId}")]
        public async Task<IActionResult> DeleteItem(Guid inventoryId, Guid itemId)
        {
            var findInventory = await _appDbContext.InventoryModel.FindAsync(inventoryId);
            if (findInventory == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Don't Have Access To This Inventory" });

            var findItem = await _appDbContext.itemModel.FindAsync(itemId);
            if (findItem == null)
                return StatusCode(StatusCodes.Status404NotFound,
                    new Response { Status = "Error", Message = "Item Not Found" });
            _appDbContext.itemModel.Remove(findItem);
            _appDbContext.SaveChanges();
            return StatusCode(StatusCodes.Status200OK,
                new Models.Response { Status = "Success", Message = "Item Is Delete" });
        }

        [AllowAnonymous]
        [HttpGet("GetCategory/{categoryId}")]
        public async Task<IActionResult> GetCategory(Guid categoryId)
        {
            CategoryModel category = await _appDbContext.CategoryModel.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
                return Ok("Null");
            return Ok(category.Name);
        }

        [AllowAnonymous]
        [HttpGet("GetAllCategory")]
        public async Task<IActionResult> GetAllCategory()
        {
            List<CategoryModel> category = _appDbContext.CategoryModel.ToList();
            if (category == null) 
                return Ok(new List<CategoryModel>());

            return Ok(category);
        }


        [AllowAnonymous]
        [HttpGet ("GetDiscussion/{inventoryId}")]
        public async Task<IActionResult> GetDiscussion(Guid inventoryId)
        {
            List<DiscussionModel> returnMessage = new List<DiscussionModel>();
            List<DiscussionModel> discussion = _appDbContext.DiscussionModel.ToList();
            foreach (var message in discussion)
            {
                if (message.inventoryId == inventoryId)
                    returnMessage.Add(message);
            }
            return Ok(returnMessage);
        }

        [Authorize]
        [HttpPost ("CreateDiscussion")]
        public async Task<IActionResult> CreateDiscussion([FromBody] DiscussionModel discussion)
        {
            var user = await _userManager.FindByIdAsync(discussion.userId);
            if(user == null)
                return StatusCode(StatusCodes.Status404NotFound,
                  new Response { Status = "Error", Message = "User Not Found" });

            var inventory = await _appDbContext.InventoryModel.FirstOrDefaultAsync(i => i.Id == discussion.inventoryId);
            if(inventory == null)
                return StatusCode(StatusCodes.Status404NotFound,
                  new Response { Status = "Error", Message = "Inventory Not Found" });


            var response = await _appDbContext.DiscussionModel.AddAsync(discussion);
            if (response != null)
            {
                await _appDbContext.SaveChangesAsync();
                return StatusCode(StatusCodes.Status201Created,
                    new Response { Status = "Success", Message = "Comment is Create" });
            }
            return StatusCode(StatusCodes.Status400BadRequest,
                  new Response { Status = "Error", Message = "Comment is Not Create" });
        }

        
    }
}