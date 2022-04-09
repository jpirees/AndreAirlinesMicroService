﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Airports.API.Data;
using Models.Entities;
using Airports.API.Repositories;

namespace Airports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AirportsController : ControllerBase
    {
        private readonly AirportsAPIContext _context;
        private readonly AirportRepository _dapperContext;

        public AirportsController(AirportsAPIContext context, AirportRepository dapperContext)
        {
            _context = context;
            _dapperContext = dapperContext;
        }

        // GET: api/Airports
        // Dapper
        [HttpGet]
        public async Task<ActionResult<List<Airport>>> GetAirport()
        {
            return await _dapperContext.ToListAsync();
        }

        // GET: api/Airports
        // Entity Framework
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Airport>>> GetAirport()
        //{
        //    return await _context.Airport.ToListAsync();
        //}

        // GET: api/Airports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Airport>> GetAirport(int id)
        {
            var airport = await _context.Airport.FindAsync(id);

            if (airport == null)
            {
                return NotFound("Aeroporto não encontrado.");
            }

            return airport;
        }

        [HttpGet("Code/{code}")]
        public async Task<ActionResult<Airport>> GetAirportByCode(string code)
        {
            var airport = await _context.Airport.Where(a => a.Code == code).FirstOrDefaultAsync();

            if (airport == null)
            {
                return NotFound("Aeroporto não encontrado.");
            }

            return airport;
        }

        // PUT: api/Airports/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAirport(int id, Airport airport)
        {
            if (id != airport.Id)
            {
                return BadRequest();
            }

            _context.Entry(airport).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AirportExists(id))
                {
                    return NotFound("Aeroporto não encontrado.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Airports
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Airport>> PostAirport(Airport airport)
        {
            _context.Airport.Add(airport);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAirport", new { id = airport.Id }, airport);
        }

        // DELETE: api/Airports/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAirport(int id)
        {
            var airport = await _context.Airport.FindAsync(id);
            if (airport == null)
            {
                return NotFound("Aeroporto não encontrado.");
            }

            _context.Airport.Remove(airport);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AirportExists(int id)
        {
            return _context.Airport.Any(e => e.Id == id);
        }
    }
}
