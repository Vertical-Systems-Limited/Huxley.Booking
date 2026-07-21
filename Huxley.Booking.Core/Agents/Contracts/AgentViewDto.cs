using MassTransit.Configuration;

namespace Huxley.Booking.Core.Agents.Contracts;

public class AgentViewDto
{
    public string Initials { get; set; }
    public string Name { get; set; }
}