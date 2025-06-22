using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Data
{
    public static class DbInitializer
    {
        public static void Seed(EventEasePOE2_0Context context)
        {
            if (!context.EventTypes.Any())
            {
                context.EventTypes.AddRange(
                    new EventType { Name = "Conference", Description = "Business conference" },
                    new EventType { Name = "Wedding", Description = "Wedding ceremony or reception" },
                    new EventType { Name = "Concert", Description = "Musical concert" }
                );
                context.SaveChanges();
            }
        }
    }
}