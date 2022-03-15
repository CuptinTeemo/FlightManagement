using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FlightManagement.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace FlightManagement.Controllers
{
    [Authorize]
    public class FlightsController : Controller
    {
        private readonly FlightManagementContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        private string _userId;
        public FlightsController(FlightManagementContext context, IMapper mapper,
            UserManager<IdentityUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        // GET: Flights
        public async Task<IActionResult> Index()
        {
            _userId = _userManager.GetUserAsync(HttpContext.User).Result.Id.ToString();
            return View(await _context.Flight.Include(x => x.Destination).Include(x => x.Departure)
                .Include(x => x.Aircraft).Where(x => x.UserId == _userId).ToListAsync());
        }

        // GET: Flights/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight.Include(x=>x.Destination)
                .Include(x=>x.Departure)
                .Include(x => x.Aircraft)
                .FirstOrDefaultAsync(m => m.ID == id);
            FlightViewModel flightVM = _mapper.Map<Flight, FlightViewModel>(flight);
            flightVM.Consumption = GetFuelConsumptionPerKM(flight.Aircraft, flightVM.Distance);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flightVM);
        }

        // GET: Flights/Create
        public async Task<IActionResult> Create()
        {
            FlightViewModel newFlightModel = new FlightViewModel
            {
                Airports = (await _context.Airport.ToListAsync()).Select(x => new SelectListItem(x.Name,x.ID.ToString())).ToList(),
                Aircrafts = (await _context.Aircraft.ToListAsync()).Select(x => new SelectListItem(x.Name, x.ID.ToString())).ToList()
            };
            return View(newFlightModel);
        }

        // POST: Flights/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Title,FlightTime, DestinationId, DepartureId, AircraftId")] FlightViewModel flightVM)
        {
            if (ModelState.IsValid)
            {
                Flight flight = _mapper.Map<FlightViewModel, Flight>(flightVM);
                flight.Destination = await _context.Airport.FindAsync(flightVM.DestinationId);
                flight.Departure = await _context.Airport.FindAsync(flightVM.DepartureId);
                flight.Aircraft = await _context.Aircraft.FindAsync(flightVM.AircraftId);
                flight.ModificationTime = DateTime.Now;
                flight.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id.ToString();
                flightVM = _mapper.Map<Flight, FlightViewModel>(flight);
                _context.Add(flight);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(flightVM);
        }

        // GET: Flights/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight.Include(x => x.Destination)
                .Include(x => x.Departure)
                .Include(x => x.Aircraft)
                .FirstOrDefaultAsync(x=>x.ID == id);
            if (flight == null)
            {
                return NotFound();
            }

            FlightViewModel flightVM = _mapper.Map<Flight, FlightViewModel>(flight);
            flightVM.Airports = (await _context.Airport.ToListAsync()).Select(x => new SelectListItem(x.Name, x.ID.ToString())).ToList();
            flightVM.Aircrafts = (await _context.Aircraft.ToListAsync()).Select(x => new SelectListItem(x.Name, x.ID.ToString())).ToList();
            return View(flightVM);
        }

        // POST: Flights/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Title,FlightTime,DestinationId, DepartureId, AircraftId, Destination")] FlightViewModel flightVM)
        {
            if (id != flightVM.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Flight flight = _mapper.Map<FlightViewModel, Flight>(flightVM);
                    flight.Destination = await _context.Airport.FindAsync(flightVM.DestinationId);
                    flight.Departure = await _context.Airport.FindAsync(flightVM.DepartureId);
                    flight.Aircraft = await _context.Aircraft.FindAsync(flightVM.AircraftId);
                    flight.ModificationTime = DateTime.Now;
                    flight.UserId = _userManager.GetUserAsync(HttpContext.User).Result.Id.ToString();
                    _context.Update(flight);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FlightExists(flightVM.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            flightVM.Airports = (await _context.Airport.ToListAsync()).Select(x => new SelectListItem(x.Name, x.ID.ToString())).ToList();
            flightVM.Aircrafts = (await _context.Aircraft.ToListAsync()).Select(x => new SelectListItem(x.Name, x.ID.ToString())).ToList();
            return View(flightVM);
        }

        // GET: Flights/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var flight = await _context.Flight
                .FirstOrDefaultAsync(m => m.ID == id);
            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        // POST: Flights/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var flight = await _context.Flight.FindAsync(id);
            _context.Flight.Remove(flight);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FlightExists(int id)
        {
            return _context.Flight.Any(e => e.ID == id);
        }

        private Decimal GetFuelConsumptionPerKM(Aircraft aircraft,decimal distance)
        {
            if(aircraft != null)
            {
                return aircraft.ConsumptionPerKm * (distance + aircraft.TakeOffDistance * aircraft.TakeOffEffort) * 100;
            }
            return 0;
        }
    }
}
