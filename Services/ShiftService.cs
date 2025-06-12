using Shiftly.Models;
using Shiftly.Repositories;

namespace Shiftly.Services
{
    public class ShiftService
    {
        private readonly ShiftRepository _shiftRepo;

        public ShiftService(ShiftRepository shiftRepo)
        {
            _shiftRepo = shiftRepo;
        }

        // Przykładowa logika: sprawdzenie, czy użytkownik ma nakładające się zmiany
        public async Task<bool> IsUserAvailable(string userId, DateTime start, DateTime end)
        {
            var allShifts = await _shiftRepo.GetAllAsync();
            return !allShifts.Any(s =>
                s.UserId == userId && start < s.End && end > s.Start // nakładają się
            );
        }

        // Możesz dodać inne metody biznesowe, np. automatyczne przydzielanie zmian itp.
    }
}