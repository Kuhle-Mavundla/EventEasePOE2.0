using EventEasePOE2._0.Models;

namespace EventEasePOE2._0.Data
{
    // This class helps put starting data into the database
    public static class DbInitializer
    {
        // This method adds some event types if there aren't any yet
        public static void Seed(EventEasePOE2_0Context context)
        {
            // Check if there are no event types in the database
            if (!context.EventTypes.Any())
            {
                // Add some event types to the database
                context.EventTypes.AddRange(
                    new EventType { Name = "Conference", Description = "Business conference" },
                    new EventType { Name = "Wedding", Description = "Wedding ceremony or reception" },
                    new EventType { Name = "Concert", Description = "Musical concert" }
                );

                // Save these new event types so they stay in the database
                context.SaveChanges();
            }
        }
    }
}
