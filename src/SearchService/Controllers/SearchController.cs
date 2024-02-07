using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
           
        {
            // var query = DB.Find<Item>();
            var query = DB.PagedSearch<Item, Item>();

            query.Sort(x => x.Ascending(y => y.Make));

            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(x => x.Make == searchParams.SearchTerm || x.Model == searchParams.SearchTerm || x.Color == searchParams.SearchTerm);
            }

            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(x => x.Ascending(y => y.Make)),
                "newest" => query.Sort(x => x.Descending(y => y.CreatedAt)),
                "model" => query.Sort(x => x.Ascending(y => y.Model)),
                "color" => query.Sort(x => x.Ascending(y => y.Color)),
                "year" => query.Sort(x => x.Ascending(y => y.Year)),
                "mileage" => query.Sort(x => x.Ascending(y => y.Mileage)),
                _ => query.Sort(x => x.Ascending(y => y.AuctionEnd))
            };

            query = searchParams.FilterBy switch 
            {
                "finished" => query.Match(x => x.AuctionEnd < DateTime.UtcNow),
                "endingSoon" => query.Match(x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow),
                "active" => query.Match(x => x.AuctionEnd > DateTime.UtcNow),
                _ => query
            };

            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Seller == searchParams.Seller);
            }

            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Winner == searchParams.Winner);
            }

            query.PageNumber(searchParams.PageNumber);
            query.PageSize(searchParams.PageSize);

            var result = await query.ExecuteAsync();

            return Ok(new { results = result.Results, pageCount = result.PageCount, totalCount = result.TotalCount});
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> GetItem(string id)
        {
            var item = await DB.Find<Item>().OneAsync(id);

            if (item == null)
            {
                return NotFound();
            }

            return item;
        }

        
    }
}