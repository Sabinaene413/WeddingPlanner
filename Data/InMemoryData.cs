using WeddingPlanner.Api.Models;

namespace WeddingPlanner.Api.Data
{
    public class InMemoryData
    {
        public static List<Wedding> Weddings = new()
        {
            new Wedding { Id = 1, Title = "Nunta Sabina & Razvan", Date = new DateTime(2024, 6, 15), Location = "Bucuresti" },
            new Wedding { Id = 2, Title = "Nunta Maria & Alex", Date = new DateTime(2024, 9, 10), Location = "Cluj" },
            new Wedding { Id = 3, Title = "Nunta Ioana & Vlad", Date = new DateTime(2024, 7, 20), Location = "Timisoara" },
            new Wedding { Id = 4, Title = "Nunta Andreea & Mihai", Date = new DateTime(2024, 10, 5), Location = "Brasov" },
            new Wedding { Id = 5, Title = "Nunta Elena & Catalin", Date = new DateTime(2024, 8, 30), Location = "Constanta" }
        };

        public static List<WeddingTask> WeddingTasks = new()
        {
            // Tasks pentru nunta 1
            new WeddingTask { Id = 1, Title = "Programare coafor", IsCompleted = false, WeddingId = 1 },
            new WeddingTask { Id = 2, Title = "Rezervare sala", IsCompleted = true, WeddingId = 1 },
            new WeddingTask { Id = 3, Title = "Alegere fotograf", IsCompleted = false, WeddingId = 1 },

            // Tasks pentru nunta 2
            new WeddingTask { Id = 4, Title = "Alegere meniu", IsCompleted = true, WeddingId = 2 },
            new WeddingTask { Id = 5, Title = "Trimis invitatii", IsCompleted = false, WeddingId = 2 },
            new WeddingTask { Id = 6, Title = "Proba rochii", IsCompleted = true, WeddingId = 2 },

            // Tasks pentru nunta 3
            new WeddingTask { Id = 7, Title = "Contract muzica", IsCompleted = false, WeddingId = 3 },
            new WeddingTask { Id = 8, Title = "Decor sala", IsCompleted = false, WeddingId = 3 },

            // Tasks pentru nunta 4
            new WeddingTask { Id = 9, Title = "Transport invitati", IsCompleted = false, WeddingId = 4 },
            new WeddingTask { Id = 10, Title = "Test meniu", IsCompleted = true, WeddingId = 4 },

            // Tasks pentru nunta 5
            new WeddingTask { Id = 11, Title = "Verificat hoteluri", IsCompleted = true, WeddingId = 5 },
            new WeddingTask { Id = 12, Title = "Decor floral", IsCompleted = false, WeddingId = 5 },
            new WeddingTask { Id = 13, Title = "Confirmat DJ", IsCompleted = false, WeddingId = 5 }
        };

        // Optional: metode de incrementare Id-uri pentru POST
        public static int NextWeddingId() => Weddings.Any() ? Weddings.Max(w => w.Id) + 1 : 1;
        public static int NextTaskId() => WeddingTasks.Any() ? WeddingTasks.Max(t => t.Id) + 1 : 1;
    }
}
