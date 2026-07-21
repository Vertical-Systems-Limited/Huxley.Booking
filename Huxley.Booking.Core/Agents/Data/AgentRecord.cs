using Huxley.Booking.Core.Agents.Contracts;

namespace Huxley.Booking.Core.Agents.Data;

public class AgentRecord
{
    public string TarscInitials { get; set; }
    public string Description { get; set; }
}

public static class AgentRecordExtensions
{
    public static AgentViewDto MapToDto(this AgentRecord agent)
    {
        return new AgentViewDto()
        {
            Initials = agent.TarscInitials,
            Name = agent.Description
        };
    }
}